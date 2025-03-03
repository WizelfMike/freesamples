using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpawnpointUser))]
[RequireComponent(typeof(IntersectionTraverser))]
public class ManPacEnemy : MonoBehaviour
{
    [SerializeField]
    private Vector2 BeginDirection = Vector2.up;
    [SerializeField]
    private UnityEvent<GameObject> OnGotHitByPlayer;

    private IntersectionTraverser _traverser;
    private SpawnpointUser _spawnpointUser;
    
    private void OnValidate()
    {
        BeginDirection.Normalize();
    }

    private void Start()
    {
        _traverser = GetComponent<IntersectionTraverser>();
        _spawnpointUser = GetComponent<SpawnpointUser>();
        
        _spawnpointUser.ToSpawnPoint();
        _traverser.SetBeginDirection(BeginDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnGotHitByPlayer.Invoke(other.gameObject);
    }

    public void OnAgentEpisodeBegan()
    {
        _spawnpointUser.ToSpawnPoint();
        _traverser.SetBeginDirection(BeginDirection);
    }
}