namespace WebApplication2.Models;

public interface ICorrelationIDGenerator
{
    Guid Get();
    void Set(Guid correlationId);
}