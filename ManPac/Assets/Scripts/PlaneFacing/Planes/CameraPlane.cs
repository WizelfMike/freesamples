using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraPlane : Plane
{
    [SerializeField] private Camera FollowingCamera;

    public override Transform Transform => FollowingCamera.transform;
    public override Vector3 Normal => Transform.forward;
}
