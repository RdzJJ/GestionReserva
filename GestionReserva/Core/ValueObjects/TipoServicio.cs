namespace GestionReserva.Core.ValueObjects
{
    /// <summary>
    /// Tipos de servicios turísticos que pueden formar parte de una reserva.
    /// </summary>
    public enum TipoServicio 
    {
        /// <summary>Servicio de transporte aéreo.</summary>
        Vuelo,
        /// <summary>Servicio de hospedaje en hotel.</summary>
        Hotel,
        /// <summary>Servicio de excursión o tour guiado.</summary>
        Tour
    }

}