using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Rendering;
using Math = Unity.Mathematics.Geometry.Math;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Locate Enemy ", story: "Locate [Enemy] for [Player]", category: "Action", id: "f9a7ac1f7c0041f79049651e54d89bb1")]
public partial class LocateEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Enemy;
    [SerializeReference] public BlackboardVariable<string> Player;
    private GameObject _player;
    private GameObject _enemy;
    private List<float> _positionDifferents = new();
    private List<Vector2> _directionsX = new()
    {
        new Vector2(1,0),
        new Vector2(-1,0)
    };
    private List<Vector2> _directionsY = new()
    {
        new Vector2(0, 1),
        new Vector2(0, -1)
    };
    private List<Vector2> _fastestDirections = new();

    public List<Vector2> FastestDirections
    {
        get => _fastestDirections;
        set => _fastestDirections = value;
    }

    protected override Status OnStart()
    {
        _player = GameObject.Find(Player.Value);
        _enemy = GameObject.Find(Enemy.Value);
        LocateEnemy();
        GetBiggerDifference();
        return Status.Success;
    }

    private void LocateEnemy()
    {
        _positionDifferents.Clear();
        Vector2 playerPos = _player.transform.position;
        Vector2 enemyPos = _enemy.transform.position;
        _positionDifferents.Add(playerPos.x - enemyPos.x);
        _positionDifferents.Add(playerPos.y - enemyPos.y);
    }

    private float GetBiggerDifference()
    {
        if (_positionDifferents[0] - _positionDifferents[1] >= 0)
        {
            _fastestDirections = _directionsX;
            return _positionDifferents[0];
        }
        
        _fastestDirections = _directionsY;
        return _positionDifferents[1];
    }
}

