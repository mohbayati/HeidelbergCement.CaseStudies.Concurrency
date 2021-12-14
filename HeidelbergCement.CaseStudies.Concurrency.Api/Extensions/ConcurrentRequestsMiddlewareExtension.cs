using HeidelbergCement.CaseStudies.Concurrency.Middleware;

namespace HeidelbergCement.CaseStudies.Concurrency.Extensions
{
    public static class ConcurrentRequestsMiddlewareExtension
    {
        public static IApplicationBuilder UseConcurrentRequestsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConcurrentRequestsMiddleware>();
        }
    }
}
