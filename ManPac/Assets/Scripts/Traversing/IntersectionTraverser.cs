using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class IntersectionTraverser : MonoBehaviour
{
    [SerializeField]
    private float Velocity = 1f;
    [SerializeField]
    [Range(0.01f, 5f)]
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
        // GivePreferredDirection(Vector2.down);
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

        if (_preferredDirection == Vector2.zero)
            _preferredDirection = new Vector2(_currentDirection.x, _currentDirection.z);
        
        /*
            Todo! Edge cases for when the current direction is not
            a viable direction and no preferred direction was given
        */

        var intersectionPosition = _targetIntersection.transform.position;
        var newDirection = _targetIntersection.GetDirection(_preferredDirection);

        transform.position = intersectionPosition;
        _currentDirection = newDirection;

        _preferredDirection = Vector2.zero;
        _mayInteractIntersection = false;
        return true;
    }
}