using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteorFall_RedJoyStick : MeteorFall_JoyStick
{
    protected override void Start()
    {
        base.Start();

        MeteorFall_GameManager.Instance.OnJoyStick += OnJoyStick;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnJoyStick -= OnJoyStick;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    protected override bool CheckValidPosition()
    {
        return (Input.mousePosition.y < (float)Screen.height / 2);
    }
}
