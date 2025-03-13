using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class IntersectionTraverser : MonoBehaviour
{
    [SerializeField]
    private float Velocity = 1f;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float MinimalIntersectionProximity = 0.1f;
    [SerializeField]
    [Range(-1f, 1f)]
    private float MinimalRequiredCorrespondence = 0f;
    [SerializeField]
    [Range(-1f, 1f)]
    private float DoubleCheckThreshold = 0.7071f;
    

    [SerializeField]
    public UnityEvent<IntersectionNode> OnIntersectionInteraction;

    private float _previousVelocity = 0f;
    private Vector3 _currentDirection = Vector3.zero;
    private Vector2 _preferredDirection = Vector3.zero;

    private List<IntersectionNode> _interactingIntersections = new();

    public float CurrentVelocity
    {
        get => Velocity;
        set
        {
            _previousVelocity = Velocity;
            Velocity = value;
        }
    }
    public List<IntersectionNode> InteractingIntersections => _interactingIntersections;
    public Vector3 VelocityVector => _currentDirection * Velocity;

    private void OnValidate()
    {
        DoubleCheckThreshold = Mathf.Max(DoubleCheckThreshold, MinimalRequiredCorrespondence + 0.01f);
    }

    private void FixedUpdate()
    {
        if (CheckIntersectionProximity())
            InteractWithIntersection();
        
        transform.position += VelocityVector * Time.fixedDeltaTime;
    }

    public void SetBeginDirection(Vector2 direction)
    {
        direction.Normalize();
        _currentDirection = direction.ToVector3Z();
    }

    public Vector2 GivePreferredDirection(Vector2 newPreferred)
    {
        newPreferred.Normalize();
        
        // Turn around when the given input is the opposite of the current direction
        Vector2 twoDCurrentDirection = _currentDirection.ToVector2Z(); 
        if (Vector2.Dot(newPreferred, twoDCurrentDirection) <= -0.99f)
            TurnAround();
            
        _preferredDirection = newPreferred;
        return _preferredDirection;
    }

    public Vector2 TurnAround()
    {
        if (_interactingIntersections.Count > 0)
            return _currentDirection;
        
        _currentDirection = -_currentDirection;
        return _currentDirection;
    }

    public float RestoreVelocity()
    {
        CurrentVelocity = _previousVelocity;
        return CurrentVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IntersectionNode intersectionNode))
            return;

        _interactingIntersections.Add(intersectionNode);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IntersectionNode intersectionNode))
            _interactingIntersections.Remove(intersectionNode);

    }

    private bool CheckIntersectionProximity()
    {
        if (_interactingIntersections.Count <= 0)
            return false;

        Vector3 ownPosition = transform.position;

        IntersectionNode closestIntersection = DistanceHelper.FindClosestGameObject(ownPosition, _interactingIntersections);
        return Vector3.Distance(ownPosition, closestIntersection.transform.position) < MinimalIntersectionProximity;
    }

    private bool InteractWithIntersection()
    {
        if (_preferredDirection == Vector2.zero)
            _preferredDirection = _currentDirection.ToVector2Z();

        Vector3 ownPosition = transform.position;
        IntersectionNode closestIntersection =
            DistanceHelper.FindClosestGameObject(ownPosition, _interactingIntersections);
        
        Vector3 intersectionPosition = closestIntersection.transform.position;
        (Vector3 newDirection, float correspondence) = closestIntersection.GetDirection(_preferredDirection);

        if (correspondence <= DoubleCheckThreshold)
            (newDirection, correspondence) = closestIntersection.GetDirection(_currentDirection.ToVector2Z());
        
        if (Vector3.Dot(_currentDirection, newDirection) < 1f)
            transform.position = intersectionPosition;
        
        if (correspondence <= MinimalRequiredCorrespondence)
        {
            _currentDirection = Vector2.zero;
            OnIntersectionInteraction.Invoke(closestIntersection);
            return true;
        }
        
        if (Vector3.Dot(_currentDirection, newDirection) < 0.99f)
            _preferredDirection = Vector2.zero;
        
        _currentDirection = newDirection.normalized;
        OnIntersectionInteraction.Invoke(closestIntersection);
        return true;
    }
}