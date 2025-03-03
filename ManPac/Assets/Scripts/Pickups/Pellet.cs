using UnityEngine;
using UnityEngine.Events;

public class Pellet : MonoBehaviour
{
    [SerializeField]
    private PelletTypes PelletType = PelletTypes.Ordinary;

    [SerializeField]
    private int PelletScore = 10;
    
    public UnityEvent<int, PelletTypes> OnPickedUp;
    private void OnTriggerEnter(Collider manpac)
    {
        if (manpac.gameObject.CompareTag("ManPac"))
        {
            OnPickedUp.Invoke(PelletScore, PelletType);
            Destroy(this.gameObject);
        }
    }
}
