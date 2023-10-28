using System;
using api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Map
{
    public class MatchMap: IEntityTypeConfiguration<MatchModel>
    {
         void IEntityTypeConfiguration<MatchModel>.Configure(EntityTypeBuilder<MatchModel> builder)
        {
           builder.HasKey(x => x.Id);
        }
    }
}
