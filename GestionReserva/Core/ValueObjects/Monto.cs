namespace Core.ValueObjects
{
    public class Monto
    {
        public decimal Valor { get; set; }
        public string Moneda { get; set; } = "COP";
    }
}