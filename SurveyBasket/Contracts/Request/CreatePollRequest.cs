﻿namespace SurveyBasket.Contracts.Request;

public record CreatePollRequest(
    string Title,
    string Description);
