using System.Net;
using VillaAPI.utils;

namespace VillaAPI.Middlewares
{
    public class GlobalErrorHandlingMDW
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment Env;

        public GlobalErrorHandlingMDW(RequestDelegate _next , IWebHostEnvironment _env)
        {
            next = _next;
            Env = _env;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (AppError appError)
            {
                await HandleOperationExeptionAsync(context, appError);
            }
            catch (Exception ex)
            {
                await HandleNoneOperationExeptionAsync(context, ex);
            }
        }
        private Task HandleOperationExeptionAsync(HttpContext context, AppError appError)
        {
            object responseObj = new { };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = appError.statusCode;

            if (Env.EnvironmentName == "Development")
            {
                responseObj = new
                {
                    status = appError.status,
                    message = appError.Message,
                    StackTrace = appError.StackTrace
                };
            }

            if (Env.EnvironmentName == "Production")
            {
                responseObj = new
                {
                    status = appError.status,
                    message = appError.Message,
                };

            }

            return context.Response.WriteAsJsonAsync(responseObj);
        }
        private Task HandleNoneOperationExeptionAsync(HttpContext context, Exception ex)
        {
            object responseObj = new { };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            if (Env.EnvironmentName == "Development")
            {
                responseObj = new
                {
                    status = "Error",
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace,
                };
            }

            if (Env.EnvironmentName == "Production")
            {
                responseObj = new
                {
                    status = "Error",
                    message = "Something went very wrong",
                };
            }

            return context.Response.WriteAsJsonAsync(responseObj);
        }
    }
}
