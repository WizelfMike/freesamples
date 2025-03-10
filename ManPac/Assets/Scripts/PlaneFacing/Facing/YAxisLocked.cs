using System;
using UnityEngine;

public class YAxisLocked : MonoBehaviour, IFacePlane
{
    [SerializeField] private Plane facingPlane;
    [SerializeField] private GameObject floatingPoint;

    public IPlane GetFacingPlane() => facingPlane;

    public Vector3 FaceCamera()
    {
        var curPos = transform.position;
        var camTrans = facingPlane.Transform;
        var camRight = -camTrans.right;
        float dot = 0f;
        Vector3 camRelDir;
        Vector3 targetPos;
        Vector3 targetDir;
        Quaternion newDir;

        // calculate the direction to this sprite relative from the camera it faces
        camRelDir = curPos - camTrans.position;
        // calculate the dot product necessary for calculating the horizontal distance in the plane
        // with respect to the local orientation
        dot = Vector3.Dot(camRight, camRelDir);
        // with the dot product, the point right in front of this sprite can be calculated at the height of
        // the camera
        targetPos = camTrans.position + camRight * dot;
        targetDir = curPos - targetPos;

        floatingPoint.transform.position = targetPos;

        newDir = Quaternion.LookRotation(targetDir);

        transform.rotation = newDir;
        return newDir.eulerAngles;
    }

    private void Update()
    {
        FaceCamera();
    }
}