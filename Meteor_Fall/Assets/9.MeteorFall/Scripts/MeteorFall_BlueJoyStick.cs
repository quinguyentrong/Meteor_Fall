using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_BlueJoyStick : MeteorFall_JoyStick
{
    protected override void Start()
    {
        base.Start();

        if (GameConfig.IsPVPMode)
        {
            MeteorFall_GameManager.Instance.OnJoyStick += OnJoyStick;
            MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
        }
    }

    private void OnDestroy()
    {
        if (GameConfig.IsPVPMode)
        {
            MeteorFall_GameManager.Instance.OnJoyStick -= OnJoyStick;
            MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
        }
    }

    protected override bool CheckValidPosition()
    {
        return (Input.mousePosition.y > (float)Screen.height / 2);
    }
}
