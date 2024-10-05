using System;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree
{
  public interface IStrategy
  {
    NodeStatus Execute();
    void Reset()
    {
      // Do nothing by default
    }
  }

  public class ActionStrategy : IStrategy
  {
    readonly Action doSomething;

    public ActionStrategy(Action action)
    {
      this.doSomething = action;
    }

    public NodeStatus Execute()
    {
      doSomething();
      return NodeStatus.SUCCESS;
    }
  }

  // The Condition class is a leaf node that executes a function to determine if the node should return SUCCESS or FAILURE
  public class Condition : IStrategy
  {
    readonly Func<bool> predicate;

    public Condition(Func<bool> predicate)
    {
      this.predicate = predicate;
    }

    public NodeStatus Execute()
    {
      Debug.Log(predicate() ? "SUCCESS" : "FAILURE");
      return predicate() ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
    }
  }
  public class PatrolStrategy : IStrategy
  {
    readonly NavMeshAgent agent;
    readonly Transform[] waypoints;
    int currentWaypoint = 0;
    float PATROL_GOAL_DISTANCE = 0.3f;

    public PatrolStrategy(NavMeshAgent agent, Transform[] waypoints)
    {
      this.agent = agent;
      this.waypoints = waypoints;
    }

    public NodeStatus Execute()
    {
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
        return NodeStatus.SUCCESS;
      }

      return NodeStatus.RUNNING;
    }

    public void Reset()
    {
      currentWaypoint = 0;
    }
  }

  public class ChaseStrategy : IStrategy
  {
    readonly NavMeshAgent agent;
    readonly Transform target;

    public ChaseStrategy(NavMeshAgent agent, Transform target)
    {
      this.agent = agent;
      this.target = target;
    }

    public NodeStatus Execute()
    {
      agent.SetDestination(target.position);

      if (agent.remainingDistance < 0.5f)
      {
        return NodeStatus.SUCCESS;
      }

      return NodeStatus.RUNNING;
    }

    public void Reset()
    {
      agent.ResetPath();
    }
  }
}