using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;
using System;
using System.Collections.Generic;

public class WardenController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private GameObject _player;
    private PlayerInteraction _playerInteraction;
    private Animator _animator;
    public Transform[] patrolWaypoints;
    private BehaviourTree.Tree _tree;
    private SoundManager _soundManager;
    private List<GameObject> _tracksInRange = new List<GameObject>();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        GameObject avatar = transform.Find("Avatar").gameObject;
        _animator = avatar.GetComponent<Animator>();

        _player = GameObject.Find("Player");
        _playerInteraction = _player.GetComponent<PlayerInteraction>();

        _soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();

        _tracksInRange = new List<GameObject>();

        createBehaviourTree();
    }

    void createBehaviourTree()
    {
        // Create the behaviour tree
        _tree = new BehaviourTree.Tree(gameObject.name);

        PrioritySelector actions = new PrioritySelector("Warden Logic");

        // ROOT - Look for player
        Sequence lookForPlayer = new Sequence("Look For Player", 100);
        lookForPlayer.AddChild(new Leaf("Player Detected?", new Condition(playerDetected)));

        PrioritySelector chasePlayer = new PrioritySelector("Chase Player");

        // LOOK -> CHASE -> Check for player visibility
        Sequence playerVisibility = new Sequence("Player Visibility", 100);
        playerVisibility.AddChild(new Leaf("Player Lost?", new Condition(playerLost)));
        playerVisibility.AddChild(new Leaf("LookAroundStrategy", new LookAroundStrategy(agent, transform)));
        chasePlayer.AddChild(playerVisibility);

        // LOOK -> CHASE -> GRAB 
        Sequence playerInRange = new Sequence("Player In Range");
        playerInRange.AddChild(new Leaf("Player Close?", new Condition(playerIsClose)));
        playerInRange.AddChild(new Leaf("Grab Player", new GrabStrategy(_player, agent, _animator)));
        chasePlayer.AddChild(playerInRange);

        // LOOK -> CHASE - Move towards player (default)
        chasePlayer.AddChild(new Leaf("Move Towards Player", new ChaseStrategy(agent, _player.transform)));

        lookForPlayer.AddChild(chasePlayer);
        actions.AddChild(lookForPlayer);

        // ROOT - Look for track
        Sequence lookForTrack = new Sequence("Look For Track", 50);
        lookForTrack.AddChild(new Leaf("Track Detected?", new Condition(trackDetected)));

        // LOOK -> TRACK - Move towards track
        lookForTrack.AddChild(new Leaf("Move Towards Track", new GoToTrackStrategy(agent, this)));

        // LOOK -> TRACK - Set new waypoints
        lookForTrack.AddChild(new Leaf("Set New Waypoints", new ActionStrategy(() => SetNewWaypoints(_tracksInRange[0].transform))));

        actions.AddChild(lookForTrack);

        // ROOT - Patrol (default)
        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(agent, patrolWaypoints));
        actions.AddChild(patrol);

        _tree.AddChild(actions);

        // Draw the behaviour tree in the console
        _tree.Print();
    }

    void Update()
    {
        ApplyAnimation();
        NodeStatus status = _tree.Evaluate();
        if (status == NodeStatus.SUCCESS)
        {
            _tree.Reset();
        }
    }

    private void ApplyAnimation()
    {
        // Set the speed of the player
        _animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void SetNewWaypoints(Transform track)
    {
        // Scatter waypoints around the track
        Vector3 trackPosition = track.position;
        Vector3 trackDirection = track.forward;
        Vector3 trackRight = track.right;

        Vector3[] waypoints = new Vector3[5];
        waypoints[0] = trackPosition + trackDirection * 2 + trackRight * 2;
        waypoints[1] = trackPosition + trackDirection * 2 - trackRight * 2;
        waypoints[2] = trackPosition - trackDirection * 2 + trackRight * 2;
        waypoints[3] = trackPosition - trackDirection * 2 - trackRight * 2;
        waypoints[4] = trackPosition;

        // Set the waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            patrolWaypoints[i].position = waypoints[i];
        }

    }

    // Check in a player track is within the warden's range
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Track detected!");
        // Check if the object is a track
        if (!other.CompareTag("Track"))
            return;

        // Add the object to the list of objects in range
        _tracksInRange.Add(other.gameObject);

        // Sort the list of objects in range by distance to the warden
        _tracksInRange.Sort((a, b) => Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));
    }

    // Check if a player track has left the warden's range
    private void OnTriggerExit(Collider other)
    {
        // Check if the object is a track
        if (!other.CompareTag("Track"))
            return;

        // Remove the object from the list of objects in range
        _tracksInRange.Remove(other.gameObject);
    }

    public GameObject GetClosestTrack()
    {
        if (_tracksInRange.Count == 0)
            return null;

        return _tracksInRange[0];
    }

    private bool playerDetected()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = direction.magnitude;


        if (angle < 60 && distance < 5f)
        {
            // _soundManager.StartChaseSound();
            return true;
        }
        return false;
    }

    private bool trackDetected()
    {
        // Check if there are any tracks in range
        if (_tracksInRange.Count > 0)
            return true;

        return false;
    }

    private bool playerLost()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = direction.magnitude;

        // If the player is out of sight in front of the warden and not within immediate reach
        if (distance > 2f && !(angle < 60 && distance < 5f))
        {
            Debug.Log("angle, distance " + angle + " " + distance);
            return true;
        }
        return false;
    }

    private bool playerIsClose()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < 1f)
        {
            return true;
        }
        return false;
    }

    private bool playerShineLight()
    {
        Vector3 direction = _player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < 1f)
        {
            return true;
        }
        return false;
    }
}
