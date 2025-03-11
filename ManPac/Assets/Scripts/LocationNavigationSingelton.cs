using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationNavigationSingelton : MonoBehaviour
{
    [SerializeField]
    private  GameObject _enemy;

    public float PositionDifference 
    {
        get => _closestDistance;
        private set => _closestDistance = value;
    }

    public float SecondClosest
    {
        get => _secondClosestDistance;
        private set => _secondClosestDistance = value;
    }

    public bool IsXClosest
    {
        get => _isXClosest;
        private set => _isXClosest = value;
    }

    private static List<float> _positionDifferents = new();
    private static float _closestDistance;
    private static float _secondClosestDistance;
    private static bool _isXClosest;
    
    public void LocateEnemy(GameObject player)
    {
        _positionDifferents.Clear();
        Vector3 playerPos = player.transform.localPosition;
        Vector3 enemyPos = _enemy.transform.localPosition;
        _positionDifferents.Add(playerPos.x - enemyPos.x);
        _positionDifferents.Add(playerPos.z - enemyPos.z);
        GetBiggerDifference();
    }

    private void GetBiggerDifference()
    {
        if (_positionDifferents[0] - _positionDifferents[1] >= 0)
        {
             _isXClosest = true;
            _closestDistance = _positionDifferents[0];
            _secondClosestDistance = _positionDifferents[1];
        }

        _isXClosest = false;
        _closestDistance = _positionDifferents[1];
        _secondClosestDistance = _positionDifferents[0];
    }
}
