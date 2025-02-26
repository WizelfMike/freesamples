using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class IntersectionTraverser : MonoBehaviour
{
    [SerializeField]
    private float Velocity = 1f;
    [SerializeField]
    [Range(0.01f, 5f)]
    private float MinimalIntersectionProximity = 0.1f;

    [SerializeField]
    public UnityEvent<IntersectionNode> OnIntersectionInteraction;

    private float _previousVelocity = 0f;
    private Vector3 _currentDirection = Vector3.zero;
    private Vector2 _preferredDirection = Vector3.zero;
    private IntersectionNode _targetIntersection;
    private bool _mayInteractIntersection = false;

    public float CurrentVelocity
    {
        get => Velocity;
        set
        {
            _previousVelocity = Velocity;
            Velocity = value;
        }
    }
    public Vector3 VelocityVector => _currentDirection * Velocity;

    private void FixedUpdate()
    {
        if (CheckIntersectionProximity())
            InteractWithIntersection();
        
        transform.position += VelocityVector * Time.fixedDeltaTime;
    }

    public void SetBeginDirection(Vector2 direction)
    {
        direction.Normalize();
        _currentDirection = new Vector3(direction.x, 0, direction.y);
    }

    public Vector2 GivePreferredDirection(Vector2 newPreferred)
    {
        newPreferred.Normalize();
        
        // Turn around when the given input is the opposite of the current direction
        Vector2 twoDCurrentDirection = new Vector2(_currentDirection.x, _currentDirection.z);
        if (Vector2.Dot(newPreferred, twoDCurrentDirection) <= -0.99f)
            TurnAround();
            
        _preferredDirection = newPreferred;
        return _preferredDirection;
    }

    public Vector2 TurnAround()
    {
        if (_targetIntersection is not null)
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

        Vector3 intersectionPosition = _targetIntersection.transform.position;
        return Vector3.Distance(transform.position, intersectionPosition) < MinimalIntersectionProximity;
    }

    private bool InteractWithIntersection()
    {
        if (!_mayInteractIntersection)
            return false;

        if (_preferredDirection == Vector2.zero)
            _preferredDirection = new Vector2(_currentDirection.x, _currentDirection.z);
        
        Vector3 intersectionPosition = _targetIntersection.transform.position;
        (Vector3 newDirection, float correspondence)= _targetIntersection.GetDirection(_preferredDirection);

        transform.position = intersectionPosition;
        if (correspondence <= 0.01f)
        {
            _currentDirection = Vector2.zero;
            OnIntersectionInteraction.Invoke(_targetIntersection);
            return true;
        }
        
        _currentDirection = newDirection.normalized;

        _preferredDirection = Vector2.zero;
        _mayInteractIntersection = false;
        OnIntersectionInteraction.Invoke(_targetIntersection);
        return true;
    }
}