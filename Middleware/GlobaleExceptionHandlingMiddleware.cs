using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BlogWebApi.Middleware
{
    public class GlobaleExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public GlobaleExceptionHandlingMiddleware(ILogger<GlobaleExceptionHandlingMiddleware> logger) => _logger = logger;
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) 
        {
            //we try our request if the request we pass 
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //logging our error
                _logger.LogError(ex, ex.Message);

                //we return response with 500 error code
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.OK,
                    Type = "OK",
                    Title = "Ok",
                    Detail = "Ok"
                };

                //return a json string to the reponse body
                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);

            }
        }
    }
}
