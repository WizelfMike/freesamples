using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUICommunicator : MonoBehaviour
{
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

    private Animator _powerPelletAnimator;
    private Animator _pelletCounterAnimator;
    private int _normalPelletCount = 0;
    
    private void Awake()
    {
        // Binding to events
        PelletMediator.OnPelletPickedUp.AddListener(OnPelletPickedUp);
        Enemy.OnBehaviourStateChanged.AddListener(OnManPacStateChange);
        
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

    private string GetPelletDisplayString()
    {
        return _normalPelletCount.ToString().PadLeft(0, '0');
    }

    private void UpdatePelletCounter()
    {
        UIPelletCounter.SetText(GetPelletDisplayString());
    }
}
