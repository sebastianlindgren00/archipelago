using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
  public class PatrolAction : IAction
  {
    readonly NavMeshAgent agent;
    readonly Transform[] waypoints;
    int currentWaypoint = 0;
    float PATROL_GOAL_DISTANCE = 0.3f;

    public PatrolAction(NavMeshAgent agent, Transform[] waypoints)
    {
      this.agent = agent;
      this.waypoints = waypoints;
    }

    public NodeStatus Execute()
    {
      Debug.Log($"Patrolling to waypoint {currentWaypoint} at position {waypoints[currentWaypoint].position}");

      if (waypoints.Length == 0)
      {
        return NodeStatus.ERROR;
      }

      if (agent.remainingDistance < PATROL_GOAL_DISTANCE)
      {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // Loop back to the first waypoint if we reach the last one
        agent.SetDestination(waypoints[currentWaypoint].position);
      }

      if (currentWaypoint == waypoints.Length)
      {
        Debug.Log("Towards last waypoint");
        return NodeStatus.SUCCESS;
      }

      return NodeStatus.RUNNING;
    }

    public void Reset()
    {
      currentWaypoint = 0;
    }
  }
}
