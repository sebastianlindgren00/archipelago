using UnityEngine;
using System.Collections.Generic;

public class PlayerTrailManager : MonoBehaviour
{
    public GameObject TrailPointPrefab;
    [SerializeField] private float _trailFrequency = 2f;
    private float _timeSinceLastPoint = 0f;
    private List<GameObject> _trailPoints = new List<GameObject>();
    private GameObject _player;
    public GameObject PlayerTrailContainer;

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
        GameObject trailPoint = Instantiate(TrailPointPrefab, _player.transform.position, _player.transform.rotation, PlayerTrailContainer.transform);
        _trailPoints.Add(trailPoint);
    }

}