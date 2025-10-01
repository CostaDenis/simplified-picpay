using simplified_picpay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace simplified_picpay.Data.Mappings
{
    public class TransactionMapping : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired(true);

            builder.HasOne(t => t.Payer)
                .WithMany(a => a.Payments)
                .HasForeignKey(T => T.PayerId)
                .HasConstraintName("fk_transactions_payer_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.PayerPublicId)
                .HasColumnName("payer_public_id")
                .HasColumnType("varchar")
                .HasMaxLength(36)
                .IsRequired(true);

            builder.HasOne(t => t.Payee)
                .WithMany(a => a.Receipts)
                .HasForeignKey(t => t.PayeeId)
                .HasConstraintName("fk_transactions_payee_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.PayeePublicId)
                .HasColumnName("payee_public_id")
                .HasColumnType("varchar")
                .HasMaxLength(36)
                .IsRequired(true);

            builder.Property(t => t.Value)
                .HasColumnName("value")
                .HasColumnType("numeric(18,2)")
                .IsRequired(true);
        }
    }
}