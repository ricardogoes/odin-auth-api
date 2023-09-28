using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.Infra.Data.EF.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<CustomerModel>
    {
        public void Configure(EntityTypeBuilder<CustomerModel> builder)
        {
            builder.ToTable("customers")
                .HasKey(customer => customer.Id);

            builder.Property(customer => customer.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(customer => customer.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(customer => customer.Document)
                .HasColumnName("document")
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(customer => customer.StreetName)
                .HasColumnName("street_name")
                .HasMaxLength(255);

            builder.Property(customer => customer.StreetNumber)
                .HasColumnName("street_number");

            builder.Property(customer => customer.Complement)
                .HasColumnName("complement")
                .HasMaxLength(255);

            builder.Property(customer => customer.Neighborhood)
                .HasColumnName("neighborhood")
                .HasMaxLength(255);

            builder.Property(customer => customer.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(255);

            builder.Property(customer => customer.City)
                .HasColumnName("city")
                .HasMaxLength(255);

            builder.Property(customer => customer.State)
                .HasColumnName("state")
                .HasMaxLength(255);

            builder.Property(customer => customer.IsActive)
               .HasColumnName("is_active")
               .IsRequired();

            builder.Property(customer => customer.CreatedAt)
               .HasColumnName("created_at")
               .IsRequired();

            builder.Property(customer => customer.CreatedBy)
               .HasColumnName("created_by")
               .IsRequired();

            builder.Property(customer => customer.LastUpdatedAt)
               .HasColumnName("last_updated_at")
               .IsRequired();

            builder.Property(customer => customer.LastUpdatedBy)
               .HasColumnName("last_updated_by")
               .IsRequired();
        }
    }
}
