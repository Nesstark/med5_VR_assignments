using UnityEngine;
using TMPro;

public class RetroScoreboard : MonoBehaviour
{
    public static RetroScoreboard Instance { get; private set; }

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;

    [Header("Animation (Optional)")]
    public Transform flipPanel;
    public float flipDuration = 0.2f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged += UpdateDisplay;

        UpdateDisplay(0, 0);
    }

    void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int homeScore, int awayScore)
    {
        if (scoreText)
            scoreText.text = $"{homeScore}-{awayScore}";

        if (flipPanel)
            StartCoroutine(SimpleFlip());
    }

    private System.Collections.IEnumerator SimpleFlip()
    {
        float half = flipDuration / 2f;
        Quaternion start = flipPanel.localRotation;
        Quaternion mid = Quaternion.Euler(
            flipPanel.localEulerAngles.x + 90f,
            flipPanel.localEulerAngles.y,
            flipPanel.localEulerAngles.z
        );

        float t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            flipPanel.localRotation = Quaternion.Slerp(start, mid, t / half);
            yield return null;
        }

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            flipPanel.localRotation = Quaternion.Slerp(mid, start, t / half);
            yield return null;
        }
    }
}
