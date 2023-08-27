using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System;
using Storesys.web;
using Storesys.exception;

namespace Storesys.utils
{
    /**
     * @author sty
     * @brief exception middleware for global exception handling
     *        http return code will be modified according to the exception catched
     * 
     * @param exceptions catched
     * @return error code and messages
     * @throws nothrow exception specification
     */
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate requestDelegate, ILogger<GlobalExceptionHandler> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = ComResponse<int>.error();

            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.msg = ex.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.msg = ex.Message;
                    break;
                case DuplicateException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse.code = (int)HttpStatusCode.Conflict;
                    errorResponse.msg = ex.Message;
                    break;
                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.code = (int)HttpStatusCode.NotFound;
                    errorResponse.msg = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.msg = "Internal Server errors. Check Logs!";
                    break;
            }

            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

    }
}
