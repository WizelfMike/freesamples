using UnityEngine;

public class EpisodeBeginPosition : EpisodeListener
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    
    public void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public override void OnEpisodeStarted()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
}
