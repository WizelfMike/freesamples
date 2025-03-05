using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathHandler : MonoBehaviour
{

    [SerializeField] private Animator GhostAnimator;
    [SerializeField] private Transform HomePosition;

    private PlayerInput _thisPlayerInput;
    private PlayerMovement _thisPlayerMovement;
    private IntersectionTraverser _thisPlayerTraverser;
    //private bool _isActivePlayer = true;
    private bool _isInDanger;
    private bool _canDie;

    public bool IsInDanger { private get => _isInDanger; set => _isInDanger = value; }
    public bool CanDie { get => _canDie; set => _canDie = value; }

    private void Start()
    {
        _canDie = true;
        _isInDanger = false;
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
        GhostAnimator.SetTrigger("Died");

        yield return new WaitForSeconds(2);

        this.gameObject.transform.position = HomePosition.transform.position;
        _thisPlayerMovement.SetDirection();
        GhostAnimator.SetTrigger("Revived");

        yield return new WaitForSeconds(2);

        _thisPlayerInput.enabled = true;
        _thisPlayerTraverser.CurrentVelocity = 1f;
        _canDie = true;
    }
}
