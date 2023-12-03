namespace EasySave.Models;

public interface ITaskModel
{
    int TaskId { get; set; }
    string TaskName { get; set; }
    
    void UpdateStateFile();
}