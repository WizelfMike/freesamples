using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpawnpointUser))]
[RequireComponent(typeof(IntersectionTraverser))]
public class ManPacEnemy : MonoBehaviour
{
    [SerializeField]
    private Vector2 BeginDirection = Vector2.up;
    [SerializeField]
    [Description("Duration of the agressive state in seconds")]
    private float AggressiveDuration = 10f;
    [SerializeField]
    private int StartingLiveCount = 3;
    
    [Header("Events")]
    [SerializeField]
    public UnityEvent<GameObject> OnGotHitByPlayer;
    [SerializeField]
    public UnityEvent<GameObject> OnDied;
    [SerializeField]
    public UnityEvent<ManPacStates> OnBehaviourStateChanged;

    public int TotalLiveCount => StartingLiveCount;
    public int CurrentLiveCount => _currentLives;

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

    private void Start()
    {
        _currentLives = StartingLiveCount;
        _traverser = GetComponent<IntersectionTraverser>();
        _spawnpointUser = GetComponent<SpawnpointUser>();
        
        _spawnpointUser.ToSpawnPoint();
        _traverser.SetBeginDirection(BeginDirection);

        _aggressiveTimer = new DeltaTimer(AggressiveDuration)
        {
            OnTimerRanOut = OnAggressiveRanOut
        };

        _invincibilityTimer = new DeltaTimer(5f);
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
            other.GetComponent<DeathHandler>().CallDeath();
        }

        if (other.CompareTag("Player") && _currentState == ManPacStates.Avoidant && other.GetComponent<DeathHandler>().CanDie == true)
        {
            if (_invincibilityTimer.IsRunning)
                return;
            
            GotHitByPlayer(other);
        }
    }

    private void GotHitByPlayer(Collider playerCollider)
    {
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

    public void OnPelletPickedUp(int scoreAddition, PelletTypes pelletType)
    {
        // only change to aggressive state when power-pellet was picked up
        if (pelletType != PelletTypes.Power)
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
        OnBehaviourStateChanged.Invoke(_currentState);
    }
}