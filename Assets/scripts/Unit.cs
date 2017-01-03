using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    [System.NonSerialized]
    public Task[] availableCommands;

    void Start()
    {
        availableCommands = GetComponents<Task>();
    }

    public T ChangeCommand<T>() where T :Task
    {
        DisableAllCommands();
        for (int i = 0; i < availableCommands.Length; i++)
        {
            Task command = availableCommands[i];
            if (availableCommands[i] is T && !command.GetType().IsSubclassOf(typeof(T)))
            {
                command.enabled = true;
                return command as T;
            }
        }
        new System.Exception("Command not available for this unit");
        return null;
    }

    public void DisableAllCommands()
    {
        for (int i = 0; i < availableCommands.Length; i++)
        {
            if (availableCommands[i].enabled)
            {
                availableCommands[i].enabled = false;
            }
        }
    }


}
