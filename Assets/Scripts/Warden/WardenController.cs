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

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        GameObject avatar = transform.Find("Avatar").gameObject;
        _animator = avatar.GetComponent<Animator>();

    }

    private void ApplyAnimation()
    {
        // Set the speed of the player
        _animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
