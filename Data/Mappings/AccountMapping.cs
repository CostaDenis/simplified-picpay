using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using simplified_picpay.Models;

namespace simplified_picpay.Data.Mappings
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired(true);

            builder.Property(a => a.FullName)
                .HasColumnName("full_name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired(true);

            builder.Property(a => a.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasIndex(a => a.Email)
                .IsUnique(true)
                .HasDatabaseName("ux_accounts_email");

            builder.Property(a => a.PasswordHash)
                .HasColumnName("password_hash")
                .HasColumnType("varchar")
                .HasMaxLength(256)
                .IsRequired(true);

            builder.Property(a => a.CurrentBalance)
                .HasColumnName("current_balance")
                .HasColumnType("numeric(18,2)")
                .IsRequired(true);

            builder.Property(a => a.AccountType)
                .HasColumnName("account_type")
                .HasColumnType("varchar")
                .HasMaxLength(11)
                .HasConversion<string>()
                .IsRequired(true);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean")
                .IsRequired(true);
        }
    }
}