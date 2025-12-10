using System;

namespace PersonalCapital.Exceptions;

/// <summary>
///     Base exception for all PersonalCapital responses.
/// </summary>
public class PersonalCapitalException(string message) : Exception(message)
{
}