using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(IntersectionTraverser))]
public class PlayerMovement : MonoBehaviour
{
    private IntersectionTraverser _intersectionTraverser;

    private void Start()
    {
        _intersectionTraverser = GetComponent<IntersectionTraverser>();
        _intersectionTraverser.SetBeginDirection(Vector2.up);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        Vector2 preferredDirection = context.ReadValue<Vector2>();
        _intersectionTraverser.GivePreferredDirection(preferredDirection);

    }
}
