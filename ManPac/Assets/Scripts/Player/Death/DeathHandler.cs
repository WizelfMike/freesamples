using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathHandler : MonoBehaviour
{

    private PlayerInput ThisPlayerInput;
    private IntersectionTraverser PlayerTraverser;
    private bool isActivePlayer = true;

    private void Start()
    {
        ThisPlayerInput = GetComponent<PlayerInput>();
        PlayerTraverser = GetComponent<IntersectionTraverser>();

        if (isActivePlayer == true)
        {
            ThisPlayerInput.enabled = true;
        }
    }

    private IEnumerator Death()
    {
        ThisPlayerInput.enabled = false;
        PlayerTraverser.CurrentVelocity = 0f;

        yield return new WaitForSeconds(5);

        ThisPlayerInput.enabled = true;
        PlayerTraverser.CurrentVelocity = 1f;
    }

    public void CallDeath()
    {
        StartCoroutine(Death());
    }
}
