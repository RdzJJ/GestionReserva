using System;

namespace Core.Entities
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public bool EsPagoCompleto { get; set; }
    }
}