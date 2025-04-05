﻿using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {

        builder.HasIndex(x => new {x.Content , x.PollId}).IsUnique();
        builder.Property(x => x.Content).HasMaxLength(1000);

    }
}
