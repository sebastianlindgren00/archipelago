using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

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

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        GameObject avatar = transform.Find("Avatar").gameObject;
        _animator = avatar.GetComponent<Animator>();

        _player = GameObject.Find("Player");
        _playerInteraction = _player.GetComponent<PlayerInteraction>();

        _soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();

        createBehaviourTree();
    }

    void createBehaviourTree()
    {
        // Create the behaviour tree
        _tree = new BehaviourTree.Tree(gameObject.name);

        PrioritySelector actions = new PrioritySelector("Warden Logic");

        Sequence trackPlayer = new Sequence("Track Player", 100);
        trackPlayer.AddChild(new Leaf("Player Detected?", new Condition(playerDetected)));

        PrioritySelector chasePlayer = new PrioritySelector("Chase Player");

        // CHASE - Check for player visibility
        Sequence playerVisibility = new Sequence("Player Visibility", 100);
        playerVisibility.AddChild(new Leaf("Player Lost?", new Condition(playerLost)));
        playerVisibility.AddChild(new Leaf("LookAroundStrategy", new LookAroundStrategy(agent, transform)));
        chasePlayer.AddChild(playerVisibility);

        // CHASE -> GRAB 
        Sequence playerInRange = new Sequence("Player In Range");
        playerInRange.AddChild(new Leaf("Player Close?", new Condition(playerIsClose)));
        playerInRange.AddChild(new Leaf("Grab Player", new GrabStrategy(_player, agent, _animator)));
        chasePlayer.AddChild(playerInRange);

        // CHASE - Move towards player (default)
        chasePlayer.AddChild(new Leaf("Move Towards Player", new ChaseStrategy(agent, _player.transform)));

        trackPlayer.AddChild(chasePlayer);
        actions.AddChild(trackPlayer);

        // ROOT - Patrol (default)
        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(agent, patrolWaypoints));
        actions.AddChild(patrol);

        _tree.AddChild(actions);
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
