using System;

namespace MusicStream.Application.Interfaces;

public interface ISeeder
{
    Task MigrateAsync();
}
