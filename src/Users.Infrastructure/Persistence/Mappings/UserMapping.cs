using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Persistence.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Phone).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(c => c.FullName).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(c => c.Password).HasColumnType("VARCHAR(256)").IsRequired();
            builder.Property(c => c.IsDeleted).IsRequired();
            builder.OwnsOne(c => c.Email).Property(c => c.Address).HasColumnType("VARCHAR(150)").HasColumnName("Email").IsRequired();
            builder.OwnsOne(c => c.Document).Property(c => c.Number).HasColumnType("VARCHAR(150)").HasColumnName("Document").IsRequired();
        }
    }
}
