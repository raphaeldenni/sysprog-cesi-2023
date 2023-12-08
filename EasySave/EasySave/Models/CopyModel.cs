using System.Diagnostics;
using EasySave.Types;

namespace EasySave.Models;

public class CopyModel
{
    public event Action<string[]>? FileCopied;

    // Properties
    
    //// Task properties
    private string SourcePath { get; }
    private string DestPath { get; }
    private BackupType Type { get; }
    // CHANGE CODE TO MAKE THE FOLLOWING PROPERTIES PRIVATE
    public int LeftFilesNumber { get; private set; }
    public long LeftFilesSize { get; private set; }
    // END OF CHANGE
    
    //// Directory structure
    private Dictionary<string, List<string>> DirectoryStructure { get; }

    // Constructors
    public CopyModel(string sourcePath, string destPath, BackupType type)
    {
        // Task properties
        SourcePath = sourcePath;
        DestPath = destPath;
        Type = type;
        LeftFilesNumber = 0;
        LeftFilesSize = 0;
        
        // Directory structure
        DirectoryStructure = new Dictionary<string, List<string>>();
        
        CheckFiles();
    }

    // Methods
    private void CheckFiles()
    {
        if (!Directory.Exists(SourcePath) || !Directory.Exists(DestPath))
            throw new DirectoryNotFoundException();

        BuildDirectoryStructure(SourcePath, SourcePath);
    }
    
    private bool IsFileModified(string filePath, string relativePath)
    {
        var fileName = Path.GetFileName(filePath);
        var destFilePath = Path.Combine(DestPath, relativePath, fileName);

        return !File.Exists(destFilePath) || File.GetLastWriteTime(filePath) > File.GetLastWriteTime(destFilePath);
    }

    private void BuildDirectoryStructure(string currentDirectory, string rootDirectory)
    {
        var relativePath = currentDirectory.Substring(rootDirectory.Length).TrimStart('\\');
        var currentDirectoryComplete = Directory.GetFiles(currentDirectory);
        
        // If the current directories structure doesn't exist in DirectoryStructure, create it
        if (!DirectoryStructure.ContainsKey(relativePath))
            DirectoryStructure[relativePath] = new List<string>();
        
        // In differential save, keep in memory the last used file
        foreach (var file in currentDirectoryComplete)
        {
            if (Type != BackupType.Complete && !IsFileModified(file, relativePath)) continue;

            var fileName = Path.GetFileName(file);
            DirectoryStructure[relativePath].Add(fileName);
            
            LeftFilesNumber++;
            LeftFilesSize += new FileInfo(file).Length;
        }

        foreach (var subdirectory in currentDirectoryComplete)
        {
            BuildDirectoryStructure(subdirectory, rootDirectory);
        }
    }

    public void CopyFiles()
    {
        foreach (var directory in DirectoryStructure)
        {
            var sourceDirectory = Path.Combine(SourcePath, directory.Key);
            var destDirectory = Path.Combine(DestPath, directory.Key);

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            foreach (var file in directory.Value)
            {
                var sourceFilePath = Path.Combine(sourceDirectory, file);
                var destFilePath = Path.Combine(destDirectory, file);
                
                // Use a stop watch to get the time it takes to copy a file
                var stopwatch = new Stopwatch();
                
                stopwatch.Start();
                
                File.Copy(sourceFilePath, destFilePath, true);
                
                stopwatch.Stop();
                
                var copyTime = stopwatch.ElapsedMilliseconds;
                
                LeftFilesNumber--;
                LeftFilesSize -= new FileInfo(sourceFilePath).Length;
                
                // Invoke file info data to write it in the log file
                string[] fileInfo =
                {
                    sourceFilePath, 
                    destFilePath, 
                    new FileInfo(sourceFilePath).Length.ToString(), 
                    copyTime.ToString()
                };
                
                FileCopied?.Invoke(fileInfo);
            }
        }
    }
}