using AspNetCore.AsyncRequests.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Sodium;
using System.Threading.Tasks;

namespace AspNetCore.AsyncRequests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AsyncController : ControllerBase
    {
        [HttpGet("{seconds:int}/{id?}")]
        public async Task<Status> Get(int seconds, int? id)
        {
            var obj = new CodeHolder(seconds.ToTimeSpanSeconds());
            return await obj.AsyncData(id.GetValueOrDefault(0));
        }

        [HttpGet("{quantity:int}/cpu-bound")]
        public async Task<Status> GetCpu(int quantity)
        {
            await Task.Run(() => CpuIntensive(quantity));

            return new Status(quantity);
        }

        private void CpuIntensive(int quantity)
        {
            var faker = new Faker();
            for (int i = 0; i < quantity; i++)
            {
                PasswordHash.ArgonHashString(faker.Internet.Password(), PasswordHash.StrengthArgon.Sensitive);
            }
        }
    }
}