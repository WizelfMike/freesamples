using System;
using System.ComponentModel;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ManPacAgent))]
[RequireComponent(typeof(SpawnpointUser))]
[RequireComponent(typeof(IntersectionTraverser))]
public class ManPacEnemy : MonoBehaviour
{
    [SerializeField]
    private Vector2 BeginDirection = Vector2.up;
    [SerializeField]
    [Description("Duration of the agressive state in seconds")]
    private float AggressiveDuration = 10f;

    [Header("Models")]
    [SerializeField]
    private ModelAsset AvoidantModel;
    [SerializeField]
    private ModelAsset AggressiveModel;

    [Header("Lifes")]
    [SerializeField]
    private float InvincibilityDuration = 5f;
    [SerializeField]
    private int StartingLiveCount = 3;
    
    [Header("Events")]
    public UnityEvent<GameObject> OnGotHitByPlayer;
    public UnityEvent<GameObject> OnDied;
    public UnityEvent<ManPacStates> OnBehaviourStateChanged;

    public int TotalLiveCount => StartingLiveCount;
    public int CurrentLiveCount => _currentLives;

    private ManPacAgent _agent;
    private IntersectionTraverser _traverser;
    private SpawnpointUser _spawnpointUser;
    private ManPacStates _currentState = ManPacStates.Avoidant;
    private DeltaTimer _aggressiveTimer;
    private DeltaTimer _invincibilityTimer;
    private int _currentLives;
    
    private void OnValidate()
    {
        BeginDirection.Normalize();
    }

    private void Awake()
    {
        _currentLives = StartingLiveCount;

        _agent = GetComponent<ManPacAgent>();
        _traverser = GetComponent<IntersectionTraverser>();
        _spawnpointUser = GetComponent<SpawnpointUser>();
        
        _aggressiveTimer = new DeltaTimer(AggressiveDuration)
        {
            OnTimerRanOut = OnAggressiveRanOut
        };

        _invincibilityTimer = new DeltaTimer(InvincibilityDuration);
    }

    private void Start()
    {
        _spawnpointUser.ToSpawnPoint();
        _traverser.SetBeginDirection(BeginDirection);
    }

    private void Update()
    {
        if (_aggressiveTimer.IsRunning && _currentState == ManPacStates.Aggressive)
            _aggressiveTimer.Update(Time.deltaTime);
        
        if (_invincibilityTimer.IsRunning)
            _invincibilityTimer.Update(Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _currentState == ManPacStates.Aggressive)
        {
            HitsPlayer(other);
        }

        if (other.CompareTag("Player") && _currentState == ManPacStates.Avoidant && other.GetComponent<DeathHandler>().CanDie == true)
        {
            if (!_invincibilityTimer.IsRunning)
                GotHitByPlayer(other);
        }
    }

    private void HitsPlayer(Collider playerCollider)
    {
        _agent.AddReward(100f);
        playerCollider.GetComponent<DeathHandler>().CallDeath();
    }

    private void GotHitByPlayer(Collider playerCollider)
    {
        _agent.AddReward(-100f);

        _invincibilityTimer.Reset();
        
        _currentLives -= 1;
        
        OnGotHitByPlayer.Invoke(playerCollider.gameObject);
        if (_currentLives <= 0)
            OnDied.Invoke(playerCollider.gameObject);
    }

    public void OnAgentEpisodeBegan()
    {
        _spawnpointUser.ToSpawnPoint();
        _traverser.SetBeginDirection(BeginDirection);
    }

    public void OnPelletPickedUp(Pellet pellet)
    {
        _agent.AddReward(pellet.Score);
        
        // only change to aggressive state when power-pellet was picked up
        if (pellet.Type != PelletTypes.Power)
            return;
        
        _aggressiveTimer.Reset();
        ChangeState(ManPacStates.Aggressive);
    }

    private void OnAggressiveRanOut()
    {
        ChangeState(ManPacStates.Avoidant);
    }
    
    private void ChangeState(ManPacStates newState)
    {
        if (newState == _currentState)
            return;
        
        _currentState = newState;
        ChangeAIModel(_currentState);
        OnBehaviourStateChanged.Invoke(_currentState);
    }

    private void ChangeAIModel(ManPacStates newState)
    {
        switch (newState)
        {
            case ManPacStates.Avoidant:
                _agent.SetModel("Enemy", AvoidantModel);
                break;
            case ManPacStates.Aggressive:
                _agent.SetModel("Enemy", AggressiveModel);
                break;
        }
    }
}