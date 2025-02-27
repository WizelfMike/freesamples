using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    public void OnNoPellets()
    {
        Debug.Log("You Lost!");
    }

    public void OnManPacGotHit(GameObject player)
    {
        Debug.Log("You Won!");
    }
}