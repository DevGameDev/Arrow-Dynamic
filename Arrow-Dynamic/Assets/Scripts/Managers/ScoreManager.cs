using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; set; }
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rankText;
    public GameObject scoreChangeTextPrefab;
    public GameObject canvas;

    private float timer;
    private float score;
    private int rank;

    public float initialScore = 100.0f;
    public float scoreDecayRate = 0.01f;
    public float basicArrowScoreCost = 0f;
    public float iceArrowScoreCost = 0.1f;
    public float grappleArrowScoreCost = 1f;
    public float bombArrowScoreCost = 0.1f;
    public float gravityArrowScoreCost = 0.5f;
    public float windArrowScoreCost = 0.5f;
    public float timeArrowScoreCost = 0.5f;
    public float bubbleArrowScoreCost = 0.1f;

    private void Start()
    {
        timer = 0f;
        score = initialScore;
        UpdateRank();
    }

    private void Update()
    {
        UpdateTimer();
        UpdateScore();
        UpdateRank();
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    private void SpawnScoreChangeText(float scoreChange)
    {
        GameObject scoreChangeTextObject = Instantiate(scoreChangeTextPrefab, scoreChangeTextPrefab.transform.position, Quaternion.identity, canvas.transform);

        scoreChangeTextObject.SetActive(true);

        ScoreChangeText scoreChangeText = scoreChangeTextObject.GetComponent<ScoreChangeText>();

        StartCoroutine(scoreChangeText.ScoreChangeTextEffect(scoreChange));
    }

    private float totalTimeDecay = 0f;
    private float nextDecayMilestone = 0.1f;
    private void UpdateScore()
    {
        score -= scoreDecayRate * Time.deltaTime;
        totalTimeDecay += scoreDecayRate * Time.deltaTime;
        if (totalTimeDecay > nextDecayMilestone)
        {
            SpawnScoreChangeText(-0.1f);
            nextDecayMilestone += 0.1f;
        }
        score = Mathf.Max(score, 0f);
        scoreText.text = score.ToString("F1");
    }

    private void UpdateRank()
    {
        int newRank = CalculateRank(score);
        if (newRank != rank)
        {
            rank = newRank;
            rankText.text = GetRankName(rank);
        }
    }

    private int CalculateRank(float currentScore)
    {
        if (currentScore >= 90.0f)
            return 5;
        else if (currentScore >= 80.0f)
            return 4;
        else if (currentScore >= 70.0f)
            return 3;
        else if (currentScore >= 50.0f)
            return 2;
        else
            return 1;
    }

    private string GetRankName(int rank)
    {
        switch (rank)
        {
            case 5:
                rankText.color = new Color(184, 240, 255); // Diamond
                return "S";
            case 4:
                rankText.color = new Color(255, 215, 0); // Gold
                return "A";
            case 3:
                rankText.color = new Color(192, 192, 192); // Silver
                return "B";
            case 2:
                rankText.color = new Color(176, 126, 44); // Bronze
                return "C";
            default:
                rankText.color = new Color(207, 39, 39); // Red
                return "D";
        }

        Canvas.ForceUpdateCanvases();
    }

    public void UseArrow(ArrowType arrowType)
    {
        float arrowCost = GetArrowCost(arrowType);
        score -= arrowCost;
        SpawnScoreChangeText(-arrowCost);
        score = Mathf.Max(score, 0f);
    }

    private float GetArrowCost(ArrowType arrowType)
    {
        switch (arrowType)
        {
            case ArrowType.Basic:
                return basicArrowScoreCost;
            case ArrowType.Grapple:
                return grappleArrowScoreCost;
            case ArrowType.Bomb:
                return bombArrowScoreCost;
            case ArrowType.Time:
                return timeArrowScoreCost;
            case ArrowType.Wind:
                return windArrowScoreCost;
            case ArrowType.Gravity:
                return gravityArrowScoreCost;
            case ArrowType.Bubble:
                return bubbleArrowScoreCost;
            case ArrowType.Ice:
                return iceArrowScoreCost;
            default:
                return 0f;
        }
    }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }
}