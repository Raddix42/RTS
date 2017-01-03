using UnityEngine;

public class GoToUnitTask : GoToTask
{
    public Transform UnitToFollow;

    void Update()
    {
            if (!(agent.destination.x == UnitToFollow.position.x && agent.destination.z == UnitToFollow.position.z))
            {
                SetDestination(UnitToFollow.position);
                agent.Resume();
            }
            if ((transform.position - agent.destination).magnitude < stoppingDistance + transform.localScale.magnitude)
            {
                agent.Stop();
            }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnitToFollow = null;
    }

}
