using System.Collections.Generic;
using UnityEngine;

public class CheckOnIntersection : MonoBehaviour
{
    [SerializeField]
    private IntersectionTraverser IntersectionTraverser;

    [SerializeField] 
    private CheckIntersectionDirections CheckIntersectionDirections;
    
    [HideInInspector] 
    public bool mayActivate;

    private IntersectionNode _intersectionNode;

    
    public void OnIntersection()
    {
        if (_intersectionNode == IntersectionTraverser.InteractingIntersections[0])
            return;
        
        _intersectionNode = IntersectionTraverser.InteractingIntersections[0];
        CheckIntersectionDirections.SetIntersectionNode(_intersectionNode);
        mayActivate = true;
    }
}
