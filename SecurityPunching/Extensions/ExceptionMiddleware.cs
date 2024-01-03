﻿using System;
using System.Net;
using DataAccess.Contracts;
using DataAccess.Entities;

namespace SecurityPunching.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleException(httpContext, ex);

            }
        }
        private async Task HandleException(HttpContext context, Exception exception)
        {
            String[] strlist = Convert.ToString(exception.Message).Split(":", StringSplitOptions.RemoveEmptyEntries);

            context.Response.ContentType = "application/json";
            if (strlist[0].Contains("IDX"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Unauthorized"
                }.ToString());
            }
            else if (strlist[0].Contains("ORA"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Database Error"
                }.ToString());
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Internal Server Errorr"
                }.ToString());
            }

        }
    }
}
