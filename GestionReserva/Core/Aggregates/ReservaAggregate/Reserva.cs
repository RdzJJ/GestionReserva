using GestionReserva.Core.Events;
using GestionReserva.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionReserva.Core.Aggregates.ReservaAggregate
{
    // La Ra�z del Agregado principal: Reserva. Encapsula la l�gica y el estado de una reserva.
    public class Reserva : AggregateRoot // Hereda de la clase base AggregateRoot
    {
        public ReservaId Id { get; private set; } // Identificador �nico (VO).
        public Guid UsuarioId { get; private set; } // ID del usuario que realiza la reserva.
        public OfertaPersonalizada Oferta { get; private set; } // La oferta asociada (Entidad Contenida).
        private readonly List<Pago> _pagos = new List<Pago>(); // Lista interna de pagos (Entidades Contenidas).
        public IReadOnlyCollection<Pago> Pagos => _pagos.AsReadOnly(); // Exposici�n de solo lectura.
        public Voucher? Voucher { get; private set; } // El voucher emitido (Entidad Contenida, opcional).
        public EstadoReserva Estado { get; private set; } // Estado actual de la reserva (Enum).

        // Constructor privado para EF Core y reconstrucci�n.
        private Reserva() { }

        // M�todo Factor�a (Factory Method) est�tico para crear nuevas instancias.
        // Asegura que las reservas se creen en un estado v�lido.
        public static Reserva CrearNueva(ReservaId id, Guid usuarioId, OfertaPersonalizada oferta, Pago pagoInicial)
        {
            // Validaciones de los par�metros de entrada...
            var reserva = new Reserva
            {
                Id = id,
                UsuarioId = usuarioId,
                Oferta = oferta,
                Estado = EstadoReserva.Pendiente
            };
            reserva.AgregarPagoInterno(pagoInicial); // A�ade el pago inicial sin evento p�blico.
            reserva.AddDomainEvent(new ReservaCreadaEvent(reserva.Id)); // A�ade evento de dominio.
            return reserva;
        }

        // M�todo privado para a�adir el pago inicial durante la creaci�n.
        private void AgregarPagoInterno(Pago pago) { _pagos.Add(pago); }

        // M�todo p�blico para registrar pagos adicionales despu�s de la creaci�n.
        public void AgregarPago(Pago pago)
        {
            // Validaciones: Solo en estado Pendiente, moneda correcta, no exceder total...
            _pagos.Add(pago);
            AddDomainEvent(new PagoRealizadoEvent(this.Id, pago.Id, pago.MontoPagado)); // A�ade evento.
        }

        // M�todo para intentar confirmar la reserva basado en el estado interno.
        // Se llama internamente cuando llegan confirmaciones externas.
        public void ConfirmarSiEsPosible()
        {
            if (Estado != EstadoReserva.Pendiente) return; // Solo si est� pendiente.

            // Verifica si se cumplen las condiciones (pagos y servicios confirmados externamente).
            bool pagoMinimoConfirmado = _pagos.Any(p => p.ConfirmadoExternamente);
            bool todosServiciosConfirmados = Oferta.TodosLosServiciosConfirmadosExternamente();

            if (pagoMinimoConfirmado && todosServiciosConfirmados)
            {
                this.Estado = EstadoReserva.Confirmada; // Cambia estado.
                string voucherCode = $"VCH-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"; // Genera c�digo.
                this.Voucher = new Voucher(new VoucherId(Guid.NewGuid()), voucherCode); // Crea el voucher.
                AddDomainEvent(new ReservaConfirmadaEvent(this.Id, this.Voucher.Id)); // A�ade evento.
            }
        }

        // M�todo p�blico para cancelar la reserva.
        public void Cancelar()
        {
            // Validaciones: No cancelar si ya est� cancelada. L�gica adicional si estaba confirmada...
            this.Estado = EstadoReserva.Cancelada; // Cambia estado.
            AddDomainEvent(new ReservaCanceladaEvent(this.Id)); // A�ade evento.
        }

        // Calcula el monto restante por pagar.
        public Monto CalcularSaldoPendiente()
        {
            var costoTotal = Oferta.CalcularCostoTotal();
            var totalPagado = _pagos.Sum(p => p.MontoPagado.Valor);
            return new Monto(costoTotal.Valor - totalPagado, costoTotal.Moneda);
        }

        // M�todo interno llamado por el Handler de Webhook de Pagos.
        internal void ProcesarConfirmacionPagoExterno(PagoId pagoId)
        {
            if (Estado != EstadoReserva.Pendiente) return; // Ignorar si no est� pendiente.
            var pago = _pagos.FirstOrDefault(p => p.Id == pagoId);
            if (pago != null && !pago.ConfirmadoExternamente)
            {
                pago.MarcarComoConfirmadoExternamente(); // Actualiza estado del pago.
                AddDomainEvent(new PagoConfirmadoExternamenteEvent(this.Id, pagoId)); // A�ade evento.
                ConfirmarSiEsPosible(); // Intenta confirmar la reserva ahora.
            }
        }

        // M�todo interno llamado por el Handler de Webhook de Proveedores.
        internal void ProcesarConfirmacionServicioExterno(TipoServicio tipo, string descripcion)
        {
            if (Estado != EstadoReserva.Pendiente) return; // Ignorar si no est� pendiente.
            bool changed = Oferta.MarcarServicioComoConfirmado(tipo, descripcion); // Actualiza estado del servicio.
            if (changed)
            {
                AddDomainEvent(new ServicioConfirmadoExternamenteEvent(this.Id, tipo, descripcion)); // A�ade evento.
                if (Oferta.TodosLosServiciosConfirmadosExternamente()) // Si este era el �ltimo servicio...
                {
                    AddDomainEvent(new TodosServiciosConfirmadosExternamenteEvent(this.Id)); // Evento espec�fico.
                    ConfirmarSiEsPosible(); // Intenta confirmar la reserva ahora.
                }
            }
        }
    }
}