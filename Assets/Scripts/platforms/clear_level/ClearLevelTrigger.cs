using UnityEngine;


public class ClearLevelTrigger : MonoBehaviour
{
    private GameController gameController;
    private bool hasTriggered;


    private void Start()
    {
        gameController = GameController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        // Player has collided
        hasTriggered = true;
        gameController.completedLevel();
    }
}