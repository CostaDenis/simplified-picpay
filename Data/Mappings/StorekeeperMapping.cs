using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using simplified_picpay.Models;

namespace simplified_picpay.Data.Mappings
{
    public class StorekeeperMapping : IEntityTypeConfiguration<Storekeeper>
    {
        public void Configure(EntityTypeBuilder<Storekeeper> builder)
        {
            builder.ToTable("storekeepers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired(true);

            builder.HasOne(s => s.Account)
                .WithOne(a => a.Storekeeper)
                .HasForeignKey<Storekeeper>(s => s.AccountId)
                .HasConstraintName("fk_storekeepers_account_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.CNPJ)
                .HasColumnName("cnpj")
                .HasColumnType("varchar")
                .HasMaxLength(14)
                .IsRequired(true);

            builder.HasIndex(s => s.CNPJ)
                .IsUnique()
                .HasDatabaseName("ux_storekeepers_cnpj");
        }
    }
}