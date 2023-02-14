using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText: MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreDisplayText;
    private void Start()
    {
        CustomEventManager.Instance.OnSetScore += OnSetScore;
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnSetScore -= OnSetScore;
    }

    private void OnSetScore(Vector2Int newScore)
    {
        ScoreDisplayText.text = $"<color=#F0684B>{newScore.x}</color> • <color=#6A70BD>{newScore.y}</color>";
    }
}
