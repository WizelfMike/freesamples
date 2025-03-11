using System.Collections;
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

    [Header("Animation")]
    [SerializeField]
    private Animator ModelAnimator;
    
    [Header("Events")]
    [SerializeField]
    private UnityEvent<GameObject> OnGotHitByPlayer;
    [SerializeField]
    private UnityEvent<ManPacStates> OnBehaviourStateChanged;

    private IntersectionTraverser _traverser;
    private SpawnpointUser _spawnpointUser;
    private ManPacStates _currentState = ManPacStates.Avoidant;
    private DeltaTimer _aggressiveTimer;
    private bool _isPlayerHit = false;
    private static readonly int Velocity = Animator.StringToHash("Velocity");

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

        _aggressiveTimer = new DeltaTimer(AggressiveDuration)
        {
            OnTimerRanOut = OnAggressiveRanOut
        };
    }

    private void Update()
    {
        if (_aggressiveTimer.IsRunning && _currentState == ManPacStates.Aggressive)
            _aggressiveTimer.Update(Time.deltaTime);
        
        ModelAnimator.SetFloat(Velocity, _traverser.VelocityVector.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && _currentState == ManPacStates.Aggressive)
        {
            other.GetComponent<DeathHandler>().CallDeath();
        }

        if (other.CompareTag("Player") && _currentState == ManPacStates.Avoidant && other.GetComponent<DeathHandler>().CanDie == true)
        {
            if (!_isPlayerHit)
                StartCoroutine(OnPlayerHit(other));
        }
    }

    public IEnumerator OnPlayerHit(Collider playerCollider)
    {
        _isPlayerHit = true;
        OnGotHitByPlayer.Invoke(playerCollider.gameObject);
        ModelAnimator.SetTrigger("DeathTrigger");
        _traverser.enabled = false;
        
        yield return new WaitForSeconds(3f);

        _traverser.enabled = true;
        _isPlayerHit = false;
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