using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Pellet : MonoBehaviour
{
    [Header("Score settings")]
    [SerializeField]
    private PelletTypes PelletType = PelletTypes.Ordinary;
    [SerializeField]
    private int PelletScore = 10;

    [Header("AnimationSettings")]
    [SerializeField]
    private Animator Animator;

    [Header("AudioSettings")]
    [SerializeField]
    private FMODEventTrack disableAudio;

    [Header("Events")]
    public UnityEvent<int, PelletTypes> OnPickedUp;
    
    private void Start()
    {
        Invoke(nameof(EnableAnimator), Random.value);
    }
    
    private void OnTriggerEnter(Collider manpac)
    {
        if (manpac.gameObject.CompareTag("ManPac"))
        {
            OnPickedUp.Invoke(PelletScore, PelletType);
            Destroy(this.gameObject);
        }
    }

    private void EnableAnimator()
    {
        Animator.enabled = true;
    }
}
