using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int HomeScore { get; private set; } = 0;
    public int AwayScore { get; private set; } = 0;

    // Event that fires when the score changes (home, away)
    public event Action<int, int> OnScoreChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddHomePoints(int points)
    {
        if (points <= 0) return;

        HomeScore += points;
        OnScoreChanged?.Invoke(HomeScore, AwayScore);
        Debug.Log($"Home scored +{points}. New score: {HomeScore}-{AwayScore}");
    }

    public void AddAwayPoints(int points)
    {
        if (points <= 0) return;

        AwayScore += points;
        OnScoreChanged?.Invoke(HomeScore, AwayScore);
        Debug.Log($"Away scored +{points}. New score: {HomeScore}-{AwayScore}");
    }

    public void ResetScore()
    {
        HomeScore = 0;
        AwayScore = 0;
        OnScoreChanged?.Invoke(HomeScore, AwayScore);
        Debug.Log("Score reset to 0-0");
    }
}
