using System.Text;
using System.Text.Json;

public class GitHubService
{
    private readonly HttpClient _http;

    private const string OWNER = "Badym";
    private const string REPO = "DevEstate";
    private const string TOKEN = "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"; // ⚠️ potem do configa

    public GitHubService(HttpClient http)
    {
        _http = http;
    }

    public async Task UploadFileAsync(string localPath, string repoPath)
    {
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
        request.Headers.Add("Authorization", $"Bearer {TOKEN}");
        request.Headers.Add("User-Agent", "DevEstate");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.SendAsync(request);
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