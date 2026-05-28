using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    public GameObject GameOverPanel;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            GameOverPanel.SetActive(true);
        }
    }
}
