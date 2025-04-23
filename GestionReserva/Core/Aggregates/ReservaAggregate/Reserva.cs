using GestionReserva.Core.Events;
using GestionReserva.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // La Raíz del Agregado principal: Reserva. Encapsula la lógica y el estado de una reserva.
    public class Reserva : AggregateRoot // Hereda de la clase base AggregateRoot
    {
        public ReservaId Id { get; private set; } // Identificador único (VO).
        public Guid UsuarioId { get; private set; } // ID del usuario que realiza la reserva.
        public OfertaPersonalizada Oferta { get; private set; } // La oferta asociada (Entidad Contenida).
        private readonly List<Pago> _pagos = new List<Pago>(); // Lista interna de pagos (Entidades Contenidas).
        public IReadOnlyCollection<Pago> Pagos => _pagos.AsReadOnly(); // Exposición de solo lectura.
        public Voucher? Voucher { get; private set; } // El voucher emitido (Entidad Contenida, opcional).
        public EstadoReserva Estado { get; private set; } // Estado actual de la reserva (Enum).

        // Constructor privado para EF Core y reconstrucción.
        private Reserva() { }

        // Método Factoría (Factory Method) estático para crear nuevas instancias.
        // Asegura que las reservas se creen en un estado válido.
        public static Reserva CrearNueva(ReservaId id, Guid usuarioId, OfertaPersonalizada oferta, Pago pagoInicial)
        {
            // Validaciones de los parámetros de entrada...
            var reserva = new Reserva
            {
                Id = id,
                UsuarioId = usuarioId,
                Oferta = oferta,
                Estado = EstadoReserva.Pendiente
            };
            reserva.AgregarPagoInterno(pagoInicial); // Añade el pago inicial sin evento público.
            reserva.AddDomainEvent(new ReservaCreadaEvent(reserva.Id)); // Añade evento de dominio.
            return reserva;
        }

        // Método privado para añadir el pago inicial durante la creación.
        private void AgregarPagoInterno(Pago pago) { _pagos.Add(pago); }

        // Método público para registrar pagos adicionales después de la creación.
        public void AgregarPago(Pago pago)
        {
            // Validaciones: Solo en estado Pendiente, moneda correcta, no exceder total...
            _pagos.Add(pago);
            AddDomainEvent(new PagoRealizadoEvent(this.Id, pago.Id, pago.MontoPagado)); // Añade evento.
        }

        // Método para intentar confirmar la reserva basado en el estado interno.
        // Se llama internamente cuando llegan confirmaciones externas.
        public void ConfirmarSiEsPosible()
        {
            if (Estado != EstadoReserva.Pendiente) return; // Solo si está pendiente.

            // Verifica si se cumplen las condiciones (pagos y servicios confirmados externamente).
            bool pagoMinimoConfirmado = _pagos.Any(p => p.ConfirmadoExternamente);
            bool todosServiciosConfirmados = Oferta.TodosLosServiciosConfirmadosExternamente();

            if (pagoMinimoConfirmado && todosServiciosConfirmados)
            {
                this.Estado = EstadoReserva.Confirmada; // Cambia estado.
                string voucherCode = $"VCH-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"; // Genera código.
                this.Voucher = new Voucher(new VoucherId(Guid.NewGuid()), voucherCode); // Crea el voucher.
                AddDomainEvent(new ReservaConfirmadaEvent(this.Id, this.Voucher.Id)); // Añade evento.
            }
        }

        // Método público para cancelar la reserva.
        public void Cancelar()
        {
            // Validaciones: No cancelar si ya está cancelada. Lógica adicional si estaba confirmada...
            this.Estado = EstadoReserva.Cancelada; // Cambia estado.
            AddDomainEvent(new ReservaCanceladaEvent(this.Id)); // Añade evento.
        }

        // Calcula el monto restante por pagar.
        public Monto CalcularSaldoPendiente()
        {
            var costoTotal = Oferta.CalcularCostoTotal();
            var totalPagado = _pagos.Sum(p => p.MontoPagado.Valor);
            return new Monto(costoTotal.Valor - totalPagado, costoTotal.Moneda);
        }

        // Método interno llamado por el Handler de Webhook de Pagos.
        internal void ProcesarConfirmacionPagoExterno(PagoId pagoId)
        {
            if (Estado != EstadoReserva.Pendiente) return; // Ignorar si no está pendiente.
            var pago = _pagos.FirstOrDefault(p => p.Id == pagoId);
            if (pago != null && !pago.ConfirmadoExternamente)
            {
                pago.MarcarComoConfirmadoExternamente(); // Actualiza estado del pago.
                AddDomainEvent(new PagoConfirmadoExternamenteEvent(this.Id, pagoId)); // Añade evento.
                ConfirmarSiEsPosible(); // Intenta confirmar la reserva ahora.
            }
        }

        // Método interno llamado por el Handler de Webhook de Proveedores.
        internal void ProcesarConfirmacionServicioExterno(TipoServicio tipo, string descripcion)
        {
            if (Estado != EstadoReserva.Pendiente) return; // Ignorar si no está pendiente.
            bool changed = Oferta.MarcarServicioComoConfirmado(tipo, descripcion); // Actualiza estado del servicio.
            if (changed)
            {
                AddDomainEvent(new ServicioConfirmadoExternamenteEvent(this.Id, tipo, descripcion)); // Añade evento.
                if (Oferta.TodosLosServiciosConfirmadosExternamente()) // Si este era el último servicio...
                {
                    AddDomainEvent(new TodosServiciosConfirmadosExternamenteEvent(this.Id)); // Evento específico.
                    ConfirmarSiEsPosible(); // Intenta confirmar la reserva ahora.
                }
            }
        }
    }
}