using System;
using Webapi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Webapi.Data.Map
{
    public class MatchMap : IEntityTypeConfiguration<MatchModel>
    {
        void IEntityTypeConfiguration<MatchModel>.Configure(EntityTypeBuilder<MatchModel> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
