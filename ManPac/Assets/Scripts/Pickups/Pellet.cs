using UnityEngine;
using UnityEngine.Events;

public class Pellet : MonoBehaviour
{
    public UnityEvent onPickedUp;
    private void OnTriggerEnter(Collider manpac)
    {
        if (manpac.gameObject.CompareTag("ManPac"))
        {
            onPickedUp.Invoke();
            Destroy(this.gameObject);
        }
    }
}
