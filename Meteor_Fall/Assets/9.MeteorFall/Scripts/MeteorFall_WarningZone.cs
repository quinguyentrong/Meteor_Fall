using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorFall_WarningZone : MonoBehaviour
{
    [SerializeField] private CircleCollider2D WarningZoneCircleCollider2D;
    public GameObject Meteor;

    [SerializeField] private List<Sprite> DestroyMeteorAnimationList = new List<Sprite>();
    [SerializeField] private SpriteRenderer MeteroSpriteRenderer;

    private int IndexMeteroSpriterender;
    private bool IsCanRunAnimation;

    private void OnEnable()
    {
        IndexMeteroSpriterender = 1;
        IsCanRunAnimation = true;
        MeteroSpriteRenderer.sprite = DestroyMeteorAnimationList[0];
    }

    public void ActiveMeteor()
    {
        //Active thiên thạch sau 0.5s
        StartCoroutine(ActiveMeteor(0.5f));
        //Active animation thiên thạch nổ sau 1s
        StartCoroutine(Temp(1f));
        //Inactive thiên thạch sau 1.3s
        StartCoroutine(InActiveMeteor(1.3f));
    }

    public void InactiveWarningZone()
    {
        //Inactive Prefab WarningZone sau 1.3s(1 chu kỳ 1.3s)
        StartCoroutine(InactiveWarningZone(1.3f));
    }

    IEnumerator ActiveMeteor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Meteor.SetActive(true);
        //Thiên thạch di chuyển về tâm WarningZone trong 0.5s
        Meteor.transform.DOMove(gameObject.transform.position, 0.5f, false).SetEase(Ease.InSine);
    }

    IEnumerator InActiveMeteor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Meteor.SetActive(false);
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
