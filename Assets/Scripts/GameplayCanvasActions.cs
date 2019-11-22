using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvasActions : MonoBehaviour
{
    [SerializeField]
    private FloatReference RemainingTime;
    [SerializeField]
    private Text remainingTimeText;

    private bool isGameOver;

    private void Update()
    {
        UpdateLevelTimer();
    }

    private void UpdateLevelTimer()
    {
        if (isGameOver)
            return;
        remainingTimeText.text = FormattedTimer(RemainingTime.Value);
    }

    public void GoalGathered()
    {
        isGameOver = true;
    }

    private string FormattedTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.RoundToInt(timer % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
