using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [Tooltip("Drag your TextMeshProUGUI component here.")]
    public TMP_Text timerText;

    void Update()
    {
        // Get the live elapsed time
        float t = GameManager.Instance.elapsedTime;

        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        int milliseconds = (int)((t * 1000) % 1000);

        // Format as 00:00.000
        timerText.text = string.Format("{0:00}:{1:00}.{2:000}",
                                        minutes, seconds, milliseconds);
    }
}