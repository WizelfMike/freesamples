using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(IntersectionTraverser))]
public class PlayerMovement : MonoBehaviour
{
    private IntersectionTraverser _intersectionTraverser;
    [SerializeField]
    private Vector2 BeginDirection = new (0,1);

    private void Start()
    {
        _intersectionTraverser = GetComponent<IntersectionTraverser>();
        _intersectionTraverser.SetBeginDirection(BeginDirection);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        Vector2 preferredDirection = context.ReadValue<Vector2>();
        _intersectionTraverser.GivePreferredDirection(preferredDirection);

    }
}
