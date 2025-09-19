using System.Collections.Concurrent;

namespace MusicStream.Infrastructure.FileManagement;

public class FileWatcherBuilder
{
    private FileSystemWatcher? _fileWatcher;

    public FileWatcherBuilder FolderToWatch(string folder)
    {

        _fileWatcher = new FileSystemWatcher(folder);

        return this;
    }

    public FileWatcherBuilder Filter(string filter)
    {
        if (_fileWatcher is null)
            throw new InvalidOperationException("Call FolderToWatch first before setting the filter");
        _fileWatcher.Filter = filter;
        return this;
    }
    public FileWatcher Build()
    {
        if (_fileWatcher is null)
            throw new InvalidOperationException("Call FolderToWatch first, before  Build");
        return new FileWatcher(_fileWatcher);

    }

}

public class FileWatcher
{
    public FileSystemWatcher _fileWatcher { get; set; }
    public SemaphoreSlim _semaphore = new(0);
    public ConcurrentQueue<string> _createdFiles;
    public FileWatcher(FileSystemWatcher watcher)
    {
        _createdFiles = new();
        _fileWatcher = watcher;
        _fileWatcher.Created += (s, e) => OnCreate(e);
        // _fileWatcher.Deleted += (s, e) => OnDelete(e);
        // _fileWatcher.Changed += (s, e) => OnChanged(e);
    }

    public void Start() => _fileWatcher.EnableRaisingEvents = true;
    public void Stop() => _fileWatcher.EnableRaisingEvents = false;

    // public void OnChanged(FileSystemEventArgs @event) { }
    // public void OnDelete(FileSystemEventArgs @event) { }
    public void OnCreate(FileSystemEventArgs @event)
    {
        _createdFiles.Enqueue(@event.FullPath);
        _semaphore.Release();
    }

    public async Task WaitForFileAsnc(CancellationToken cancellationToken)
        => await _semaphore.WaitAsync(cancellationToken);




}
