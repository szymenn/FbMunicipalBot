using Microsoft.AspNetCore.Builder;

namespace FbMunicipalTransportBot.Extensions
{
    public static class CustomExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler
            (this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}