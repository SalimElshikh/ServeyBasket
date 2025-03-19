﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.Entities;

public sealed class Vote
{
    public int Id { get; set; }

    public int PollId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public DateTime  SubmittedOn { get; set; } = DateTime.UtcNow;


    public ApplicationUser User { get; set; } = default!;
    public Poll Poll { get; set; } = default!;
    public ICollection<VoteAnswer> Answers { get; set; } = [];
}
