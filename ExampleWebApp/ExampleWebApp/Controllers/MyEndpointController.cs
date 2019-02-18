using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyEndpointController : ControllerBase
    {
        private readonly ILogger _logger;

        public MyEndpointController(ILogger<MyEndpointController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("info")]
        public void LogInfo(string msg)
        {
            _logger.Log(LogLevel.Information, msg);
        }

        [HttpPost]
        [Route("warning")]
        public void LogWarning(string msg)
        {
            _logger.Log(LogLevel.Warning, msg);
        }

        [HttpPost]
        [Route("error")]
        public void LogError(string msg)
        {
            _logger.Log(LogLevel.Error, msg);
        }

        private void IWillFail()
        {
            throw new SystemException("FAIL!!!!!");
        }

        [HttpPost]
        [Route("trace")]
        public void LogTrace(string msg)
        {
            try
            {
                IWillFail();
            }
            catch (SystemException ex)
            {
                _logger.Log(LogLevel.Trace, ex, msg);
            }
        }
    }
}
