using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestionReserva.Core.Aggregates.ReservaAggregate;
using GestionReserva.Core.ValueObjects;
using System;

namespace GestionReserva.Infrastructure.Configuration
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas");
            builder.HasKey(r => r.Id);

            // Mapea ReservaId (Guid) directamente
            builder.Property(r => r.Id)
                   .HasConversion(id => id.Value, v => new ReservaId(v))
                   .ValueGeneratedNever()
                   .HasColumnName("ReservaId");

            builder.Property(r => r.UsuarioId).IsRequired();

            builder.Property(r => r.Estado)
                   .IsRequired()
                   .HasConversion(e => e.ToString(), s => (EstadoReserva)Enum.Parse(typeof(EstadoReserva), s))
                   .HasMaxLength(50);

            // --- OfertaPersonalizada Owned ---
            builder.OwnsOne(r => r.Oferta, ofertaBuilder =>
            {
                ofertaBuilder.WithOwner();

                ofertaBuilder.OwnsOne(o => o.Destino, db =>
                {
                    db.Property(d => d.Nombre).HasColumnName("DestinoNombre").IsRequired();
                    db.Property(d => d.Pais).HasColumnName("DestinoPais").IsRequired();
                });

                ofertaBuilder.OwnsOne(o => o.Fechas, fb =>
                {
                    fb.Property(f => f.Inicio).HasColumnName("FechaInicio").IsRequired();
                    fb.Property(f => f.Fin).HasColumnName("FechaFin").IsRequired();
                });

                // DetallesServicio (Value Object) Owned Many
                ofertaBuilder.OwnsMany(o => o.DetallesServicio, detalleBuilder =>
                {
                    detalleBuilder.WithOwner().HasForeignKey("ReservaOfertaId");

                    detalleBuilder.Property<int>("Id");
                    detalleBuilder.HasKey("Id");

                    // Aquí mapeamos el Precio (Monto) para que EF lo conozca:
                    detalleBuilder.OwnsOne(d => d.Precio, mb =>
                    {
                        mb.Property(m => m.Valor)
                          .HasColumnName("PrecioValor")
                          .IsRequired();

                        mb.Property(m => m.Moneda)
                          .HasColumnName("PrecioMoneda")
                          .IsRequired()
                          .HasMaxLength(3);
                    });

                    detalleBuilder.Property(d => d.Descripcion).HasColumnName("Descripcion").IsRequired();
                    detalleBuilder.Property(d => d.Tipo).HasColumnName("TipoServicio").IsRequired();
                    detalleBuilder.Property(d => d.ConfirmadoExternamente)
                                  .IsRequired()
                                  .HasDefaultValue(false);

                    detalleBuilder.ToTable("ReservaDetallesServicio");
                });

                ofertaBuilder.Navigation(o => o.DetallesServicio)
                              .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Navigation(r => r.Oferta).IsRequired();

            // --- Pagos (Owned Entities) ---
            builder.OwnsMany(r => r.Pagos, pagoBuilder =>
            {
                pagoBuilder.WithOwner().HasForeignKey("ReservaId");

                // Usamos Pago.Id (PagoId / Guid) como PK
                pagoBuilder.HasKey(p => p.Id);

                pagoBuilder.Property(p => p.Id)
                           .HasConversion(id => id.Value, v => new PagoId(v))
                           .ValueGeneratedNever()
                           .HasColumnName("PagoId")
                           .IsRequired();

                // MontoPagado (VO)
                pagoBuilder.OwnsOne(p => p.MontoPagado, mb =>
                {
                    mb.Property(m => m.Valor)
                      .HasColumnName("MontoValor")
                      .IsRequired();

                    mb.Property(m => m.Moneda)
                      .HasColumnName("MontoMoneda")
                      .IsRequired()
                      .HasMaxLength(3);
                });
                pagoBuilder.Navigation(p => p.MontoPagado).IsRequired();

                pagoBuilder.Property(p => p.Tipo)
                           .HasColumnName("TipoPago")
                           .IsRequired();

                pagoBuilder.Property(p => p.FechaPago)
                           .HasColumnName("FechaPago")
                           .IsRequired();

                pagoBuilder.Property(p => p.ConfirmadoExternamente)
                           .HasColumnName("PagoConfirmadoExternamente")
                           .IsRequired()
                           .HasDefaultValue(false);

                pagoBuilder.ToTable("ReservaPagos");
            });
            builder.Navigation(r => r.Pagos)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            // --- Voucher (Owned, optional) ---
            builder.OwnsOne(r => r.Voucher, voucherBuilder =>
            {
                voucherBuilder.WithOwner();

                voucherBuilder.Property(v => v.Id)
                              .HasConversion(id => id.Value, v => new VoucherId(v))
                              .ValueGeneratedNever()
                              .HasColumnName("VoucherId")
                              .IsRequired();
                voucherBuilder.Property(v => v.Codigo)
                              .HasColumnName("VoucherCodigo")
                              .IsRequired();
                voucherBuilder.Property(v => v.FechaEmision)
                              .HasColumnName("VoucherFechaEmision")
                              .IsRequired();

                voucherBuilder.ToTable("ReservaVouchers");
            });

            // Ignora los events
            builder.Ignore(r => r.DomainEvents);
        }
    }
}
