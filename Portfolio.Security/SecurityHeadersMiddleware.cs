using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Portfolio.Security
{
    /// <summary>
    /// Security Headers Middleware
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersPolicy _policy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityHeadersMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="policy">The policy.</param>
        public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersPolicy policy)
        {
            _next = next;
            _policy = policy;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            IHeaderDictionary headers = context.Response.Headers;

            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }

            await _next(context);
        }
    }
}
