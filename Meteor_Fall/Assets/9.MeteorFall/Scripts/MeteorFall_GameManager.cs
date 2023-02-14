using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeteorFall_GameManager: MonoBehaviour
{
    public static MeteorFall_GameManager Instance;

    public Action OnSpawnWarningZone;
    public Action<bool> OnRedDie;
    public Action OnJoyStick;
    public Action OnBotMove;
    public Action OnEndGame;

    private int RedScores = 3;
    private int BlueScores = 3;
    private int LoseScores = 0;
    private bool IsCanSpawnWarningZone;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CustomEventManager.Instance.OnNewGame += StartGame;
        CustomEventManager.Instance.OnSetScore(new Vector2Int(RedScores, BlueScores));
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnNewGame -= StartGame;
    }

    private void StartGame()
    {
        StartCoroutine(StartGameCountdown(2));
        OnJoyStick();

        if (OnBotMove != null)
        {
            OnBotMove();
        }
    }

    public void SetScore(bool isRedDie)
    {
        if (isRedDie)
        {
            RedScores--;
            OnRedDie(true);
        }
        else
        {
            BlueScores--;
            OnRedDie(false);
        }
        IsCanSpawnWarningZone = true;
        StartCoroutine(CheckScoreCoroutine());
    }

    IEnumerator CheckScoreCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if(RedScores == LoseScores && BlueScores == LoseScores)
        {
            RedScores++;
            BlueScores++;
            yield break;
        }

        CustomEventManager.Instance.OnSetScore(new Vector2Int(RedScores, BlueScores));
        if (RedScores == LoseScores)
        {
            CustomEventManager.Instance.OnGameOver(false);
            OnEndGame();
            yield break;
        }

        if (BlueScores == LoseScores)
        {
            CustomEventManager.Instance.OnGameOver(true);
            OnEndGame();
            yield break;
        }

        if (IsCanSpawnWarningZone == false) yield break;
        IsCanSpawnWarningZone = false;
        StartCoroutine(StartGameCountdown(2f));
    }

    IEnumerator StartGameCountdown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnSpawnWarningZone();
    }
}
