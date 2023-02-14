using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton: MonoBehaviour
{
    [SerializeField] private Button SelfButton;
    private void Start()
    {
        SelfButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        SceneManager.LoadSceneAsync(GameConfig.HOME_SCENE);
    }
}
