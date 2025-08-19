namespace Music.API.Helper;

public static class FileHelper
{
    public static (string FullPath, string FileName, string UploadPath)
    PrepareFileForSaving(string fileName, string WebRootPath)
    {
        var ext = Path.GetExtension(fileName);
        var name = Path.GetFileNameWithoutExtension(fileName);
        var uploadPath = Path.Combine(WebRootPath, "Temp");
        var storedName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadPath, storedName);


        return (fullPath, name, uploadPath);
    }

}
