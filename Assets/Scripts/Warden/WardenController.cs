using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class WardenController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject player;
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

        _soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();

        // Create the behaviour tree
        _tree = new BehaviourTree.Tree(gameObject.name);

        PrioritySelector actions = new PrioritySelector("Warden Logic");
        Sequence chasePlayer = new Sequence("Chase Player", 100);
        chasePlayer.AddChild(new Leaf("playerInSight?", new Condition(playerInSight)));
        PrioritySelector chasePlayerPriority = new PrioritySelector("Chase Player Priority");
        chasePlayerPriority.AddChild(new Leaf("Chase Player", new ChaseStrategy(agent, player.transform)));
        Inverter inSightInverter = new Inverter("Invert Player In Sight", 50);
        inSightInverter.AddChild(new Leaf("playerInSight?", new Condition(playerInSight)));
        inSightInverter.AddChild(new Leaf("Reset Chase", new ActionStrategy(() => agent.ResetPath())));
        chasePlayerPriority.AddChild(inSightInverter);
        chasePlayer.AddChild(chasePlayerPriority);
        actions.AddChild(chasePlayer);

        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(agent, patrolWaypoints));
        actions.AddChild(patrol);

        _tree.AddChild(actions);
    }

    void Update()
    {
        ApplyAnimation();
        _tree.Evaluate();
    }

    private void ApplyAnimation()
    {
        // Set the speed of the player
        _animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private bool playerInSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = direction.magnitude;


        if (angle < 60 && distance < 10f)
        {
            _soundManager.StartChaseSound();
            return true;
        }
        return false;
    }
}
