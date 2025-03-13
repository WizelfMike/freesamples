using UnityEngine;

[RequireComponent(typeof(IntersectionTraverser))]
public class ForwardLooker : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 2f)]
    [Tooltip("The speed in direction change in seconds")]
    private float LookSpeed = 0.1f;

    private IntersectionTraverser _traverser;

    private void Start()
    {
        _traverser = GetComponent<IntersectionTraverser>();
    }

    private void Update()
    {
        UpdateLookDirection(Time.deltaTime);
    }

    private void UpdateLookDirection(float deltaTime)
    {
        Vector3 currentForward = transform.forward;
        Vector3 nextDirection =
            Vector3.Slerp(currentForward, GetTraverserDirection(), (1 / LookSpeed) * deltaTime);

        transform.forward = nextDirection;
    }

    public Vector3 GetTraverserDirection() =>
        _traverser.VelocityVector.normalized;

}