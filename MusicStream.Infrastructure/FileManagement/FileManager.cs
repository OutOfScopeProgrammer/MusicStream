using System.Threading.Tasks;
using MusicStream.Domain.Common;

namespace MusicStream.Infrastructure.FileManagement;

public class FileManager
{
    private const int MAXRETRY = 10;
    private const int DELAYMS = 500;


    public async Task<Response<IEnumerable<string>>> GetFilesFromDirectory(string dirpath)
    {


        for (var i = 0; i < MAXRETRY; i++)
        {
            if (await CheckIfDirectoryReady(dirpath))
            {
                var files = await Task.Run(() => Directory.EnumerateFiles(dirpath));
                return Response<IEnumerable<string>>.Succeed(files);
            }
            await Task.Delay(DELAYMS);
        }
        return Response<IEnumerable<string>>.Failed("Directory is locked or does not exist");
    }
    public void DeleteSingleFile(string filePath) => File.Delete(filePath);
    public void DeleteSingleDirectory(string dirpath) => Directory.Delete(dirpath, true);


    public async Task<bool> IsFileReady(string filePath)
    {
        for (var i = 0; i < MAXRETRY; i++)
        {
            if (await CheckIfFileReady(filePath))
                return true;
            await Task.Delay(DELAYMS);

        }
        return false;
    }

    private async Task<bool> CheckIfFileReady(string filePath)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            var buffer = new byte[1];
            _ = await stream.ReadAsync(buffer.AsMemory(0, 1));
            return true;
        }
        catch (IOException)
        {
            Console.WriteLine("File IOExecption");
            return false;
        }
    }

    private async Task<bool> CheckIfDirectoryReady(string dirpath)
    {
        try
        {
            var entries = await Task.Run(() => Directory.EnumerateFiles(dirpath).FirstOrDefault());
            return true;

        }
        catch (IOException)
        {
            Console.WriteLine("Directory IOExecption");
            return false;
        }

    }
}
