    using UnityEngine;

    public class XAxisLocked : MonoBehaviour, IFacePlane
    {
        [SerializeField]
        private Plane FacingPlane;

        public IPlane GetFacingPlane() => FacingPlane;

        public Vector3 FaceCamera()
        {
            Vector3 dir = FacingPlane.Transform.position - transform.position;
            Quaternion newDir = Quaternion.LookRotation(dir);

            dir = newDir.eulerAngles;
            dir.x = 0;
            transform.eulerAngles = dir;

            return newDir.eulerAngles;
        }
    }