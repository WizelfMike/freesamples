using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TurnToRandom", story: "Turn [player] to random with [intersectionNode]", category: "Action", id: "33aea51992b5aa2accd0a09ee1257641")]
public partial class TurnToRandomAction : Action
{
    [SerializeReference] public BlackboardVariable<IntersectionTraverser> Player;
    [SerializeReference] public BlackboardVariable<CheckIntersectionDirections> IntersectionNode;

    protected override Status OnStart()
    {
        MoveToRandom();
        return Status.Running;
    }

    private Vector2 RandomDirection()
    {
        // gives a random number between 0 and the amount of directions in an intersection
        IntersectionNode node = IntersectionNode.Value.IntersectionNode;
        int randomNode = Random.Range(0, node.IntersectionDirections.Length);
        Debug.Log(node.IntersectionDirections.Length);
        return node.IntersectionDirections[randomNode];
    }

    private Status MoveToRandom()
    {
        Player.Value.GivePreferredDirection(RandomDirection());
        return Status.Success;
    }
}

