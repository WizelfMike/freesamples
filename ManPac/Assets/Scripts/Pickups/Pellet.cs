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
    
    [Header("Events")]
    public UnityEvent<Pellet> OnPickedUp;

    public PelletTypes Type => PelletType;
    public int Score => PelletScore;
    
    public PelletTypes Type => PelletType;
    
    private void Start()
    {
        Invoke(nameof(EnableAnimator), Random.value);
    }
    
    private void OnTriggerEnter(Collider manpac)
    {
        if (manpac.gameObject.CompareTag("ManPac"))
        {
            OnPickedUp.Invoke(this);
            Destroy(this.gameObject);
        }
    }

    private void EnableAnimator()
    {
        Animator.enabled = true;
    }
}
