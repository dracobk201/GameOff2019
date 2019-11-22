using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvasActions : MonoBehaviour
{
    [SerializeField]
    private FloatReference RemainingTime;
    [SerializeField]
    private StringReference SceneToChange;
    [SerializeField]
    private GameEvent ChangeSceneEvent;
    [SerializeField]
    private Text remainingTimeText;

    public void ShowGameOverPanel()
    {
        remainingTimeText.text = FormattedTimer(RemainingTime.Value);
    }

    public void RestartButtonPressed()
    {
        SceneToChange.Value = Global.FIRSTLEVELSCENE;
        ChangeSceneEvent.Raise();
    }

    public void QuitButtonPressed()
    {
        SceneToChange.Value = Global.MAINMENUSCENE;
        ChangeSceneEvent.Raise();
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
