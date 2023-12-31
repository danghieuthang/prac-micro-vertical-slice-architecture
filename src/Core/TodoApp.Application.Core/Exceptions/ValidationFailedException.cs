﻿namespace TodoApp.Application.Core.Exceptions;
public class ValidationFailedException : Exception
{
    public record ValidationError(string Property, string ErrorMessage);
    public IEnumerable<ValidationError> Errors { get; private set; }

    public ValidationFailedException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
