using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] 
    private Animator GhostAnimator;
    [SerializeField] 
    private Transform HomePosition;
    [SerializeField]
    public UnityEvent<PlayerCharacter> OnDied;
    [SerializeField]
    public UnityEvent<PlayerCharacter> OnRevived;

    public bool IsInDanger { private get => _isInDanger; set => _isInDanger = value; }
    public bool CanDie { get => _canDie; set => _canDie = value; }
    public bool IsDead => !CanDie;

    private PlayerInput _thisPlayerInput;
    private PlayerMovement _thisPlayerMovement;
    private IntersectionTraverser _thisPlayerTraverser;
    private PlayerCharacter _playerCharacter;
    private int _deathWaitTime = 2;
    private string _deathTrigger = "Died";
    private string _reviveTrigger = "Revived";
    private bool _isInDanger = false;
    private bool _canDie = true;

    private void Start()
    {
        _thisPlayerTraverser = GetComponent<IntersectionTraverser>();
        _thisPlayerMovement = GetComponent<PlayerMovement>();
        _thisPlayerInput = GetComponent<PlayerInput>();
        _playerCharacter = GetComponent<PlayerCharacter>();
        
        _thisPlayerInput.enabled = true;
        
    }

    public void CallDeath()
    {
        if (!CanDie)
            return;

        StartCoroutine(Death());
    }

    public void ChangeGhostMovementSpeed(float slowSpeed)
    {
        _thisPlayerTraverser.CurrentVelocity = _isInDanger?
            slowSpeed : 1f;
    }

    private IEnumerator Death()
    {
        _canDie = false;
        _thisPlayerInput.enabled = false;
        _thisPlayerTraverser.CurrentVelocity = 0f;
        GhostAnimator.SetTrigger(_deathTrigger);
        OnDied.Invoke(_playerCharacter);

        yield return new WaitForSeconds(_deathWaitTime);

        this.gameObject.transform.position = HomePosition.transform.position;
        _thisPlayerMovement.SetDirection();
        GhostAnimator.SetTrigger(_reviveTrigger);

        yield return new WaitForSeconds(_deathWaitTime);

        _thisPlayerInput.enabled = true;
        _thisPlayerTraverser.CurrentVelocity = 1f;
        _canDie = true;
        OnRevived.Invoke(_playerCharacter);
    }
}
