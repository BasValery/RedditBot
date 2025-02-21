using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class GPTProcessor : IImageProcessor
{
   private readonly string apiKey;

    public GPTProcessor(IConfiguration configuration)
    {
        apiKey = configuration["OpenAI:ApiKey"];
    }

    public async Task<string> ProcessImageAsync(string imagePath)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            
            var requestBody = new
            {
                model = "gpt-4-vision-preview",
                messages = new[]
                {
                    new { role = "system", content = "Ты – комик, который придумывает смешные подписи к изображениям." },
                    new { role = "user", content = "Придумай смешной заголовок для этого изображения:" }
                },
                max_tokens = 50
            };

            string json = JsonSerializer.Serialize(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            
            string responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
            return jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}