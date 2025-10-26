using System.Net;
using GamePlanBackend.Application.Common.ResponseType;

namespace GamePlanBackend.Application.Common.MiddleWare
{
    public static class ResponseData
    {
        public static ResponseModel GetSuccessResponse(dynamic? model, string message = "")
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.OK,
                IsError = false,
                Success = true,
                DataModel = model,
                ResponseMessage = string.IsNullOrEmpty(message) ? "Success" : message
            };
        }

        public static ResponseModel ErrorResponse(string message, dynamic? model = null)
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                ResponseMessage = message,
                DataModel = model,
                IsError = true,
                Success = false
            };
        }

        public static ResponseModel SaveResponse(dynamic? model = null, string message = "Data has been Saved Successfully")
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.OK,
                ResponseMessage = message,
                DataModel = model,
                IsError = false,
                Success = true
            };
        }

        public static ResponseModel DeleteSuccessResponse(string message = "Data has been Deleted Successfully")
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.OK,
                ResponseMessage = message,
                IsError = false,
                Success = true
            };
        }

        public static ResponseModel GetSuccessResponseStream(MemoryStream? model, string message = "")
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.OK,
                IsError = false,
                Success = true,
                DataModel = model,
                ResponseMessage = string.IsNullOrEmpty(message) ? "Success" : message
            };
        }

        public static ResponseModel NotSuccessResponse(string message)
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                ResponseMessage = message,
                DataModel = null,
                IsError = true,
                Success = false
            };
        }

        public static ResponseModel FoundSuccessResponse(dynamic model)
        {
            return new ResponseModel
            {
                statusCode = (int)HttpStatusCode.OK,
                ResponseMessage = "Success",
                DataModel = model,
                IsError = false,
                Success = true
            };
        }
    }
}
