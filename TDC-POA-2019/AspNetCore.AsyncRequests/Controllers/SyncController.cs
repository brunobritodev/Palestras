using AspNetCore.AsyncRequests.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.AsyncRequests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncController : ControllerBase
    {
        [HttpGet("{seconds:int}/{id?}")]
        public Status Get(int seconds, int? id)
        {
            var obj = new CodeHolder(seconds.ToTimeSpanSeconds());
            return obj.SyncData(id.GetValueOrDefault(0));
        }
    }
}
