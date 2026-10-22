namespace NorthWind.DomainLogs.Entities.Interfaces;

using NorthWind.DomainLogs.Entities.ValueObjects;
using NorthWind.Entities.Interfaces;

public interface IDomainLogsRepository : IUnitOfWork
{
    Task Add(DomainLog log);
}