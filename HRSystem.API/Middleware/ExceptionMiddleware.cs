namespace HRSystem.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            //catch (Exception ex)
            //{
            // context.Response.StatusCode = 500;
            //await context.Response.WriteAsJsonAsync(new
            //{
            //   message = ex.Message
            //});

            //}
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}
