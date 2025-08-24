using MusicStream.Domain.Common;

namespace MusicStream.Infrastructure.FileManagement;

public class FileManager
{


    public async Task<Response<IEnumerable<string>>> GetFilesFromDirectory(string dirpath)
    {
        int maxRetry = 10;

        for (var i = 0; i < maxRetry; i++)
        {
            if (CheckIfDirectoryReady(dirpath))
            {
                return Response<IEnumerable<string>>.Succeed(Directory.EnumerateFiles(dirpath));
            }
            await Task.Delay(200);
        }
        return Response<IEnumerable<string>>.Failed("Directory is locked or does not exist");
    }
    public void DeleteSingleFile(string filePath) => File.Delete(filePath);
    public void DeleteSingleDirectory(string dirpath) => Directory.Delete(dirpath, true);


    public async Task<bool> IsFileReady(string filePath, int maxRetry = 10)
    {
        for (var i = 0; i < maxRetry; i++)
        {
            if (CheckIfFileReady(filePath))
                return true;
            await Task.Delay(200);

        }
        return false;
    }

    private bool CheckIfFileReady(string filePath)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            return true;
        }
        catch (IOException)
        {
            Console.WriteLine("File IOExecption");
            return false;
        }
    }

    private bool CheckIfDirectoryReady(string dirpath)
    {
        try
        {
            var entries = Directory.EnumerateFiles(dirpath).FirstOrDefault();
            return true;

        }
        catch (IOException)
        {
            Console.WriteLine("Directory IOExecption");
            return false;
        }

    }
}
