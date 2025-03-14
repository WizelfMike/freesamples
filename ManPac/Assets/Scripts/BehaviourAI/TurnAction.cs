using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Turn", story: "turn [Player] to [Direction] with [singleton] and [CheckDirection]", category: "Action", id: "74db563bc010564b997149a3211da501")]
public partial class TurnAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<IntersectionTraverser> Direction;
    [SerializeReference] public BlackboardVariable<LocationNavigationSingelton> Singleton;
    [SerializeReference] public BlackboardVariable<CheckIntersectionDirections> CheckDirection;

    private bool _isRetry;
    
    private List<Vector2> Directions = new()
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1)
    };

    protected override Status OnStart()
    {
        Singleton.Value.LocateEnemy(Player.Value);
        CheckDirections();
        return Status.Running;
    }

    private Vector2 ChooseDirection(bool isRetry)
    {
        bool IsX = Singleton.Value.IsXClosest;
        float difference = Singleton.Value.PositionDifference;
        if (isRetry)
        {
            IsX = !IsX;
            difference = Singleton.Value.SecondClosest;
        }
        
        List<Vector2> chosenDirection = new();

        if (IsX)
        {
            chosenDirection.Add(Directions[0]);
            chosenDirection.Add(Directions[1]);
        }
        else
        {
            chosenDirection.Add(Directions[2]);
            chosenDirection.Add(Directions[3]);
        }

        if (difference >= 0)
            return chosenDirection[1];

        return chosenDirection[0];
    }

    private Status MovePlayer(Vector2 moveDirection)
    {
        _isRetry = false;
        Direction.Value.GivePreferredDirection(moveDirection);
        return Status.Success;
    }

    private void CheckDirections()
    {
        Vector2 direction = ChooseDirection(_isRetry);
        bool canUseDirection = CheckDirection.Value.CanUseDirection(direction);
        if (canUseDirection)
        {
            MovePlayer(direction);
            return;
        }

        if (_isRetry)
        { 
            // gives a random number between 0 and 1
            int randomNode = Random.Range(0, 2);
            MovePlayer(CheckDirection.Value.IntersectionNode.AllowedDirections[randomNode]);
            return;
        }

        _isRetry = true;
        CheckDirections();
    }
}

