using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace WebApplication2.Utilities
{
    public class ErrorLogUtil
    {
        private const string _logFilePath = "ErrorLog.txt";

        public static void LogError(Exception ex, string message = "")
        {
            string newLine = new FileInfo(_logFilePath).Length > 0 ? "\n" : string.Empty;

            File.AppendAllText(_logFilePath, newLine + ex.ToString() + "\n");
            File.AppendAllText(_logFilePath, "Additional message: " + message + "\n----------------------------------------------\n");
        }


    }
}
