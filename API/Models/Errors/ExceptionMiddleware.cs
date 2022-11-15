using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Data;
using WebApplication2.Models.Errors;

namespace WebApplication2.Models;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    // private readonly DataContext _context;
    // private DbSet<ErrorViewModel> _errorList;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ICorrelationIDGenerator correlationIdGenerator, IErrorRepository errorList, DataContext dataContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {ex}", ex.ToString());
            await HandleExceptionAsync(context, ex, correlationIdGenerator, errorList, dataContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, ICorrelationIDGenerator correlationIdGenerator, IErrorRepository errorList, DataContext dataContext)
    {
        // context.Response.ContentType = "application/json";
        // context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        StackTrace stackTrace = new StackTrace(ex, true);

        int? fileLine = null;
        string? fileName = null;

        if(stackTrace.GetFrame(0) != null)
        {
            var fileDir = stackTrace.GetFrame(0).GetFileName();
            if(fileDir != null)
            {
                fileName = fileDir[(fileDir.LastIndexOf('\\') + 1)..];
                fileLine = stackTrace.GetFrame(0).GetFileLineNumber();
            }
        }

        ErrorModel errorDetails = new ErrorModel()
        {
            DateAndTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
            Id = correlationIdGenerator.Get(),
            RequestId = Activity.Current?.Id ?? context.TraceIdentifier,
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Source = ex.Source,
            File = fileName,
            Line = fileLine,
            Method = ex.TargetSite?.ToString(),
            Type = ex.GetType().Name,
            Message = ex.Message,
            StackTrace = ex.ToString().Replace("\r\n", "<br/>")
        };
        
        _logger.LogInformation("Logging error to DB");
        
        errorList.Add(errorDetails);
        await dataContext.SaveChangesAsync();
        
        context.Response.Redirect("/Home/Error");
    }

}