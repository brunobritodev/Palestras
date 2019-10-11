using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace AspNetCore.AsyncRequests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableAsyncIoThreads);

            return Ok(new { AvailableAsyncIOThreads = availableAsyncIoThreads, AvailableWorkerThreads = availableWorkerThreads });
        }

        [HttpGet("threads")]
        public IActionResult GetTheads()
        {
            ThreadPool.GetMaxThreads(out var availableWorkerThreads, out var availableAsyncIoThreads);

            return Ok(new { MaxAsyncIOThreads = availableAsyncIoThreads, MaxWorkerThreads = availableWorkerThreads });
        }
    }
}