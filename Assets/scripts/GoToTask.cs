using UnityEngine;
using System.Collections.Generic;

public class GoToTask : Task {
    protected NavMeshAgent agent;
    static Dictionary<Vector3, int> unitsPerDestination = new Dictionary<Vector3, int>();
    protected const float parkingDistance = 3;
    protected float stoppingDistance;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - agent.destination).magnitude < stoppingDistance)
        {
            OnDisable();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        if (unitsPerDestination.ContainsKey(agent.destination))
        {
            unitsPerDestination[agent.destination] = unitsPerDestination[agent.destination] - 1;
            if (unitsPerDestination[agent.destination] == 0)
            {
                unitsPerDestination.Remove(agent.destination);
            }
        }
        if (!unitsPerDestination.ContainsKey(destination))
        {
            unitsPerDestination.Add(destination, 0);
        }
        unitsPerDestination[destination] = unitsPerDestination[destination] + 1;
        stoppingDistance = Mathf.Sqrt((agent.radius * agent.radius + parkingDistance) * unitsPerDestination[destination]);
        agent.destination = destination;
        agent.avoidancePriority = Mathf.Min(99, Mathf.RoundToInt(Mathf.Sqrt(unitsPerDestination[destination])));
    }

    protected override void OnDisable()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.Stop();
            agent.ResetPath();
        }
    }

}
