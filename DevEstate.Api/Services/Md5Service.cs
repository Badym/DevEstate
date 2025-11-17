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
        string fileName = Path.GetFileName(filePath);

        File.WriteAllText(filePath + ".md5", $"{md5}  {fileName}");
    }
}