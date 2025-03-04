using UnityEngine;

public class EpisodeBeginPosition : EpisodeListener
{
    private Transform _startTransform;
    
    public override void Start()
    {
        _startTransform = transform;
        
        base.Start();
    }

    public override void OnEpisodeStarted()
    {
        transform.position = _startTransform.position;
        transform.rotation = _startTransform.rotation;
    }
}
