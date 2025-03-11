using UnityEngine;
using UnityEngine.UI;

public class LevelUICommunicator : MonoBehaviour
{
    [Header("Power Pellet")]
    [SerializeField]
    private Image PowerPelletImage;
    [SerializeField]
    private string PowerPelletActiveBoolean = "Active";

    private Animator _powerPelletAnimator;

    private void Awake()
    {
        _powerPelletAnimator = PowerPelletImage.GetComponent<Animator>();
    }

    public void OnManPacStateChange(ManPacStates newState)
    {
        _powerPelletAnimator.SetBool(PowerPelletActiveBoolean, newState == ManPacStates.Aggressive);
    }
}
