using System;
using UnityEngine;

public interface IPlane
{
    public Transform Transform { get; }
    public Vector3 Normal { get; }

    public Vector3 Project(Vector3 position);
}