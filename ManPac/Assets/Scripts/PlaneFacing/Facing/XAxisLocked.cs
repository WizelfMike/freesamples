    using System;
    using UnityEngine;

    public class XAxisLocked : MonoBehaviour, IFacePlane
    {
        [SerializeField] private Plane facingPlane;

        public IPlane GetFacingPlane() => facingPlane;

        public Vector3 FaceCamera()
        {
            Vector3 dir = facingPlane.Transform.position - transform.position;
            Quaternion newDir = Quaternion.LookRotation(dir);

            dir = newDir.eulerAngles;
            dir.x = 0;
            transform.eulerAngles = dir;

            return newDir.eulerAngles;
        }

        private void Update()
        {
            FaceCamera();
        }
    }