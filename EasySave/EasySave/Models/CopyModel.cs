using System.Diagnostics;
using System.Runtime.InteropServices;
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
    
    //// Others
    private Dictionary<string, List<string>> DirectoryStructure { get; }
    private Process CryptoSoftProcess { get; }

    // Constructors
    
    /// <summary>
    /// CopyModel constructor
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destPath"></param>
    /// <param name="type"></param>
    public CopyModel(string sourcePath, string destPath, BackupType type)
    {
        // Task properties
        SourcePath = sourcePath;
        DestPath = destPath;
        Type = type;
        LeftFilesNumber = 0;
        LeftFilesSize = 0;
        
        DirectoryStructure = new Dictionary<string, List<string>>();
        
        // CryptoSoft process is used to encrypt the files.
        // Here we set the path to the executable depending on the OS.
        CryptoSoftProcess = new Process();
        CryptoSoftProcess.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? ".\\CryptoSoft.exe" 
            : "./CryptoSoft";
        
        CheckFiles();
    }

    // Methods
    
    /// <summary>
    /// Check if the source and destination paths exist
    /// </summary>
    /// <exception cref="DirectoryNotFoundException"></exception>
    private void CheckFiles()
    {
        if (!Directory.Exists(SourcePath) || !Directory.Exists(DestPath))
            throw new DirectoryNotFoundException();

        BuildDirectoryStructure(SourcePath, SourcePath);
    }
    
    /// <summary>
    /// Check if the file has been modified since the last backup
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    private bool IsFileModified(string filePath, string relativePath)
    {
        var fileName = Path.GetFileName(filePath);
        var destFilePath = Path.Combine(DestPath, relativePath, fileName);

        return !File.Exists(destFilePath) || File.GetLastWriteTime(filePath) > File.GetLastWriteTime(destFilePath);
    }
    
    /// <summary>
    /// Build the directory structure of the source path
    /// </summary>
    /// <param name="currentDirectory"></param>
    /// <param name="rootDirectory"></param>
    private void BuildDirectoryStructure(string currentDirectory, string rootDirectory)
    {
        var relativePath = currentDirectory.Substring(rootDirectory.Length).TrimStart('\\');
        
        // If the current directories structure doesn't exist in DirectoryStructure, create it
        if (!DirectoryStructure.ContainsKey(relativePath))
            DirectoryStructure[relativePath] = new List<string>();
        
        // In differential save, keep in memory the last used file
        foreach (var file in Directory.GetFiles(currentDirectory))
        {
            if (Type != BackupType.Complete && !IsFileModified(file, relativePath)) continue;

            var fileName = Path.GetFileName(file);
            DirectoryStructure[relativePath].Add(fileName);
            
            LeftFilesNumber++;
            LeftFilesSize += new FileInfo(file).Length;
        }

        foreach (var subdirectory in Directory.GetDirectories(currentDirectory))
        {
            BuildDirectoryStructure(subdirectory, rootDirectory);
        }
    }
    
    /// <summary>
    /// Copy the files from the source path to the destination path and encrypt them
    /// </summary>
    public void CopyFiles()
    {
        foreach (var directory in DirectoryStructure)
        {
            var sourceDirectory = Path.Combine(SourcePath, directory.Key);
            var destDirectory = Path.Combine(DestPath, directory.Key);
            
            var tempDestDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
                ? ".\\temp\\"
                : "./temp/";

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            foreach (var file in directory.Value)
            {
                var sourceFilePath = Path.Combine(sourceDirectory, file);
                var destFilePath = Path.Combine(destDirectory, file);
                
                var tempDestFilePath = Path.Combine(tempDestDirectory, file);
                
                CryptoSoftProcess.StartInfo.Arguments = $"{sourceFilePath} {tempDestFilePath}";
                
                // Use a stop watch to get the time it takes to copy a file
                var stopwatch = new Stopwatch();
                
                stopwatch.Start();
                
                // Encrypt the file if it's in the list of extensions to encrypt
                CryptoSoftProcess.Start();
                CryptoSoftProcess.WaitForExit();
                
                // Move the encrypted (or not) file to the destination path
                File.Move(tempDestFilePath, destFilePath, true);
                
                stopwatch.Stop();
                
                var copyTime = stopwatch.ElapsedMilliseconds;
                
                Directory.Delete(tempDestDirectory, true);
                
                // Update the left files number and size
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