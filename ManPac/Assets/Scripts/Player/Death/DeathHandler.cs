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
    private bool _isActivePlayer = true;

    private void Start()
    {
        _thisPlayerTraverser = GetComponent<IntersectionTraverser>();
        _thisPlayerMovement = GetComponent<PlayerMovement>();
        _thisPlayerInput = GetComponent<PlayerInput>();

        GhostAnimator = GetComponentInChildren<Animator>();

        if (_isActivePlayer == true)
        {
            _thisPlayerInput.enabled = true;
        }
    }

    private void Update() // will be removed once the true method of calling death is functional
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            CallDeath();
        }
    }

    private IEnumerator Death()
    {
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
        
    }

    public void CallDeath()
    {
        StartCoroutine(Death());
    }
}
