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

    [SerializeField]
    private IntersectionTraverser[] playerTraversers;

    private IntersectionTraverser _traverser;
    private Vector3[] _intersectionLocations;

    private Vector2[] _inputMapping = new[]
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };

    private void Start()
    {
        _traverser = GetComponent<IntersectionTraverser>();
        
        IntersectionNode[] intersections = FindObjectsByType<IntersectionNode>(FindObjectsSortMode.None);
        int intersectionCount = intersections.Length;
        _intersectionLocations = new Vector3[intersectionCount];

        for (var i = 0; i < intersectionCount; i++)
            _intersectionLocations[i] = intersections[i].transform.position;
    }
    
    public override void OnEpisodeBegin()
    {
        OnEpisodeBegins.Invoke();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 ownPosition = transform.position;
        Vector3 closestPlayerPos = DistanceHelper.FindClosestGameObject(ownPosition, playerTraversers).transform.position;
        Vector3 closestIntersectionPos = DistanceHelper.GetClosest(ownPosition, _intersectionLocations);
        
        // Adds 3 inputs
        sensor.AddObservation(ownPosition);
        // Adds 3 inputs
        sensor.AddObservation(closestPlayerPos);
        // Adds 3 inputs
        sensor.AddObservation(closestIntersectionPos);
        // ------------------------------------------------ +
        // Total of 9 inputs
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int inputOption = actions.DiscreteActions[0];
        Vector2 option = _inputMapping[inputOption];
        
        _traverser.GivePreferredDirection(option);
    }
}
