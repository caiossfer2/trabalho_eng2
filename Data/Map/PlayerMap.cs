using System;
using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Map
{
    public class PlayerMap : IEntityTypeConfiguration<PlayerModel>
    {
        void IEntityTypeConfiguration<PlayerModel>.Configure(EntityTypeBuilder<PlayerModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
}
