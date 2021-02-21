using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTracing;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITracer tracer;
        private readonly IHttpClientFactory httpClientFactory;

        public ValuesController(
            ITracer tracer,
            IHttpClientFactory httpClientFactory
            )
        {
            this.tracer = tracer;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var client = httpClientFactory.CreateClient("payment.api");

            using (tracer.BuildSpan("WaitingForValues").StartActive(finishSpanOnDispose: true))
            {
                return JsonConvert.DeserializeObject<List<string>>(
                    await client.GetStringAsync("values")
                );
            }
        }
    }
}