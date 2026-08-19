using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject Ball;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            GameOverPanel.SetActive(true);
            Ball.SetActive(false);
        }
    }
}
