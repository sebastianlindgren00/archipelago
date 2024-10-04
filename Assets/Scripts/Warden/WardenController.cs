using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WardenController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject player;
    private Animator _animator;

    private BehaviourTree.Tree tree;
    public Transform[] patrolWaypoints;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        GameObject avatar = transform.Find("Avatar").gameObject;
        _animator = avatar.GetComponent<Animator>();

        tree = new BehaviourTree.Tree("Warden");
        tree.AddChild(new BehaviourTree.Leaf("Patrol", new BehaviourTree.PatrolAction(agent, patrolWaypoints)));
    }

    void Update()
    {
        ApplyAnimation();
        tree.Evaluate();
    }

    private void ApplyAnimation()
    {
        // Set the speed of the player
        _animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
