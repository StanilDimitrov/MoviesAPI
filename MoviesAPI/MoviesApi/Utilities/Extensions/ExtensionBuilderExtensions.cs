using Microsoft.AspNetCore.Builder;

namespace MoviesApi.Utilities.Extensions
{
    public static class ExtensionBuilderExtensions
    {
        public static void UseExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
