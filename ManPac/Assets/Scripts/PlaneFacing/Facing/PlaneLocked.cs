using UnityEngine;

public class PlaneLocked : MonoBehaviour, IFacePlane
{
    [SerializeField]
    private Plane FacingPlane;

    public IPlane GetFacingPlane() => FacingPlane;

    public Vector3 FaceCamera()
    {
        Vector3 curPos = transform.position;
        
        Vector3 targetPos = FacingPlane.Project(curPos);
        Vector3 targetDir = curPos - targetPos;
        
        Quaternion newDir = Quaternion.LookRotation(targetDir);
        transform.rotation = newDir;
        return newDir.eulerAngles;
    }
}
