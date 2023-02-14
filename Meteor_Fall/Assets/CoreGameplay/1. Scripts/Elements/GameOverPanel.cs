using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverPanel: MonoBehaviour
{
    [SerializeField] private TMP_Text PanelText;
    [SerializeField] private Image PanelImage;
    [SerializeField] private Color RedColor, BlueColor;
    private void Start()
    {
        CustomEventManager.Instance.OnGameOver += ShowGameOver;
    }

    private void OnDestroy()
    {
        CustomEventManager.Instance.OnGameOver -= ShowGameOver;
    }

    private void ShowGameOver(bool isRedWin)
    {
        PanelImage.gameObject.SetActive(true);
        if (isRedWin)
        {
            PanelText.text = "RED WIN";
            PanelImage.color = RedColor;
        }
        else
        {
            PanelText.text = "BLUE WIN";
            PanelImage.color = BlueColor;
        }
        PanelImage.DOFade(0.66f, 0.25f);
        DOVirtual.DelayedCall(2, BackToMainHome);
    }

    private void BackToMainHome()
    {
        SceneManager.LoadSceneAsync(GameConfig.HOME_SCENE);
    }
}
