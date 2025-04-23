using System;

namespace Core.Entities
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public int ReservaId { get; set; }
    }
}