using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IntersectionTraverser))]
public class ManPacAgent : Agent
{
    [SerializeField]
    private UnityEvent OnEpisodeBegins;

    private IntersectionTraverser _traverser;
    private Vector3 _startPosition;

    private Vector2[] _inputMapping = new[]
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };

    private void Start()
    {
        _startPosition = transform.position;
        _traverser = GetComponent<IntersectionTraverser>();
    }
    
    public override void OnEpisodeBegin()
    {
        OnEpisodeBegins.Invoke();
        transform.position = _startPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int inputOption = actions.DiscreteActions[0];
        Vector2 option = _inputMapping[inputOption];
        
        if (_traverser.VelocityVector.magnitude == 0)
            _traverser.SetBeginDirection(option);
        
        _traverser.GivePreferredDirection(option);
    }
}
