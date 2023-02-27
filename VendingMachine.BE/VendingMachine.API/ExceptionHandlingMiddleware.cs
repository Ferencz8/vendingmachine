using Newtonsoft.Json;
using System.Net;
using VendingMachine.BLL.Exceptions;

namespace VendingMachine.API
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var errorResponse = new ErrorResponse() { Success = false };
                context.Response.ContentType = "application/json";
                var response = context.Response;


                switch (exception)
                {
                    case UnauthorizedOperationException unauthorizedOperationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Message = unauthorizedOperationException.Message;
                        break;

                    case InsuficientFundsException insuficientFundsException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Message = insuficientFundsException.Message;
                        break;

                    case ProductNotFoundException productNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Message = productNotFoundException.Message;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = "Internal server error!";
                        break;
                }

                    await response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}