using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicalheroScore : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI scoreboard;

    private int score;
    private int penalty;

    public static MusicalheroScore Instance { get; set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void SetPenalty(int penalty) {
        this.penalty = penalty;
    }

    public void AddPoints(int points) {
        score += points;
        UpdateScoreboard();
    }

    public void DeductPoints() {
        score -= penalty;
        UpdateScoreboard();
    }

    private void UpdateScoreboard() {
        scoreboard.text = score.ToString();
    }
}
