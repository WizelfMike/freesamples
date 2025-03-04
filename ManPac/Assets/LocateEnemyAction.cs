using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Locate Enemy ", story: "Locate [Enemy] for [Player]", category: "Action", id: "f9a7ac1f7c0041f79049651e54d89bb1")]
public partial class LocateEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    protected override Status OnStart()
    {
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
        GameObject player = Player.Value;
        GameObject enemy = Enemy.Value;
        Vector2 positionDifference = player.transform.position - enemy.transform.position;
        Debug.Log(positionDifference);
    }
    
}

