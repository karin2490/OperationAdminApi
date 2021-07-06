using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using OperationAdminRepository.Common.Enum;
using OperationAdminRepository.Common;
using System;
using OperationAdminRepository.Utils;

namespace OperationAdminRepository
{
    public static class Extention
    {
       private static ILogger<Response> Logger = NullLogger<Response>.Instance;
        public static string Serializer(this object source)
        {
            if (source == null)
                source = new object();
            return JsonConvert.SerializeObject(source);
        }

        public static Response ToResponse(this object Source, bool IsValid, ResponseType Type, string Messages = "")
        {
            Response Response = new Response
            {
                IsValid = IsValid,
                Type = Type,
                Data = Source,
                Message = Messages
            };            
            return Response;
        }
        public static Response ToResponse(this object Source, string Messages = "")
        {
            return Source.ToResponse(true, ResponseType.SUCCESS, Messages);
        }

        public static Response ToResponseExeption(this Exception E, bool IsValid, ResponseType Type, string Messages = "There was an error with function")
        {
            Response Response = new Response
            {
                IsValid = IsValid,
                Type = Type,
                Data = string.Format("{0}. {1}", E.Message, E.StackTrace),                
                Message = Messages
            };
            return Response;
        }
        public static Response ToResponseExeption(this Exception E, string Messages = "There was an error with function")
        {
            return E.ToResponseExeption(false, ResponseType.INTERNAL_ERROR, Messages);
        }

        public static Response ToResponseException(this Exception ex)
        {
            if (ex is CustomValidationException)
            {
                return ex.ToResponse(ex.Message);
            }
            else
            {
                Logger.LogError($"An unexpected error has occurred Ex: {ex}");
                throw ex;
            }
        }
    }
}
