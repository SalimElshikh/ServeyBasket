﻿namespace SurveyBasket.Contracts.Polls;

public record PollRequest(
    string Title,
    string Sammary,
    DateOnly StartAt,
    DateOnly EndAt
    );
