using System.Diagnostics;
using EasySave.Types;

namespace EasySave.Models;

public class CopyModel
{
    public event Action<string[]> FileCopied;

    // Properties
    public string SourcePath { get; set; }
    public string DestPath { get; set; }
    public BackupType Type { get; set; }
    public int LeftFilesNumber { get; set; }
    public long LeftFilesSize { get; set; }
    public Dictionary<string, List<string>> DirectoryStructure { get; set; }

    // Constructors
    public CopyModel(string sourcePath, string destPath, BackupType type)
    {
        SourcePath = sourcePath;
        DestPath = destPath;
        DirectoryStructure = new Dictionary<string, List<string>>();
        Type = type;
        LeftFilesNumber = 0;
        LeftFilesSize = 0;
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
            if (Type == BackupType.Complete || IsFileModified(file, relativePath))
            {
                DirectoryStructure[relativePath].Add(Path.GetFileName(file));
                LeftFilesNumber++;
                LeftFilesSize += new FileInfo(file).Length;
            }
        }

        foreach (string subdirectory in Directory.GetDirectories(currentDirectory))
        {
            BuildDirectoryStructure(subdirectory, rootDirectory);
        }
    }

    private bool IsFileModified(string filePath, string relativePath)
    {
        string fileName = Path.GetFileName(filePath);
        string destFilePath = Path.Combine(DestPath, relativePath, fileName);

        return !File.Exists(destFilePath) || File.GetLastWriteTime(filePath) > File.GetLastWriteTime(destFilePath);
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
                
                var stopwatch = new Stopwatch();
                File.Copy(sourceFilePath, destFilePath, true);
                stopwatch.Stop();
                
                var copyTime = stopwatch.ElapsedMilliseconds;
                
                LeftFilesNumber--;
                LeftFilesSize -= new FileInfo(sourceFilePath).Length;

                string[] fileInfo = { sourceFilePath, destFilePath, new FileInfo(sourceFilePath).Length.ToString(), copyTime.ToString() };
                FileCopied?.Invoke(fileInfo);
            }
        }
    }
}