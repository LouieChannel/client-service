using Ascalon.ClientService.Features.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Middlewares
{
    public class ForbiddenMiddleware
    {
        private readonly RequestDelegate _next;

        public ForbiddenMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ForbiddenException e)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ForbiddenResponse { Message = e.Message }));
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions.Count != 1 || !(ae.InnerExceptions[0] is ForbiddenException))
                {
                    throw;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ForbiddenResponse { Message = ae.InnerExceptions[0].Message }));
            }
        }
    }
}
