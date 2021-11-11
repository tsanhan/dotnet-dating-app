using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,/*to forward to the next middleware*/
            ILogger<ExceptionMiddleware> logger,/*we still want to print the exception to the terminal*/
            IHostEnvironment env/*to check if we in dev or in prod*/)
        {
            this._next = next;
            this._logger = logger;
            this._env = env;
        }

        //the reqired mothod to handle the exception
        public async Task InvokeAsync(HttpContext context /*because this is happening in the context of an http request comming in*/)
        {
            // this Is where we will use try catch to handle the exception
            try
            {
                // try to just pass this on to the next middleware
                // this is good: this middleware is not responsible for handling the exception
                // it's on the top of the middleware chain, 
                // so if it throws an exception, the exception go up and up until it reaches the top (here)
                await _next(context);
            }
            catch (Exception ex)
            {
                // first we log the exception... otherwise the log will be silent
                _logger.LogError(ex, ex.Message);

                context.Request.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;//this is effectively be a 500 error

                // now we create the response
                // if we are in dev mode we want to print the exception to the terminal
                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?/*we don't an exception here...*/.ToString())
                    : new ApiException(context.Response.StatusCode, "Internam Server Error");


                // we send this in json so we want to use cammelCase, we'll add some options for this
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                //use the serializer to create the json
                var json = JsonSerializer.Serialize(response, options);

                //write the json to the response
                await context.Response.WriteAsync(json);

                
            }
        }
    }
}