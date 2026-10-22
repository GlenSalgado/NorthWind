namespace NorthWind.DomainLogs.Entities.Interfaces;

using NorthWind.DomainLogs.Entities.ValueObjects;

public interface IDomainLogger
{
    Task LogInformation(DomainLog log);
}