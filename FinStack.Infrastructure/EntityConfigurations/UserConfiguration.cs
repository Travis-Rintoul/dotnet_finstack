using FinStack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinStack.Infrastructure.EntityConfigurations
{
       public class UserConfiguration : IEntityTypeConfiguration<User>
       {
              public void Configure(EntityTypeBuilder<User> builder)
              {
                     builder.ToTable("Users");

                     // Primary key
                     builder.HasKey(u => u.Id);

                     // Properties
                     builder.Property(u => u.Id)
                            .ValueGeneratedOnAdd();

                     builder.Property(u => u.CreatedDate)
                            .HasDefaultValueSql("CURRENT_TIMESTAMP");
              }
       }
}
