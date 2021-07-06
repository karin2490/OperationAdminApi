using OperationAdminRepository.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminRepository.Interface
{
    public interface IResponse
    {
        bool IsValid { get; set; }
        string Message { get; set; }
        ResponseType Type { get; set; }
        bool HasMessages { get; }
    }

    public interface IResponcedoObject : IResponse
    {
        object Data { get; set; }
    }
    public interface IResponceGeneric<T> : IResponse
    {
        T Data { get; set; }
    }

    public interface IResponceReporte : IResponse
    {
        int ResultadoReporteID { get; set; }
    }
}
