/*
 * This Agent script is used as an example on how Unity's ML-Agents can
 * be used and trained.
 */

using System;
using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField]
    private float MoveSpeed = 1.0f;
    [SerializeField]
    private GoalManager _Goalmanager;

    private Vector3 _startPosition;
    private bool _hasCompleted = false;
    private DateTime _startTime;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        _Goalmanager.SetTargetRandom(true);
        transform.position = _startPosition;
        _startTime = DateTime.Now;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(_Goalmanager.GoalInstance.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var moveX = actions.ContinuousActions[0];
        var moveZ = actions.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * MoveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions; 
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }

    // When the agent hits the goal
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PenaltWall"))
            SetReward(-1f);
        if (other.CompareTag("Goal"))
            SetReward(+1f);
        
        EndEpisode();
    }

    // private IEnumerator WaitForEpisodeEnd(float secondsDuration)
    // {
    //     yield return new WaitForSeconds(secondsDuration);
    //     if (_hasCompleted)
    //         yield break;
    //
    //     _hasCompleted = true;
    //     SetReward(CalcEndRewardAddition());
    //     EndEpisode();
    // }

    // private float CalcEndRewardAddition()
    // {
    //     var currentTime = DateTime.Now;
    //     var diff = (float)(currentTime - _startTime).TotalSeconds;
    //     var distance = 4.0f - Vector3.Distance(Target.position, transform.position);
    //     return distance * (1 / diff);
    // }
}
