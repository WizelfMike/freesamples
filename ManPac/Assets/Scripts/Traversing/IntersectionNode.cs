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

    public Vector2[] IntersectionDirections
    {
        get => Directions;
        private set => Directions = value;
    }
    
    public ReadOnlySpan<Vector2> AllowedDirections => Directions.AsSpan();

    private void OnValidate()
    {
        int directionCount = Directions.Length;
        for (int i = 0; i < directionCount; i++)
            Directions[i].Normalize();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        int directionCount = Directions.Length;
        Vector3 position = transform.position;
        
        for (int i = 0; i < directionCount; i++)
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


    public (Vector3 direction, float correspondence) GetDirection(Vector2 preference)
    {
        preference.Normalize();
        
        float bestValue = -1;
        int bestIndex = 0;

        int directionCount = Directions.Length;
        for (int i = 0; i < directionCount; i++)
        {
            Vector2 currentDirection = Directions[i];
            float currentBestValue = Vector2.Dot(currentDirection, preference);
            if (currentBestValue > bestValue)
            {
                bestValue = currentBestValue;
                bestIndex = i;
            }
            
            // Early loop-break when the best-value is 1, meaning a perfect correspondence
            if (Mathf.Abs(bestValue - 1) < Mathf.Epsilon)
                break;
        }

        Vector2 newDirection = Directions[bestIndex];
        return (new Vector3(newDirection.x, 0, newDirection.y), bestValue);
    }
}