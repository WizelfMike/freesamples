using UnityEngine;

[RequireComponent(typeof(LocalBeginDistributer))]
public abstract class EpisodeListener : MonoBehaviour
{
    public virtual void Start()
    {
        GetComponent<LocalBeginDistributer>().OnEpisodeStarted.AddListener(OnEpisodeStarted);
    }
    
    public abstract void OnEpisodeStarted();
}
