using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUICommunicator : MonoBehaviour
{
    [Serializable]
    private struct PlayerUIStates
    {
        public PlayerCharacterTypes Type;
        public Sprite ActiveSprite;
        public Sprite InactiveSprite;
        public Sprite DeadSprite;
    }

    private enum CharacterStates
    {
        Dead,
        Inactive,
        Active
    }
    
    [Header("Power Pellet")]
    [SerializeField]
    private ManPacEnemy Enemy;
    [SerializeField]
    private Image PowerPelletImage;
    [SerializeField]
    private string PowerPelletActiveBoolean = "Active";

    [Header("Pellet Counter")]
    [SerializeField]
    private PelletMediator PelletMediator;
    [SerializeField]
    private TextMeshProUGUI UIPelletCounter;
    [SerializeField]
    private string PelletPickupAnimationActivation = "Activate";

    [Header("Character status")]
    [SerializeField]
    private CharacterSwitcher CharacterSwitcher;
    [SerializeField]
    private Image UIBlinkyImage;
    [SerializeField]
    private Image UIPinkyImage;
    [SerializeField]
    private Image UIInkyImage;
    [SerializeField]
    private Image UIClydeImage;
    [SerializeField]
    private PlayerUIStates[] UIDisplayStates;
    [SerializeField]
    private string ActiveTrigger = "ToActive";
    [SerializeField]
    private string InactiveTrigger = "ToInactive";
    [SerializeField]
    private string DeadTrigger = "ToDead";

    private Animator _powerPelletAnimator;
    private Animator _pelletCounterAnimator;
    private int _normalPelletCount = 0;

    private Image[] _uiImageMap;
    private Animator[] _characterUiAnimators;
    private CharacterStates[] _characterStates;
    
    private void Awake()
    {
        // setting up the imagemap
        _uiImageMap = new []
        {
            UIBlinkyImage,
            UIPinkyImage,
            UIInkyImage,
            UIClydeImage
        };

        _characterStates = new[]
        {
            CharacterStates.Inactive,
            CharacterStates.Inactive,
            CharacterStates.Inactive,
            CharacterStates.Inactive
        };

        _characterUiAnimators = new[]
        {
            UIBlinkyImage.GetComponent<Animator>(),
            UIPinkyImage.GetComponent<Animator>(),
            UIInkyImage.GetComponent<Animator>(),
            UIClydeImage.GetComponent<Animator>(),
        };
        
        // Binding to events
        PelletMediator.OnPelletPickedUp.AddListener(OnPelletPickedUp);
        Enemy.OnBehaviourStateChanged.AddListener(OnManPacStateChange);
        CharacterSwitcher.OnCharacterActivated.AddListener(OnCharacterSwitched);
        
        // Setting up the animators
        _powerPelletAnimator = PowerPelletImage.GetComponent<Animator>();
        _pelletCounterAnimator = UIPelletCounter.transform.parent.GetComponent<Animator>();
        
        // Getting the ordinary pellet count in the scene
        _normalPelletCount = FindObjectsByType<Pellet>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Count(pellet => pellet.Type == PelletTypes.Ordinary);
        
        UpdatePelletCounter();
    }

    private void OnManPacStateChange(ManPacStates newState)
    {
        _powerPelletAnimator.SetBool(PowerPelletActiveBoolean, newState == ManPacStates.Aggressive);
    }

    private void OnPelletPickedUp(int score, PelletTypes pelletType)
    {
        if (pelletType != PelletTypes.Ordinary)
            return;

        _normalPelletCount -= 1;
        UpdatePelletCounter();
        _pelletCounterAnimator.SetTrigger(PelletPickupAnimationActivation);
    }

    private void OnCharacterSwitched(PlayerCharacter nowControlledCharacter)
    {
        int characterUiCount = _uiImageMap.Length;
        int nowControlledIndex = (int) nowControlledCharacter.Type;
        for (int i = 0; i < characterUiCount; i++)
        {
            if (_characterStates[i] == CharacterStates.Dead)
                continue;

            _characterStates[i] = CharacterStates.Inactive;
        }

        if (_characterStates[nowControlledIndex] == CharacterStates.Inactive)
            _characterStates[nowControlledIndex] = CharacterStates.Active;
        
        UpdateCharacterStateUI();
    }

    private string GetPelletDisplayString()
    {
        return _normalPelletCount.ToString().PadLeft(0, '0');
    }

    private void UpdatePelletCounter()
    {
        UIPelletCounter.SetText(GetPelletDisplayString());
    }

    private void UpdateCharacterStateUI()
    {
        int uiDisplayStateCount = UIDisplayStates.Length;
        for (int i = 0; i < uiDisplayStateCount; i++)
        {
            PlayerUIStates state = UIDisplayStates[i];
            CharacterStates characterState = _characterStates[(int) state.Type];
            Animator characterUIAnimator = _characterUiAnimators[(int) state.Type];
            Image characterUIImage = _uiImageMap[(int) state.Type];
            
            // Force reset all triggers
            characterUIAnimator.ResetTrigger(DeadTrigger);
            characterUIAnimator.ResetTrigger(InactiveTrigger);
            characterUIAnimator.ResetTrigger(ActiveTrigger);

            switch (characterState)
            {
                case CharacterStates.Dead:
                    characterUIAnimator.SetTrigger(DeadTrigger);
                    characterUIImage.sprite = state.DeadSprite;
                    break;
                case CharacterStates.Inactive:
                    characterUIAnimator.SetTrigger(InactiveTrigger);
                    characterUIImage.sprite = state.InactiveSprite;
                    break;
                case CharacterStates.Active:
                    characterUIAnimator.SetTrigger(ActiveTrigger);
                    characterUIImage.sprite = state.ActiveSprite;
                    break;
            }
        }
    }
}
