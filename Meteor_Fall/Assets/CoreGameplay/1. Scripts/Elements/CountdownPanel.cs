using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CountdownPanel: MonoBehaviour
{
    [SerializeField] private Image LoadingIcon;
    [SerializeField] private Image LoadingPanel;
    [SerializeField] private Image GreyPanel;
    [SerializeField] private TMP_Text CountdownText;

    private void Start()
    {
        LoadingPanel.DOFade(0, 0.25f).SetDelay(0.25f).OnComplete(() => { StartCoroutine(StartCountdown()); });
        /// TOAN TOAN TOAN
        /// SCALE + MOVE TRONG THỜI GIAN DELAY
        LoadingIcon.DOFade(0, 0.25f).SetDelay(0.25f);
    }

    private IEnumerator StartCountdown()
    {
        CountdownText.text = "3";
        yield return new WaitForSeconds(1);
        CountdownText.text = "2";
        yield return new WaitForSeconds(1);
        CountdownText.text = "1";
        yield return new WaitForSeconds(1);
        CountdownText.text = "";
        GreyPanel.DOFade(0, 0.25f);
        CustomEventManager.Instance.OnNewGame();
    }
}
