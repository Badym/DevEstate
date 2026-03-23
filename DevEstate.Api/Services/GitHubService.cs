using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevEstate.Api.Models;
using DevEstate.Api.Services;
using Microsoft.Extensions.Options;


public class GitHubService
{
    private readonly HttpClient _httpClient;
    private readonly GitHubSettings _settings;
    
    private readonly ILogger<XmlFeedScheduler> _logger;

    private const string OWNER = "Badym";
    private const string REPO = "DevEstate";
    
    private readonly string _token;
    
    public GitHubService(HttpClient httpClient, IOptions<GitHubSettings> settings, ILogger<XmlFeedScheduler> logger)
    {
        _httpClient = httpClient;
        _token = settings.Value.Token;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("DevEstateApp");
    }

    public async Task UploadFileAsync(string localPath, string repoPath)
    {
        //Console.WriteLine("TOKEN: " + _token.Substring(0, 5));
        var content = await File.ReadAllBytesAsync(localPath);
        var base64 = Convert.ToBase64String(content);

        var url = $"https://api.github.com/repos/{OWNER}/{REPO}/contents/{repoPath}";

        var body = new
        {
            message = $"add {repoPath}",
            content = base64
        };

        var json = JsonSerializer.Serialize(body);

        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("Authorization", $"Bearer {_token}");
        request.Headers.Add("User-Agent", "DevEstate");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"GitHub upload failed: {responseText}");
        }
    }

    public string GetRawUrl(string repoPath)
    {
        return $"https://raw.githubusercontent.com/{OWNER}/{REPO}/main/{repoPath}";
    }
}