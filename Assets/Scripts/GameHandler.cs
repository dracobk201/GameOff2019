using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private FloatReference InitialTimer;
    [SerializeField]
    private FloatReference RemainingTime;
    [SerializeField]
    private GameEvent GameOverEvent;

    private bool isGameOver;

    private void Start()
    {
        RemainingTime.Value = InitialTimer.Value;
        isGameOver = false;
    }

    private void Update()
    {
        UpdateLevelTimer();
    }

    private void UpdateLevelTimer()
    {
        if (isGameOver)
            return;
        
        RemainingTime.Value -= Time.deltaTime;

        if (RemainingTime.Value <= 0)
        {
            GameOverEvent.Raise();
        }
    }

    public void GoalReached()
    {
        isGameOver = true;
    }
}
