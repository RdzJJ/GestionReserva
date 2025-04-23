namespace Core.ValueObjects
{
    public class DetalleServicio
    {
        public string Proveedor { get; set; } = string.Empty;
        public string IdExterno { get; set; } = string.Empty;
        public Monto Monto { get; set; } = new();
    }
}