using Core.ValueObjects;

namespace Core.Aggregates
{
    public class OfertaPersonalizada
    {
        public int Id { get; set; }
        public DetalleServicio? Vuelo { get; set; }
        public DetalleServicio? Hotel { get; set; }
        public DetalleServicio? Tour { get; set; }
        public Destino Destino { get; set; } = new();
        public FechasViaje Fechas { get; set; } = new();
    }
}