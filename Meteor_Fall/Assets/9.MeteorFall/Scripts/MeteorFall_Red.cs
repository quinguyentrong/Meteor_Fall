using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_Red : MonoBehaviour
{
    [SerializeField] private Rigidbody2D SelfRigidbody2D;

    [SerializeField] private List<Sprite> MoveRedAnimationList = new List<Sprite>();
    [SerializeField] private SpriteRenderer RedSpriteRenderer;

    public MeteorFall_RedJoyStick JoyStick;
    private float Speed = 3f;
    private bool IsCanRunAnimation;
    private int IndexRedSpriteRenderer;
    private bool CheckCoroutine;
    private float RotatioZ;

    private void Start()
    {
        MeteorFall_GameManager.Instance.OnRedDie += OnRedDie;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnRedDie -= OnRedDie;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(0, -3.5f, 0);
    }
    
    private void Update()
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

            StartCoroutine(MoveRedAnimation(0.1f));
            CheckCoroutine = true;
        }
        else
        {
            IsCanRunAnimation = false;
            CheckCoroutine = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Contains("WarningZone"))
        {
            MeteorFall_GameManager.Instance.SetScore(true);
            //Cho Player đứng yên khi chết
            Speed = 0;
        }
    }

    private void OnRedDie(bool isRedDie)
    {
        if (isRedDie)
        {
            StartCoroutine(SpawnRed(1f));
        }
    }

    IEnumerator SpawnRed(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetPosition();
        Speed = 3f;
    }

    IEnumerator MoveRedAnimation(float seconds)
    {
        while (IsCanRunAnimation)
        {
            yield return new WaitForSeconds(seconds);
            RedSpriteRenderer.sprite = MoveRedAnimationList[IndexRedSpriteRenderer];
            IndexRedSpriteRenderer++;

            if (IndexRedSpriteRenderer >= MoveRedAnimationList.Count)
            {
                IndexRedSpriteRenderer = 0;
            }
        }
    }

    private void OnEndGame()
    {
        Speed = 0;
        StopAllCoroutines();
    }
}
