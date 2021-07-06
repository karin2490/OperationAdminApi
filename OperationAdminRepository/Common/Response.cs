
using OperationAdminRepository.Common.Enum;
using OperationAdminRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminRepository.Common
{
    public class Response : ResponceBase, IResponcedoObject
    {

        public object Data { get; set; }
        public Response() { }
        public Response(object Data, bool IsValid, string Message, ResponseType Type)
        {
            this.Data = Data;
            this.IsValid = IsValid;
            this.Message = Message;
            this.Type = Type;
        }

        public static Response ToResponse(bool v1, object nO_FOUND, string v2)
        {
            throw new NotImplementedException();
        }

        /*public Task ExecuteResultAsync(ActionContext context)
        {
            ExecuteResult(context);
            return Task.;            
        }
        public virtual void ExecuteResult(ActionContext context) { }*/
    }

    public class ResponceBase : IResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public ResponseType Type { get; set; }
        public bool HasMessages
        {
            get
            {
                return (this.Message.Length > 0);
            }
        }
        public ResponceBase()
        {
            this.IsValid = false;
            this.Type = ResponseType.INTERNAL_ERROR;
            this.Message = string.Empty;
        }
    }
}
