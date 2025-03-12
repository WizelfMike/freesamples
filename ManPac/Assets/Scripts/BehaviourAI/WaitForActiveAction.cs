using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForActive", story: "wait for [Intersection]", category: "Action", id: "7fb8ff4469723a07b6953a6cb0cd9093")]
public partial class WaitForActiveAction : Action
{
    [SerializeReference] public BlackboardVariable<CheckOnIntersection> Intersection;

    protected override Status OnUpdate()
    {
        if (HasCompleted())
            return Status.Success;

        return Status.Running;    
    }

    private bool HasCompleted()
    {
        if (Intersection.Value.mayActivate)
        {
            Intersection.Value.mayActivate = false;
            return true;
        }
        return false;
    }
}

