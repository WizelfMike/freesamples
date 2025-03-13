using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterTypes CharacterType;
    [SerializeField]
    private PlayerIndicator Indicator;
    [SerializeField]
    private GameObject SpriteObject;
    [Description("Broadcasts when an activation change took place, the boolean argument is the current active state")]
    public UnityEvent<bool> OnActivationChange;

    public PlayerCharacterTypes Type => CharacterType;

    private bool _isPlayerControlled = true;
    private PlayerInput _playerInput;
    private Animator _spriteAnimator;
    private IFacePlane _spritePlaneFacer;
    private IntersectionTraverser _traverser;
    private float _spriteStartXScale = 0f;
    private Vector3 _directionTestVector = new Vector3(0.70711f, 0f, -0.70711f);
    
    private static readonly int _animatorVelocity = Animator.StringToHash("Velocity");

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _spriteAnimator = SpriteObject.GetComponent<Animator>();
        _spritePlaneFacer = SpriteObject.GetComponent<IFacePlane>();
        _spriteStartXScale = SpriteObject.transform.localScale.x;
        _traverser = GetComponent<IntersectionTraverser>();
    }

    private void Update()
    {
        _spritePlaneFacer.FaceCamera();
        Vector3 traverserVelocity = _traverser.VelocityVector.normalized;
        _spriteAnimator.SetFloat(_animatorVelocity, traverserVelocity.magnitude);
        
        float sign = -Vector3.Dot(_directionTestVector, traverserVelocity);
        int roundedSign = Mathf.RoundToInt(sign);
        roundedSign = roundedSign == 0 ? 1 : roundedSign;
        SpriteObject.transform.localScale =
            new Vector3(roundedSign * _spriteStartXScale, _spriteStartXScale, _spriteStartXScale);
    }

    public void PlayerDeactivate()
    {
        _playerInput.enabled = false;
        _isPlayerControlled = false;
        
        // TODO! Placeholder code is here for now,
        // Fill up with Behaviour-AI to take over control.
        
        Indicator.enabled = false;
        OnActivationChange.Invoke(_isPlayerControlled);
    }

    public void PlayerActivate()
    {
        _playerInput.enabled = true;
        _isPlayerControlled = true;
        
        // TODO! Placeholder code is here for now,
        // Fill up with code to deactivate the Behaviour-AI so the player can take back control.
        
        Indicator.enabled = true;
        OnActivationChange.Invoke(_isPlayerControlled);
    }
}