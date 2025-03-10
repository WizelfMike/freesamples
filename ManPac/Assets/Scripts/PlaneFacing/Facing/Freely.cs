using System;
using UnityEngine;

public class Freely : MonoBehaviour, IFacePlane
{
    [SerializeField] private Plane facingPlane;

    public IPlane GetFacingPlane() => facingPlane;

    public Vector3 FaceCamera()
    {
        Vector3 dir = facingPlane.Transform.position - transform.position;
        Quaternion newDir = Quaternion.LookRotation(dir);

        transform.rotation = newDir;
        return newDir.eulerAngles;
    }

    private void Update()
    {
        FaceCamera();
    }
}