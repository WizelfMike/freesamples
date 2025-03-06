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

    [Header("AnimationSettings")]
    [SerializeField]
    private Animator Animator;
    
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

        gameObject.GetComponent<SphereCollider>().enabled = hasWaited;
    }

    private IEnumerator RespawnTimer()
    {
        hidePellet(false);

        yield return new WaitForSeconds(20);

        hidePellet(true);
    }
}
