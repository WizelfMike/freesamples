using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlane : Plane
{
    [SerializeField] private Camera followingCamera;

    public override Transform Transform => followingCamera.transform;
    public override Vector3 Normal => Transform.forward;
}
