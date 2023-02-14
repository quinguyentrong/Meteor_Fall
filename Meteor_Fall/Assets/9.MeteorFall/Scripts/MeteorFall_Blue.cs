using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_Blue : MonoBehaviour
{
    [SerializeField] private MeteorFall_Red Red;

    [SerializeField] private Rigidbody2D SelfRigidbody2D;
    [SerializeField] private List<Sprite> MoveBlueAnimationList = new List<Sprite>();
    [SerializeField] private SpriteRenderer BlueSpriteRenderer;

    public MeteorFall_BlueJoyStick JoyStick;

    private float Speed = 3f;
    private bool IsBotCanMove = false;
    private int IndexBlueSpriteRenderer;
    private float RotatioZ;
    private bool CheckCoroutine;
    private Vector2 TargetDirection;
    private bool IsCanRunAnimation;

    private void Start()
    {
        MeteorFall_GameManager.Instance.OnBotMove += StartGame;
        MeteorFall_GameManager.Instance.OnRedDie += OnBlueDie;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnBotMove -= StartGame;
        MeteorFall_GameManager.Instance.OnRedDie -= OnBlueDie;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    private void Update()
    {
        if (GameConfig.IsPvPMode)
        {
            SelfRigidbody2D.velocity = JoyStick.NormalizeJoyStickDragVector * Speed;

            //Nếu di chuyển joystick
            if (JoyStick.NormalizeJoyStickDragVector != Vector2.zero)
            {
                //Sét góc cho player hướng theo hướng joystick
                RotatioZ = Mathf.Atan2(JoyStick.NormalizeJoyStickDragVector.x, JoyStick.NormalizeJoyStickDragVector.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, -RotatioZ);

                IsCanRunAnimation = true;
                if (CheckCoroutine) return;

                StartCoroutine(MoveBlueAnimation(0.1f));
                CheckCoroutine = true;
            }
            else
            {
                IsCanRunAnimation = false;
                CheckCoroutine = false;
            }
        }
        else
        {
            if (IsBotCanMove == false) return;

            TargetDirection = new Vector2(Red.transform.position.x - transform.position.x, Red.transform.position.y - transform.position.y);

            SelfRigidbody2D.velocity = new Vector2(TargetDirection.x, TargetDirection.y).normalized * Speed;
            RotatioZ = Mathf.Atan2(TargetDirection.x, TargetDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -RotatioZ);
            IsCanRunAnimation = true;
        }
    }

    private void StartGame()
    {
        IsBotCanMove = true;
        StartCoroutine(MoveBlueAnimation(0.1f));
    }

    IEnumerator MoveBlueAnimation(float seconds)
    {
        while (IsCanRunAnimation)
        {
            yield return new WaitForSeconds(seconds);
            BlueSpriteRenderer.sprite = MoveBlueAnimationList[IndexBlueSpriteRenderer];
            IndexBlueSpriteRenderer++;

            if (IndexBlueSpriteRenderer >= MoveBlueAnimationList.Count)
            {
                IndexBlueSpriteRenderer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Contains("WarningZone"))
        {
            MeteorFall_GameManager.Instance.SetScore(false);
            Speed = 0;
        }
    }

    private void OnBlueDie(bool isRedDie)
    {
        if (isRedDie == false)
        {
            StartCoroutine(SpawnBlue(1f));
        }
    }

    IEnumerator SpawnBlue(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetPosition();
        Speed = 3f;
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0, 3.5f, 0);
    }

    private void OnEndGame()
    {
        IsBotCanMove = false;
        Speed = 0;
        StopAllCoroutines();
    }
}
