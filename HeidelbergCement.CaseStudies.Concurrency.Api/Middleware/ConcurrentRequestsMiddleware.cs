using Microsoft.AspNetCore.Http.Features;

namespace HeidelbergCement.CaseStudies.Concurrency.Middleware
{
    public class ConcurrentRequestsMiddleware
    {
        private int _concurrentRequests;

        private readonly RequestDelegate _next;

        public ConcurrentRequestsMiddleware(RequestDelegate next)
        {
            _concurrentRequests = 0;

            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context)
        {
            if (CheckConcurrent(context))
            {
                IHttpResponseFeature responseFeature = context.Features.Get<IHttpResponseFeature>();

                responseFeature.StatusCode = StatusCodes.Status423Locked;
                responseFeature.ReasonPhrase = "Concurrent request limitation.";

                Interlocked.Decrement(ref _concurrentRequests);
            }
            else
            {
                await _next(context);

                // Decrement concurrent requests count
                if (isPathContain(context))
                    Interlocked.Decrement(ref _concurrentRequests);
            }
        }

        private bool CheckConcurrent(HttpContext context)
        {
            if (isPathContain(context))
                Interlocked.Increment(ref _concurrentRequests);

            return _concurrentRequests > 1 ? true : false;
        }
        private string getPath(HttpContext context)
        {
            return context.Request.Path.HasValue ? context.Request.Path.Value : "";
        }
        private bool isPathContain(HttpContext context)
        {
            return getPath(context).Contains("Items") && (context.Request.Method == "POST" || context.Request.Method == "PUT");
        }
    }
}
