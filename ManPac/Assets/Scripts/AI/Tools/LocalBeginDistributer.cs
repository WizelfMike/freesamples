using UnityEngine;
using UnityEngine.Events;

public class LocalBeginDistributer : MonoBehaviour
{
    public UnityEvent OnEpisodeStarted;

    public void EpisodeStarted()
    {
        OnEpisodeStarted.Invoke();
    }
    
}