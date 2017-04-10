using Microsoft.AspNetCore.Builder;

namespace Portfolio.Security.Core
{
    /// <summary>
    /// Middleware Extensions
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Uses the security headers middleware.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app, SecurityHeadersBuilder builder)
        {
            SecurityHeadersPolicy policy = builder.Build();
            return app.UseMiddleware<SecurityHeadersMiddleware>(policy);
        }
    }
}
