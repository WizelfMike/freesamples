using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class IntersectionNode : MonoBehaviour
{
    [SerializeField]
    private Vector2[] Directions = new[]
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };

    private void OnValidate()
    {
        int directionCount = Directions.Length;
        for (var i = 0; i < directionCount; i++)
            Directions[i].Normalize();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        int directionCount = Directions.Length;
        Vector3 position = transform.position;
        for (var i = 0; i < directionCount; i++)
        {
            var offsetPosition = position + new Vector3(Directions[i].x, 0, Directions[i].y);
            Gizmos.DrawLine(position, offsetPosition);
            Gizmos.DrawSphere(offsetPosition, 0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }


    public Vector3 GetDirection(Vector2 preference)
    {
        preference.Normalize();
        
        float bestValue = -1;
        int bestIndex = 0;

        int directionCount = Directions.Length;
        for (var i = 0; i < directionCount; i++)
        {
            var currentDirection = Directions[i];
            var currentBestValue = Vector2.Dot(currentDirection, preference);
            if (currentBestValue > bestValue)
            {
                bestValue = currentBestValue;
                bestIndex = i;
            }
            
            if (Mathf.Abs(bestValue - 1) < Mathf.Epsilon)
                break;
        }

        return Directions[bestIndex];
    }
}