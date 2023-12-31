using System;
using Webapi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webapi.Data.Map
{
    public class PlayerMap : IEntityTypeConfiguration<PlayerModel>
    {
        void IEntityTypeConfiguration<PlayerModel>.Configure(EntityTypeBuilder<PlayerModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
        }
    }
}
