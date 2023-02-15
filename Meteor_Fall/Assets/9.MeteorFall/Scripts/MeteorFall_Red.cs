using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_Red : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D SelfRigidbody2D;
    [SerializeField] private List<Sprite> MoveRedAnimationList = new List<Sprite>();
    [SerializeField] protected SpriteRenderer BodySpriteRenderer;
    [SerializeField] protected SpriteRenderer HeadSpriteRenderer;
    [SerializeField] private Sprite InitHead;
    [SerializeField] private Sprite InitBody;
    [SerializeField] protected Sprite DeadSprite;

    public MeteorFall_JoyStick JoyStick;
    protected float Speed = 3f;
    protected bool IsCanRunAnimation;
    private int IndexRedSpriteRenderer;
    private bool CheckCoroutine;
    protected float RotatioZ;
    protected bool IsDead = false;

    protected virtual void Start()
    {
        MeteorFall_GameManager.Instance.OnRedDie += OnRedDie;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    protected virtual void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnRedDie -= OnRedDie;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    protected virtual void ResetPosition()
    {
        transform.position = new Vector3(0, -3.5f, 0);
    }
    
    protected virtual void Update()
    {
        SelfRigidbody2D.velocity = JoyStick.NormalizeJoyStickDragVector * Speed;

        if (IsDead)
        {
            BodySpriteRenderer.sprite = DeadSprite;
            HeadSpriteRenderer.sprite = null;
            return;
        }

        if (JoyStick.NormalizeJoyStickDragVector != Vector2.zero)
        {
            RotatioZ = Mathf.Atan2(JoyStick.NormalizeJoyStickDragVector.x, JoyStick.NormalizeJoyStickDragVector.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -RotatioZ);
            IsCanRunAnimation = true;

            if (CheckCoroutine) return;

            StartCoroutine(MoveAnimation(0.1f));

            CheckCoroutine = true;
        }

        else
        {
            IsCanRunAnimation = false;
            CheckCoroutine = false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name.Contains("WarningZone"))
        {
            MeteorFall_GameManager.Instance.SetScore(true);

            Speed = 0;
            IsDead = true;
            IsCanRunAnimation = false;
        }
    }

    protected virtual void OnRedDie(bool isRedDie)
    {
        if (isRedDie)
        {
            StartCoroutine(Spawn(1f));
        }
    }

    protected IEnumerator Spawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ResetPosition();

        HeadSpriteRenderer.sprite = InitHead;
        BodySpriteRenderer.sprite = InitBody;
        IsDead = false;
        Speed = 3f;
    }

    protected IEnumerator MoveAnimation(float seconds)
    {
        while (IsCanRunAnimation)
        {
            yield return new WaitForSeconds(seconds);

            BodySpriteRenderer.sprite = MoveRedAnimationList[IndexRedSpriteRenderer];
            IndexRedSpriteRenderer++;

            if (IndexRedSpriteRenderer >= MoveRedAnimationList.Count)
            {
                IndexRedSpriteRenderer = 0;
            }
        }
    }

    protected virtual void OnEndGame()
    {
        Speed = 0;

        StopAllCoroutines();
    }
}
