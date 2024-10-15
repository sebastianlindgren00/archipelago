using UnityEngine;
using System.Collections.Generic;

public class PlayerTrailManager : MonoBehaviour
{
    public GameObject TrailPointPrefab;
    [SerializeField] private float _trailFrequency = 2f;
    private float _timeSinceLastPoint = 0f;
    private List<GameObject> _trailPoints = new List<GameObject>();
    private GameObject _player;

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        _timeSinceLastPoint += Time.deltaTime;
        if (_timeSinceLastPoint >= _trailFrequency)
        {
            CreateTrailPoint();
            _timeSinceLastPoint = 0f;
        }
    }

    void CreateTrailPoint()
    {
        Vector3 playerPosition = _player.transform.position;
        GameObject trailPoint = Instantiate(TrailPointPrefab, playerPosition, _player.transform.rotation);
        _trailPoints.Add(trailPoint);
    }

}