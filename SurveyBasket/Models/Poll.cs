﻿namespace SurveyBasket.Models;

public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}
