using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DistanceHelper
{
    private static int FindClosestOnIndex(Vector3 position, ReadOnlySpan<Vector3> otherPositions)
    {
         float closestDistance = float.PositiveInfinity;
         int closestIndex = -1;
 
         int intersectionCount = otherPositions.Length;
         for (int i = 0; i < intersectionCount; i++)
         {
             float distance = Vector3.Distance(position, otherPositions[i]);
             if (distance < closestDistance)
             {
                 closestDistance = distance;
                 closestIndex = i;
             }
         }
 
         return closestIndex;       
    }
    
    public static Vector3 GetClosest(Vector3 position, ReadOnlySpan<Vector3> otherPositions)
    {
        int index = FindClosestOnIndex(position, otherPositions);
        return otherPositions[index];
    }

    public static T FindClosestGameObject<T>(Vector3 position, ReadOnlySpan<T> others)
        where T : Component
    {
        int othersLength = others.Length;
        Vector3[] positions = new Vector3[othersLength];
        for (int i = 0; i < othersLength; i++)
            positions[i] = others[i].transform.position;
        
        int index = FindClosestOnIndex(position, positions);
        return others[index];
    }

    public static T FindClosestGameObject<T>(Vector3 position, IReadOnlyList<T> others)
        where T : Component
    {
         int othersLength = others.Count;
         Vector3[] positions = new Vector3[othersLength];
         for (int i = 0; i < othersLength; i++)
             positions[i] = others[i].transform.position;
         
         int index = FindClosestOnIndex(position, positions);
         return others[index];       
    }
}