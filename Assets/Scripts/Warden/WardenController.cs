using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WardenController : MonoBehaviour
{
    private NavMeshAgent agent;
    // [SerializeField] private float movementSpeed = 5f;
    // [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Debug.Log(agent);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        Debug.Log(player.transform.position);
    }
}
