using UnityEngine;

[RequireComponent(typeof(IntersectionTraverser))]
public class EpisodeBeginDirection : EpisodeListener
{
    [SerializeField]
    private Vector2 BeginDirection;
    
    private IntersectionTraverser _traverser;

    private void Awake()
    {
        _traverser = GetComponent<IntersectionTraverser>();
    }
    
    public override void OnEpisodeStarted()
    {
        _traverser.SetBeginDirection(BeginDirection);
    }
}