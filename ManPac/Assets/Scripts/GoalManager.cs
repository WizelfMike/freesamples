﻿using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TargetType;
    [SerializeField]
    private Vector3[] SpawnPoints;

    private GameObject _goalInstance;
    public GameObject GoalInstance => _goalInstance ??= GameObject.Instantiate(TargetType);

    public void SetTargetRandom(bool isLocalSpace = true)
    {
        int index = Random.Range(0, SpawnPoints.Length);
        GameObject instance = GoalInstance;
        Vector3 nextPosition = SpawnPoints[index];
        instance.transform.SetParent(transform.parent);
        
        if (isLocalSpace)
        {
            instance.transform.localPosition = nextPosition;
        }
        else
        {
            instance.transform.position = nextPosition;
        }
    }
}