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

  public class GrabStrategy : IStrategy
  {
    // readonly GameObject player;
    readonly CameraManager cameraManager;
    readonly PlayerMovement playerMovement;
    readonly Animator animator;
    readonly NavMeshAgent agent;
    private float timeElapsed = 0f;
    private float timeToRelease = 2f;

    public GrabStrategy(GameObject player, NavMeshAgent agent, Animator animator)
    {
      // this.player = player;
      playerMovement = player.GetComponent<PlayerMovement>();
      this.animator = animator;
      this.agent = agent;

      // Get the camera manager
      cameraManager = GameObject.FindWithTag("CameraGroup").GetComponent<CameraManager>();
    }

    public NodeStatus Execute()
    {
      // Grab the player
      animator.SetBool("isGrabbing", true);
      playerMovement.LockMovement(true);
      cameraManager.setTargetOverride(animator.transform);
      agent.isStopped = true; // Stop the warden from moving

      // wait for 2 seconds
      timeElapsed += Time.deltaTime;
      Debug.Log("Time elapsed: " + timeElapsed);
      if (timeElapsed >= timeToRelease)
      {
        Debug.Log("Releasing player");
        animator.SetBool("isGrabbing", false);
        playerMovement.LockMovement(false);
        cameraManager.setTargetOverride(null);
        agent.isStopped = false; // Resume the warden's movement
        return NodeStatus.SUCCESS;
      }

      return NodeStatus.RUNNING;
    }

    public void Reset()
    {
      // Do nothing
    }
  }

  public class LookAroundStrategy : IStrategy
  {
    readonly NavMeshAgent agent;
    readonly Transform warden;
    readonly Transform[] waypoints;
    readonly float rotationSpeed = 90f;
    readonly float timeToLook = 2f;
    float timeElapsed = 0f;

    public LookAroundStrategy(NavMeshAgent agent, Transform warden)
    {
      this.agent = agent;
      this.warden = warden;

      // Pick 3 random points around the warden to look at
      waypoints = new Transform[3];
      for (int i = 0; i < waypoints.Length; i++)
      {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 5f;
        randomDirection.y = 0;
        randomDirection += warden.position;
        waypoints[i] = new GameObject().transform;
        waypoints[i].position = randomDirection;
      }
    }

    public NodeStatus Execute()
    {
      if (waypoints.Length == 0)
      {
        return NodeStatus.ERROR;
      }

      // Wait for 2 seconds
      timeElapsed += Time.deltaTime;
      warden.rotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);

      return (timeElapsed >= timeToLook) ? NodeStatus.SUCCESS : NodeStatus.RUNNING;
    }


  }
}