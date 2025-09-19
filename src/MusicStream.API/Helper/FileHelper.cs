namespace Music.API.Helper;

public static class FileHelper
{
    public static (string filePath, string storedName)
    PrepareFileForSaving(string fileName, string WebRootPath)
    {
        var ext = Path.GetExtension(fileName);
        var storedname = Guid.NewGuid().ToString();
        var storedNameWithExt = $"{storedname}{ext}";
        var filePath = Path.Combine(WebRootPath, storedNameWithExt);

        return (filePath, storedname);
    }

}
