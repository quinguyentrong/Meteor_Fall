using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorFall_WarningZone : MonoBehaviour
{
    [SerializeField] private CircleCollider2D WarningZoneCircleCollider2D;
    [SerializeField] private GameObject Meteor;
    [SerializeField] private GameObject MeteorShadow;
    [SerializeField] private List<Sprite> DestroyMeteorAnimationList = new List<Sprite>();
    [SerializeField] private SpriteRenderer MeteroSpriteRenderer;

    private int IndexMeteroSpriterender;
    private bool IsCanRunAnimation;

    private void OnEnable()
    {
        IndexMeteroSpriterender = 1;
        IsCanRunAnimation = true;
        MeteroSpriteRenderer.sprite = DestroyMeteorAnimationList[0];
        Meteor.transform.localPosition = new Vector3(2, 2, 0);
        MeteorShadow.transform.localPosition = new Vector3(2, 0, 0);
        MeteorShadow.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
    }

    public void ActiveMeteor()
    {
        StartCoroutine(ActiveMeteor(0.5f));
        StartCoroutine(Temp(1f));
        StartCoroutine(InActiveMeteor(1.3f));
    }

    public void InactiveWarningZone()
    {
        StartCoroutine(InactiveWarningZone(1.3f));
    }

    IEnumerator ActiveMeteor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Meteor.SetActive(true);
        MeteorShadow.SetActive(true);
        Meteor.transform.DOMove(gameObject.transform.position, 0.5f, false).SetEase(Ease.InSine);
        MeteorShadow.transform.DOMoveX(gameObject.transform.position.x, 0.5f, false).SetEase(Ease.InSine);
        MeteorShadow.transform.DOScale(1f, 0.5f);
    }

    IEnumerator InActiveMeteor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Meteor.SetActive(false);
        MeteorShadow.SetActive(false);
    }

    IEnumerator InactiveWarningZone(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        WarningZoneCircleCollider2D.enabled = false;

        PoolingSystem.Despawn(gameObject);
    }

    IEnumerator DestroyMeteorAnimation(float seconds)
    {
        while (IsCanRunAnimation)
        {
            yield return new WaitForSeconds(seconds);

            MeteroSpriteRenderer.sprite = DestroyMeteorAnimationList[IndexMeteroSpriterender];
            IndexMeteroSpriterender++;

            if (IndexMeteroSpriterender >= DestroyMeteorAnimationList.Count)
            {
                IsCanRunAnimation = false;
            }
        }
    }

    IEnumerator Temp(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        IsCanRunAnimation = true;
        WarningZoneCircleCollider2D.enabled = true;

        StartCoroutine(DestroyMeteorAnimation(0.1f));
    }
}
