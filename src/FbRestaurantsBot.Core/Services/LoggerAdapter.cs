using FbRestaurantsBot.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FbRestaurantsBot.Core.Services
{
    public class LoggerAdapter : ILoggerAdapter
    {
        private readonly ILogger<LoggerAdapter> _logger;

        public LoggerAdapter(ILogger<LoggerAdapter> logger)
        {
            _logger = logger;
        }
        
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}