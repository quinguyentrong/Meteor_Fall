using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeteorFall_GameManager : MonoBehaviour
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

        CustomEventManager.Instance.OnSetScore(new Vector2Int(3, 3));
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnNewGame -= StartGame;
    }

    private void StartGame()
    {
        StartCoroutine(StartGameCountdown(3));
        OnJoyStick();
        OnBotMove();
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

        CustomEventManager.Instance.OnSetScore(new Vector2Int(RedScores, BlueScores));

        if (RedScores == LoseScores)
        {
            CustomEventManager.Instance.OnGameOver(false);
            OnEndGame();
            return;
        }

        if (BlueScores == LoseScores)
        {
            CustomEventManager.Instance.OnGameOver(true);
            OnEndGame();
            return;
        }
        StartCoroutine(StartGameCountdown(2));
    }

    IEnumerator StartGameCountdown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnSpawnWarningZone();
    }
}
