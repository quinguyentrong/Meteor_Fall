using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_Blue : MeteorFall_Red
{
    [SerializeField] private MeteorFall_Red Red;

    private bool IsBotCanMove = false;
    private Vector2 TargetDirection;

    protected override void Start()
    {
        if (GameConfig.IsPVPMode == false)
        {
            MeteorFall_GameManager.Instance.OnBotMove += StartGame;
        }
        
        base.Start();
    }

    protected override void OnDestroy()
    {
        if (GameConfig.IsPVPMode == false)
        {
            MeteorFall_GameManager.Instance.OnBotMove -= StartGame;
        }

        base.OnDestroy();   
    }

    protected override void Update()
    {
        if (GameConfig.IsPVPMode)
        {
            base.Update();
        }
        else
        {
            if (IsDead)
            {
                BodySpriteRenderer.sprite = DeadSprite;
                HeadSpriteRenderer.sprite = null;
            }

            if (IsBotCanMove == false) return;

            TargetDirection = new Vector2(Red.transform.position.x - transform.position.x, Red.transform.position.y - transform.position.y);
            SelfRigidbody2D.velocity = new Vector2(TargetDirection.x, TargetDirection.y).normalized * Speed;
            RotatioZ = Mathf.Atan2(TargetDirection.x, TargetDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -RotatioZ);
        }
    }

    protected override void ResetPosition()
    {
        transform.position = new Vector3(0, 3.5f, 0);
        transform.localEulerAngles = new Vector3(0, 0, 180f);

        if (GameConfig.IsPVPMode == false)
        {
            IsCanRunAnimation = true;

            StartCoroutine(MoveAnimation(0.1f));
        }
    }

    protected override void OnRedDie(bool isRedDie)
    {
        if (isRedDie == false)
        {
            StartCoroutine(Spawn(1f));
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Contains("WarningZone"))
        {
            MeteorFall_GameManager.Instance.SetScore(false);

            Speed = 0;
            IsCanRunAnimation = false;
            IsDead = true;
        }
    }

    private void StartGame()
    {
        IsBotCanMove = true;
        IsCanRunAnimation = true;

        StartCoroutine(MoveAnimation(0.1f));
    }

    protected override void OnEndGame()
    {
        if (GameConfig.IsPVPMode == false)
        {
            IsBotCanMove = false;
        }

        base.OnEndGame();
    }
}
