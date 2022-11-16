using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Primitives;

namespace WebApplication2.Models;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationIdHeader = "Correlation-Id";
    
    //Inject the RequestDelegate 
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationIDGenerator correlationIdGenerator)
    {
        var correlationId = GetCorrelationIdTrace(context, correlationIdGenerator);
        AddCorrelationIdToResponse(context, correlationId);

        await _next(context);
    }

    private static Guid GetCorrelationIdTrace(HttpContext context, ICorrelationIDGenerator correlationIdGenerator)
    {
        if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
        {
            correlationIdGenerator.Set(Guid.Parse(correlationId));
            return Guid.Parse(correlationId);
        }
            
        return correlationIdGenerator.Get();
    }

    private static void AddCorrelationIdToResponse(HttpContext context, Guid correlationId)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(_correlationIdHeader, new []{ correlationId.ToString() });
            return Task.CompletedTask;
        });
    }
}