using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using what3words_resolver_server.Api.w3w.DTOs;

namespace what3words_resolver_server.Api.Controllers
{
    [Route("api/w3w/v2/[controller]")]
    public class ForwardController : Controller
    {
        //ToDo: base class for _w3wHttpClient when introduce other proxys

        private HttpClient _w3wHttpClient;
        private readonly string _w3wApiKey;

        public ForwardController(IConfiguration config)
        {
            _w3wHttpClient = new HttpClient();
            _w3wHttpClient.BaseAddress = new Uri("https://api.what3words.com/");

            _w3wApiKey = config.GetSection("What3Words")["w3wApiKey"];
        }

        // example w3w api call: https://api.what3words.com/v2/forward?addr=index.home.raft&display=full&format=json&key=[API-KEY]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery]string addr,
            [FromQuery]string key = null,
            [FromQuery]string lang = null,
            [FromQuery]Display? display = null,
            [FromQuery]Format? format = null)
        {
            var url = new Url("https://api.what3words.com")
                .AppendPathSegment("v2/forward")
                .SetQueryParams(new
                {
                    addr,
                    key = key ?? _w3wApiKey,
                    lang,
                    display = display?.ToString().ToLower(),
                    format = format?.ToString().ToLower()
                });

            var responseMessage = await _w3wHttpClient.GetAsync(url).ConfigureAwait(false);

            return Ok(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
            
