using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Persistence.EntitiesConfigurations;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {

        builder.HasIndex(x => new { x.VoteId, x.QuestionId });

    }
}
