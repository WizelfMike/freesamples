using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour, IPlane
{
    protected static Vector2 Size = Vector2.one;
    
    public virtual Transform Transform => transform;
    public virtual Vector3 Normal => Transform.forward;
    
    public Vector3 Project(Vector3 position)
    {
        Vector3 dir = position - Transform.position;
        Vector3 right = Transform.right;
        Vector3 up = Transform.up;

        float xDot = Vector3.Dot(dir, right);
        float yDot = Vector3.Dot(dir, up);

        return Transform.position + xDot * right + yDot * up;
    }

    private void OnDrawGizmos()
    {
        Quaternion rotation = Quaternion.LookRotation(Transform.forward);
        Matrix4x4 trs = Matrix4x4.TRS(Transform.position, rotation, Vector3.one);
        Gizmos.matrix = trs;
        Color32 color = Color.blue;
        color.a = 125;
        Gizmos.color = color;
        Gizmos.DrawCube(Vector3.zero, new Vector3(Size.x, Size.y, 0.001f));
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.white;
    }
}
