using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicalheroScore : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI scoreboard;

    private int score;

    public static MusicalheroScore Instance { get; set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void AddPoints(int points) {
        score += points;
        UpdateScoreboard();
    }

    public void DeductPoints(int points) {
        score -= points;
        UpdateScoreboard();
    }

    private void UpdateScoreboard() {
        scoreboard.text = score.ToString();
    }
}
