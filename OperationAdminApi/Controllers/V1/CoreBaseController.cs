using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using OperationAdminRepository;
using OperationAdminRepository.Common;
using OperationAdminRepository.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OperationAdminApi.Controllers.V1
{
   
    public class CoreBaseController<T> : ControllerBase where T : class
    {
        private readonly ILogger _logger;
        private Response response => new Response();

        public CoreBaseController(ILoggerFactory loggerFactory) => _logger = loggerFactory.CreateLogger<T>();

        protected IActionResult ActionResultException(Exception ex)
        {
            _logger.LogError(ex.StackTrace);
            var exResponse = ex.ToResponseException();
            return StatusCode((int)exResponse.Type, exResponse);
        }

        protected IActionResult InvalidModelException(ModelStateDictionary model, string errorMessage = "")
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = model.Values.SelectMany(x => x.Errors).Select(z => z.ErrorMessage).FirstOrDefault();
            }

            var modelState = response.ToResponse(false, ResponseType.BAD_REQUEST, errorMessage);

            return StatusCode((int)modelState.Type, modelState);
        }
    }
}
