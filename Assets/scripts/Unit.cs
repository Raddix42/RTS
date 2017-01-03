using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    [System.NonSerialized]
    public Task[] availableTasks;

    void Start()
    {
        availableTasks = GetComponents<Task>();
    }

    public T ChangeTask<T>() where T :Task
    {
        CencelAllTasks();
        for (int i = 0; i < availableTasks.Length; i++)
        {
            Task task = availableTasks[i];
            if (availableTasks[i] is T && !task.GetType().IsSubclassOf(typeof(T)))
            {
                task.enabled = true;
                return task as T;
            }
        }
        new System.Exception("Command not available for this unit");
        return null;
    }

    public void CencelAllTasks()
    {
        for (int i = 0; i < availableTasks.Length; i++)
        {
            if (availableTasks[i].enabled)
            {
                availableTasks[i].enabled = false;
            }
        }
    }


}
