using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Math = Unity.Mathematics.Geometry.Math;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Locate Enemy ", story: "Locate [Enemy] for [Player]", category: "Action", id: "f9a7ac1f7c0041f79049651e54d89bb1")]
public partial class LocateEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    private GameObject _player;
    private GameObject _enemy;
    private List<float> positionDifferents = new();
    protected override Status OnStart()
    {
        _player = GameObject.Find("Player");
        _enemy = GameObject.Find("ManPac");
        LocateEnemy();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    private void LocateEnemy()
    {
        positionDifferents.Clear();
        Vector2 playerPos = _player.transform.position;
        Vector2 enemyPos = _enemy.transform.position;
        positionDifferents.Add(playerPos.x - enemyPos.x);
        positionDifferents.Add(playerPos.y - enemyPos.y);
    }

    private void GetBiggerDifference()
    {
        
    }
    
}

