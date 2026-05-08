using UnityEngine;

public class HideZone : MonoBehaviour
{
    public AirstrikeManager airstrikeManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            airstrikeManager.isPlayerHidden = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            airstrikeManager.isPlayerHidden = false;
        }
    }
}