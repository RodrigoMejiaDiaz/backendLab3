using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class GifController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public GifController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://api.tenor.com/v1/");
    }

    [HttpGet("{searchQuery}")]
    public async Task<IActionResult> GetGifs(string searchQuery)
    {
        try
        {
            var apiKey = "LIVDSRZULELA";
            var response = await _httpClient.GetAsync($"search?key={apiKey}&q={searchQuery}&limit=5");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var gifs = json["results"]
                .Select(gif => new
                {
                    Id = gif["id"].ToString(),
                    WebmPreviewUrl = gif["media"][0]["gif"]["url"].ToString()
                });

            return Ok(gifs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

