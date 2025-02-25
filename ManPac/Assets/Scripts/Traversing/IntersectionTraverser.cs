using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class IntersectionTraverser : MonoBehaviour
{
    [SerializeField]
    private float Velocity = 1f;
    [SerializeField]
    [Range(0.1f, 5f)]
    private float MinimalIntersectionProximity = 0.1f;

    private Vector3 _currentDirection = Vector3.zero;
    private Vector2 _preferredDirection = Vector3.zero;
    private IntersectionNode _targetIntersection;
    private bool _mayInteractIntersection = false;

    public float CurrentVelocity => Velocity;
    public Vector3 VelocityVector => _currentDirection * Velocity;

    // Todo! Don't forget to remove
    private void Start()
    {
        _currentDirection = Vector3.forward;
        GivePreferredDirection(Vector2.left);
    }
    
    private void Update()
    {
        if (CheckIntersectionProximity())
            Debug.Log($"Interacted: {InteractWithIntersection()}");
    }

    private void FixedUpdate()
    {
        transform.position += VelocityVector * Time.fixedDeltaTime;
    }

    public Vector2 GivePreferredDirection(Vector2 newPreferred)
    {
        _preferredDirection = newPreferred;
        return _preferredDirection;
    }

    public Vector2 TurnAround()
    {
        _currentDirection = -_currentDirection;
        return _currentDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IntersectionNode>(out var intersectionNode))
            return;

        _targetIntersection = intersectionNode;
        _mayInteractIntersection = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IntersectionNode>(out _))
            _targetIntersection = null;
    }

    private bool CheckIntersectionProximity()
    {
        if (_targetIntersection == null)
            return false;

        var intersectionPosition = _targetIntersection.transform.position;
        return Vector3.Distance(transform.position, intersectionPosition) < MinimalIntersectionProximity;
    }

    private bool InteractWithIntersection()
    {
        if (!_mayInteractIntersection)
            return false;

        var intersectionPosition = _targetIntersection.transform.position;
        var currentPosition = transform.position;
        var newDirection = _targetIntersection.GetDirection(_preferredDirection);
        var perpendicular2D = Vector2.Perpendicular(new Vector2(newDirection.x, newDirection.z));
        var perpendicular3D = new Vector3(perpendicular2D.x, 0, perpendicular2D.y);
        var correctionVector = new Vector3(intersectionPosition.x * perpendicular3D.x , 0, intersectionPosition.z * perpendicular3D.z);
        
        transform.position = (currentPosition - correctionVector);
        _currentDirection = newDirection;

        _mayInteractIntersection = false;
        return true;
    }
}