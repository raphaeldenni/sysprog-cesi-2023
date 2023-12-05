namespace EasySave.Models;

public class TaskEntity
{
    // Properties
    
    //// Task
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? SourcePath { get; set; }
    public string? DestPath { get; set; }
    public string? Type { get; set; }
    public string? Timestamp { get; set; }
    
    //// State
    public string? State { get; set; }
    public int? FilesNumber { get; set; }
    public float? FilesSize { get; set; }
    public int? LeftFilesNumber { get; set; }
    public float? LeftFilesSize { get; set; }
    public string? FileSourcePath { get; set; }
    public string? FileDestPath { get; set; }
}
