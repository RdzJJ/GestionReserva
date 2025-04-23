using Core.Aggregates;

namespace Application.Commands
{
    public class CrearReservaCommand
    {
        public OfertaPersonalizada Oferta { get; set; } = new();
        public decimal MontoTotal { get; set; }
        public bool PagoCompleto { get; set; }
    }
}