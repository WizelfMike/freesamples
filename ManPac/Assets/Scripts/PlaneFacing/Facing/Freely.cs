using UnityEngine;

public class Freely : MonoBehaviour, IFacePlane
{
    [SerializeField] 
    private Plane FacingPlane;


    public IPlane GetFacingPlane() => FacingPlane;

    public Vector3 FaceCamera()
    {
        Vector3 dir = FacingPlane.Transform.position - transform.position;
        Quaternion newDir = Quaternion.LookRotation(dir);

        transform.rotation = newDir;
        return newDir.eulerAngles;
    }
}