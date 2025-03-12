using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelPelletCounter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnNoPellets;

    private List<Pellet> _levelPellets;

    public IReadOnlyList<Pellet> Pellets => _levelPellets.AsReadOnly();
    public int PelletCount => _levelPellets.Count;

    private void Awake()
    {
        _levelPellets = FindObjectsByType<Pellet>(FindObjectsSortMode.None).ToList();
    }

    public void ListenToPelletPickup(Pellet pellet)
    {
        _levelPellets.Remove(pellet);
        if (OrdinaryCount() <= 0)
            OnNoPellets.Invoke();
    }

    private int OrdinaryCount()
    {
        return Pellets.Count(x => x.Type == PelletTypes.Ordinary);
    }
}
