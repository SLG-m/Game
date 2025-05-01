using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.ShowWinPanel();
            }
        }
    }
}