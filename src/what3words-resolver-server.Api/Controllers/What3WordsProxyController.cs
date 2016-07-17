using Flurl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using what3words_resolver_server.Api.w3w.Enums;

namespace what3words_resolver_server.Api.Controllers
{
    [Route("api/w3w/v2")]
    public class What3WordsProxyController : Controller
    {
        //ToDo: base class for _w3wHttpClient when introduce other proxys

        private static string W3WAPIURLBASE = "https://api.what3words.com";

        private HttpClient _w3wHttpClient;
        private readonly string _w3wApiKey;

        public What3WordsProxyController(IConfiguration config)
        {
            _w3wHttpClient = new HttpClient();
            _w3wApiKey = config.GetSection("What3Words")["w3wApiKey"];
        }

        // example w3w api call: https://api.what3words.com/v2/forward?addr=index.home.raft&display=full&format=json&key=[API-KEY]
        [HttpGet]
        [Route("forward")]
        public async Task<IActionResult> Forward(
            [FromHeader(Name = "X-CallingAppId")]string callingAppId,
            [FromHeader(Name = "X-CallingAppName")]string callingAppName,
            [FromHeader(Name = "X-CallingAppVersion")]string callingAppVersion,
            [FromHeader(Name = "X-CallingAppKey")]string callingAppKey,
            [FromQuery]string addr,
            [FromQuery]string key = null,
            [FromQuery]string lang = null,
            [FromQuery]Display? display = null,
            [FromQuery]Format? format = null)
        {
            var url = new Url(W3WAPIURLBASE)
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
            
            var result = Content(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false),
                responseMessage.Content.Headers.ContentType.MediaType);
            
            return result;
        }

        [HttpGet]
        [Route("reverse")]
        public async Task<IActionResult> Reverse(
            [FromHeader(Name = "X-CallingAppId")]string callingAppId,
            [FromHeader(Name = "X-CallingAppName")]string callingAppName,
            [FromHeader(Name = "X-CallingAppVersion")]string callingAppVersion,
            [FromHeader(Name = "X-CallingAppKey")]string callingAppKey,
            [FromQuery]string coords,
            [FromQuery]string key = null,
            [FromQuery]string lang = null,
            [FromQuery]Display? display = null,
            [FromQuery]Format? format = null)
        {
            var url = new Url(W3WAPIURLBASE)
                .AppendPathSegment("v2/reverse")
                .SetQueryParams(new
                {
                    coords,
                    key = key ?? _w3wApiKey,
                    lang,
                    display = display?.ToString().ToLower(),
                    format = format?.ToString().ToLower()
                });

            var responseMessage = await _w3wHttpClient.GetAsync(url).ConfigureAwait(false);

            var result = Content(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false),
                responseMessage.Content.Headers.ContentType.MediaType);

            return result;
        }
    }
}
            
