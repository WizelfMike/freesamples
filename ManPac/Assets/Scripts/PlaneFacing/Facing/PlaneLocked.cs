using System;
using UnityEngine;

public class PlaneLocked : MonoBehaviour, IFacePlane
{
    [SerializeField] private Plane facingPlane;

    public IPlane GetFacingPlane() => facingPlane;

    public Vector3 FaceCamera()
    {
        var curPos = transform.position;
        
        var targetPos = facingPlane.Project(curPos);
        var targetDir = curPos - targetPos;
        
        var newDir = Quaternion.LookRotation(targetDir);
        transform.rotation = newDir;
        return newDir.eulerAngles;
    }
}
