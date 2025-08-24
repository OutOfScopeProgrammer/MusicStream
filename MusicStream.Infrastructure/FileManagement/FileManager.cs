namespace MusicStream.Infrastructure.FileManagement;

public class FileManager
{


    public async Task<bool> IsFileReady(string filePath, int maxRetry = 10)
    {
        for (var i = 0; i < maxRetry; i++)
        {
            if (CheckIfReady(filePath))
                return true;
            await Task.Delay(200);

        }
        return false;
    }

    private bool CheckIfReady(string filePath)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }
}
