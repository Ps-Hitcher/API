namespace WebApplication2.Models;

public class CorrelationIdGenerator : ICorrelationIDGenerator
{
    private Guid _correlationId = Guid.NewGuid();
    
    public Guid Get()
    {
        return _correlationId;
    }

    public void Set(Guid correlationId)
    {
        _correlationId = correlationId;
    }
}