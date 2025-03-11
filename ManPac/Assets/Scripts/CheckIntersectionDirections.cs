using System.Linq;
using UnityEngine;

public class CheckIntersectionDirections : MonoBehaviour
{
    private IntersectionNode _intersectionNode;
    
    public bool CanUseDirection(Vector2 direction)
    {
        int directionAmount = _intersectionNode.IntersectionDirections.Length;
        for (int i = 0; i < directionAmount; i++)
        {
            if (_intersectionNode.IntersectionDirections[i] == direction)
            {
                return true;
            }
        }
        return false;
    }

    public void SetIntersectionNode(IntersectionNode intersectionNode)
    {
        _intersectionNode = intersectionNode;
    }
}
