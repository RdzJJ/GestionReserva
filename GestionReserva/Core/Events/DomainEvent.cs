using MediatR; // Para heredar de INotification
using System;

namespace GestionReserva.Core.Events
{
    // Clase base abstracta para todos los eventos de dominio.
    // Implementa INotification de MediatR para poder despacharlos fácilmente.
    public abstract class DomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow; // Fecha/hora de ocurrencia.
    }

}