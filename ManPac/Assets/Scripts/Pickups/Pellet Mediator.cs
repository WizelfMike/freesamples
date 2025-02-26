using UnityEngine;

public class PelletMediator : MonoBehaviour
{
    private int pointAmount;

    public void AddScore(int amount) // Function is public because it needs to be called from a Unity Event
    {
        pointAmount += amount;
        Debug.Log(pointAmount);
    }
}
