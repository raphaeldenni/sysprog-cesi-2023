using System.Diagnostics;
using System.Runtime.InteropServices;
using EasySave.Types;

namespace EasySave.Models;

public class CopyModel
{
    public event Action<TaskEntity, string[]>? FileCopied;

    // Properties
    private const string EasySaveFolderName = "EasySave";
    public TaskEntity Task { get; set; }

    //// CryptoSoft properties
    private Process CryptoSoftProcess { get; }
    private string Key { get; }
    private string[] ExtensionsToEncrypt { get; }
    private string TempDestDirectory { get; }
    public List<string> PriorityFiles { get; set; } = new List<string>();
    public List<string> PriorityFilesExtensions { get; set; }

    //// Others
    private Dictionary<string, List<string>> DirectoryStructure { get; }

    // Constructors

    /// <summary>
    /// CopyModel constructor
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destPath"></param>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <param name="extensionsToEncrypt"></param>
    public CopyModel(TaskEntity task, string key, string[] extensionsToEncrypt, string[] priorityFilesExtensions)
    {
        PriorityFilesExtensions = new List<string>(priorityFilesExtensions);

        // Task properties
        Task = task;
        Task.FilesNumber = 0;
        Task.FilesSize = 0;
        // CryptoSoft properties
        Key = key;
        ExtensionsToEncrypt = extensionsToEncrypt;
        
        // CryptoSoft process is used to encrypt the files.
        CryptoSoftProcess = new Process();
        
        // Here we set the path to the executable depending on the OS.
        CryptoSoftProcess.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? @".\CryptoSoft.exe"
            : "./CryptoSoft";
        
        // Hide the console
        CryptoSoftProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        CryptoSoftProcess.StartInfo.CreateNoWindow = true;
        
        // Define the temp directory
        var tempPath = Path.GetTempPath();
        TempDestDirectory = Path.Combine(tempPath, EasySaveFolderName);
        
        DirectoryStructure = new Dictionary<string, List<string>>();
        
        CheckFiles();

        Task.LeftFilesNumber = Task.FilesNumber;
        Task.LeftFilesSize = Task.FilesSize;
    }

    // Methods
    
    /// <summary>
    /// Check if the source and destination paths exist
    /// </summary>
    /// <exception cref="DirectoryNotFoundException"></exception>
    private void CheckFiles()
    {
        if (!Directory.Exists(Task.SourcePath) || !Directory.Exists(Task.DestPath))
            throw new DirectoryNotFoundException();

        BuildDirectoryStructure(Task.SourcePath, Task.SourcePath);
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
        var destFilePath = Path.Combine(Task.DestPath, relativePath, fileName);

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
            if (Task.Type != BackupType.Complete && !IsFileModified(file, relativePath)) continue;

            var fileName = Path.GetFileName(file);

            if (PriorityFilesExtensions.Contains(Path.GetExtension(file)))
            {
                PriorityFiles.Add(Path.Combine(relativePath, fileName));
                Task.LeftNumberPriorityFiles++;
            }
            else
            {
                DirectoryStructure[relativePath].Add(fileName);
            }

            Task.FilesNumber++;
            Task.FilesSize += new FileInfo(file).Length;
        }

        foreach (var subdirectory in Directory.GetDirectories(currentDirectory))
        {
            BuildDirectoryStructure(subdirectory, rootDirectory);
        }
    }
    
    public void CreateDirectoryStructure()
    {
        foreach (var directory in DirectoryStructure)
        {
            var destDirectory = Path.Combine(Task.DestPath, directory.Key);
            
            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);
        }
    }

    public void CopyPriorityFiles()
    {
        foreach (var priorityFile in PriorityFiles)
        {
            var sourceFilePath = Path.Combine(Task.SourcePath, priorityFile);
            var destFilePath = Path.Combine(Task.DestPath, priorityFile);

            Copy(sourceFilePath, destFilePath, priorityFile);
            Task.LeftNumberPriorityFiles--;
        }
    }

    public void CopyNonPriorityFiles() 
    {
        foreach (var directory in DirectoryStructure)
        {
            var sourceDirectory = Path.Combine(Task.SourcePath, directory.Key);
            var destDirectory = Path.Combine(Task.DestPath, directory.Key);

            foreach (var file in directory.Value)
            {
                var sourceFilePath = Path.Combine(sourceDirectory, file);
                var destFilePath = Path.Combine(destDirectory, file);

                Copy(sourceFilePath, destFilePath, file);
                Task.LeftFilesNumber--;
            }
        }
    }

    public void Copy(string sourceFilePath, string destFilePath, string file)
    {
        // Use a stop watch to get the time it takes to copy a file
        var stopwatch = new Stopwatch();

        stopwatch.Start();

        // If the file extension is in the list of extensions to encrypt, encrypt it
        if (ExtensionsToEncrypt.Contains(Path.GetExtension(file)))
        {
            Directory.CreateDirectory(TempDestDirectory);

            var tempDestFilePath = Path.Combine(TempDestDirectory, Path.GetFileName(file));

            CryptoSoftProcess.StartInfo.Arguments = $"\"{sourceFilePath}\" \"{tempDestFilePath}\" \"{Key}\"";

            CryptoSoftProcess.Start();
            CryptoSoftProcess.WaitForExit();

            File.Move(tempDestFilePath, destFilePath, true);
            Directory.Delete(TempDestDirectory, true);
        }
        else
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        stopwatch.Stop();

        var copyTime = stopwatch.ElapsedMilliseconds;

        Task.LeftFilesSize -= new FileInfo(sourceFilePath).Length;

        // Invoke file info data to write it in the log file
        string[] fileInfo =
        {
                sourceFilePath,
                destFilePath,
                new FileInfo(sourceFilePath).Length.ToString(),
                copyTime.ToString()
            };

        FileCopied?.Invoke(Task, fileInfo);
    }

    /// <summary>
    /// Copy the files from the source path to the destination path and encrypt them
    /// </summary>
    public void CopyFiles()
    {
        CreateDirectoryStructure();
        CopyPriorityFiles();
        CopyNonPriorityFiles();
    }
}