using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] 
    private Animator GhostAnimator;
    [SerializeField] 
    private Transform HomePosition;

    public bool IsInDanger { private get => _isInDanger; set => _isInDanger = value; }
    public bool CanDie { get => _canDie; set => _canDie = value; }

    private PlayerInput _thisPlayerInput;
    private PlayerMovement _thisPlayerMovement;
    private IntersectionTraverser _thisPlayerTraverser;
    private int _deathWaitTime = 2;
    private string _deathTrigger = "Dead";
    private string _reviveTrigger = "Revived";
    private bool _isInDanger = false;
    private bool _canDie = true;

    private void Start()
    {
        _thisPlayerTraverser = GetComponent<IntersectionTraverser>();
        _thisPlayerMovement = GetComponent<PlayerMovement>();
        _thisPlayerInput = GetComponent<PlayerInput>();
        
        _thisPlayerInput.enabled = true;
        
    }

    public void CallDeath()
    {
        if (!CanDie)
            return;

        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        _canDie = false;
        _thisPlayerInput.enabled = false;
        _thisPlayerTraverser.CurrentVelocity = 0f;
        GhostAnimator.SetTrigger(_deathTrigger);

        yield return new WaitForSeconds(_deathWaitTime);

        this.gameObject.transform.position = HomePosition.transform.position;
        _thisPlayerMovement.SetDirection();
        GhostAnimator.SetTrigger(_reviveTrigger);

        yield return new WaitForSeconds(_deathWaitTime);

        _thisPlayerInput.enabled = true;
        _thisPlayerTraverser.CurrentVelocity = 1f;
        _canDie = true;
    }
}
