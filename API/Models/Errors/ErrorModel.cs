using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Data;
using WebApplication2.Models.Errors;

namespace WebApplication2.Models;

public class ErrorModel
{
    public string DateAndTime { get; set; }
    public Guid Id { get; set; }
    public string? RequestId { get; set; }
    public int StatusCode { get; set; }
    public string? Source { get; set; }
    public string? File { get; set; }
    public int? Line { get; set; }
    public string? Type { get; set; }
    public string? Method { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}