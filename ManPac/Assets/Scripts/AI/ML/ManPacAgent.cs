using System;
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
    private IntersectionTraverser[] PlayerTraversers;

    private IntersectionTraverser _traverser;
    private LevelPelletCounter _pelletCounter;
    private IntersectionNode[] _intersections;

    private void Start()
    {
        _pelletCounter = FindAnyObjectByType<LevelPelletCounter>(FindObjectsInactive.Exclude);
        _traverser = GetComponent<IntersectionTraverser>();
        
        _intersections = FindObjectsByType<IntersectionNode>(FindObjectsSortMode.None);
    }
    
    public override void OnEpisodeBegin()
    {
        OnEpisodeBegins.Invoke();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 ownPosition = transform.position;
        Vector3 closestPlayerPos = DistanceHelper.FindClosestGameObject(ownPosition, PlayerTraversers).transform.position;
        IntersectionNode closestIntersectionPos = DistanceHelper.FindClosestGameObject(ownPosition, _intersections);
        Pellet closestPellet = DistanceHelper.FindClosestGameObject(ownPosition, _pelletCounter.Pellets);
        
        // Adds 2 inputs
        sensor.AddObservation(ownPosition.ToVector2Z());
        // Adds 2 inputs
        sensor.AddObservation(closestPlayerPos.ToVector2Z());
        // Adds 2 inputs
        sensor.AddObservation(closestIntersectionPos.transform.position.ToVector2Z());
        // Adds 2 inputs
        sensor.AddObservation(closestPellet == null ? Vector2.zero : closestPellet.transform.position.ToVector2Z());
        // ------------------------------------------------ +
        // Total of 8 inputs
        
        // Adds a maximum of 8 inputs
        ReadOnlySpan<Vector2> allowedDirections = closestIntersectionPos.AllowedDirections;
        int allowedDirectionCount = Mathf.Min(allowedDirections.Length, 4);
        for (int i = 0; i < allowedDirectionCount; i++)
            sensor.AddObservation(allowedDirections[i]);
        // ------------------------------------------------ +
        // Total of 16
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        var option = new Vector2(moveX, moveY);
        
        _traverser.GivePreferredDirection(option);
    }
}
