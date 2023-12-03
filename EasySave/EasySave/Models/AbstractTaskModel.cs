using System.Text.Json;

namespace EasySave.Models;

public abstract class AbstractTaskModel
{
    protected int TaskId { get; set; }
    protected internal string TaskName { get; set; }
    protected static string StateFileName => "state.json";
    public List<AbstractTaskModel> TasksList { get; private set; }
    
    protected void CreateStateFile()
    {
        TasksList = new List<AbstractTaskModel>();
        var jsonState = JsonSerializer.Serialize(TasksList);
        File.WriteAllText(StateFileName, jsonState);
    }
    
    protected void PullStateFile()
    {
        var jsonState = File.ReadAllText(StateFileName);
        TasksList = JsonSerializer.Deserialize<List<AbstractTaskModel>>(jsonState);
    }
    
    protected void UpdateStateFile(List<TaskModel> tasks)
    {
        var jsonState = JsonSerializer.Serialize(tasks);
        File.WriteAllText(StateFileName, jsonState);
    }
}