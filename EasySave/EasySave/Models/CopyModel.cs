using EasySave.Types;

namespace EasySave.Models;

public class CopyModel
{
    // Properties
    public string SourcePath { get; set; }
    public string DestPath { get; set; }
    public BackupType Type { get; set; }
    public Dictionary<string, List<string>> DirectoryStructure { get; set; }

    // Constructors
    public CopyModel(string sourcePath, string destPath, BackupType type)
    {
        SourcePath = sourcePath;
        DestPath = destPath;
        DirectoryStructure = new Dictionary<string, List<string>>();
        Type = type;
        CheckFiles();
    }

    // Methods
    public void CheckFiles()
    {
        if (!Directory.Exists(SourcePath))
        {
            throw new DirectoryNotFoundException("Source directory does not exist");
        }

        if (!Directory.Exists(DestPath))
        {
            throw new DirectoryNotFoundException("Destination directory does not exist");
        }

        BuildDirectoryStructure(SourcePath, SourcePath);
    }

    private void BuildDirectoryStructure(string currentDirectory, string rootDirectory)
    {
        string relativePath = currentDirectory.Substring(rootDirectory.Length).TrimStart('\\');

        if (!DirectoryStructure.ContainsKey(relativePath))
        {
            DirectoryStructure[relativePath] = new List<string>();
        }

        foreach (string file in Directory.GetFiles(currentDirectory))
        {
            DirectoryStructure[relativePath].Add(Path.GetFileName(file));
        }

        foreach (string subdirectory in Directory.GetDirectories(currentDirectory))
        {
            BuildDirectoryStructure(subdirectory, rootDirectory);
        }
    }

    public void CopyFiles()
    {
        foreach (var directoryEntry in DirectoryStructure)
        {
            string sourceDirectory = Path.Combine(SourcePath, directoryEntry.Key);
            string destDirectory = Path.Combine(DestPath, directoryEntry.Key);

            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            foreach (string fileName in directoryEntry.Value)
            {
                string sourceFilePath = Path.Combine(sourceDirectory, fileName);
                string destFilePath = Path.Combine(destDirectory, fileName);

                if (Type == BackupType.Complete)
                {
                    File.Copy(sourceFilePath, destFilePath, true);
                }
                else if (Type == BackupType.Differential)
                {
                    if (File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                    {
                        File.Copy(sourceFilePath, destFilePath, true);
                    }
                }
            }
        }
    }
}