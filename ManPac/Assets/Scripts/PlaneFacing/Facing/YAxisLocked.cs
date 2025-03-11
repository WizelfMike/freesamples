using UnityEngine;

public class YAxisLocked : MonoBehaviour, IFacePlane
{
    [SerializeField]
    private Plane FacingPlane;
    public IPlane GetFacingPlane() => FacingPlane;

    public Vector3 FaceCamera()
    {
        Vector3 curPos = transform.position;
        Transform camTrans = FacingPlane.Transform;
        Vector3 camRight = -camTrans.right;
        float dot = 0f;
        Vector3 camRelDir;
        Vector3 targetPos;
        Vector3 targetDir;
        Quaternion newDir;

        camRelDir = curPos - camTrans.position;
        dot = Vector3.Dot(camRight, camRelDir);
        targetPos = camTrans.position + camRight * dot;
        targetDir = curPos - targetPos;

        newDir = Quaternion.LookRotation(targetDir);

        transform.rotation = newDir;
        return newDir.eulerAngles;
    }
}