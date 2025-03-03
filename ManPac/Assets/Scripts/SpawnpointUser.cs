using UnityEngine;

public class SpawnpointUser : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;

    public void ToSpawnPoint()
    {
        transform.position = SpawnPoint.position;
        transform.rotation = SpawnPoint.rotation;
    }
}