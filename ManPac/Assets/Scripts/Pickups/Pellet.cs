using System.Collections;
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

    [Header("Death Settings")]
    [SerializeField]
    private int TimeToRespawn;

    [Header("AnimationSettings")]
    [SerializeField]
    private Animator Animator;
    
    [Header("Events")]
    public UnityEvent<int, PelletTypes> OnPickedUp;

    private SphereCollider _sphereCollider;
    
    public PelletTypes Type => PelletType;
    
    private void Start()
    {
        Invoke(nameof(EnableAnimator), Random.value);

        gameObject.GetComponent<SphereCollider>();
    }
    
    private void OnTriggerEnter(Collider manpac)
    {
        if (manpac.gameObject.CompareTag("ManPac"))
        {
            OnPickedUp.Invoke(PelletScore, PelletType);
            handlePellet();
        }
    }

    private void EnableAnimator()
    {
        Animator.enabled = true;
    }

    private void handlePellet()
    {
        if (PelletType == PelletTypes.Ordinary)
        {
            Destroy(gameObject);
        }

        if(PelletType == PelletTypes.Power)
        {
            StartCoroutine(RespawnTimer());
        }
    }

    private void hidePellet(bool hasWaited)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(hasWaited);
        }

        _sphereCollider.enabled = hasWaited;
    }

    private IEnumerator RespawnTimer()
    {
        hidePellet(false);

        yield return new WaitForSeconds(TimeToRespawn);

        hidePellet(true);
    }
}
