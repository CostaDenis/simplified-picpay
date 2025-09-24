using Microsoft.EntityFrameworkCore;
using simplified_picpay.Models;

namespace simplified_picpay.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired(true);

            builder.HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<User>(u => u.AccountId)
                .HasConstraintName("fk_users_account_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.CPF)
                .HasColumnName("cpf")
                .HasColumnType("varchar")
                .HasMaxLength(11)
                .IsRequired(true);

            builder.HasIndex(u => u.CPF)
                .IsUnique()
                .HasDatabaseName("ux_users_cpf");
        }
    }
}