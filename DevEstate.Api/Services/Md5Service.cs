using System.Security.Cryptography;

namespace DevEstate.Api.Services;

public class Md5Service
{
    public string GenerateHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);

        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public void SaveMd5File(string filePath)
    {
        string md5 = GenerateHash(filePath);
        var dir = Path.GetDirectoryName(filePath)!;
        var nameWithoutExt = Path.GetFileNameWithoutExtension(filePath); // cennik
        var md5Path = Path.Combine(dir, nameWithoutExt + ".md5");        // cennik.md5

        File.WriteAllText(md5Path, md5 + Environment.NewLine);
    }
    
    public async Task<string> GetMd5FromUrlAsync(string url)
    {
        using var http = new HttpClient();
        var bytes = await http.GetByteArrayAsync(url);

        using var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(bytes);

        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
    
    public string GetMd5(string filePath)
    {
        return GenerateHash(filePath);
    }
}