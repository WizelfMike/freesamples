using UnityEngine;

[RequireComponent(typeof(PlaneLocked))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerIndicator : MonoBehaviour
{
    private IFacePlane _planeFacer;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _planeFacer = GetComponent<PlaneLocked>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Update()
    {
        _planeFacer.FaceCamera();
    }
}