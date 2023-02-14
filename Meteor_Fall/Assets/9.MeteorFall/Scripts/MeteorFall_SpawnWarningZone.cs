using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall_SpawnWarningZone : MonoBehaviour
{
    [SerializeField] private MeteorFall_WarningZone WarningZone;

    private void Start()
    {
        MeteorFall_GameManager.Instance.OnSpawnWarningZone += SpawnWarningZone;
        MeteorFall_GameManager.Instance.OnRedDie += DespawnWarningZone;
        MeteorFall_GameManager.Instance.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        MeteorFall_GameManager.Instance.OnSpawnWarningZone -= SpawnWarningZone;
        MeteorFall_GameManager.Instance.OnRedDie -= DespawnWarningZone;
        MeteorFall_GameManager.Instance.OnEndGame -= OnEndGame;
    }

    private void SpawnGameObject()
    {
        var warningZone = PoolingSystem.Spawn(WarningZone);
        warningZone.transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-4f, 4f), 0);
        warningZone.ActiveMeteor();
        warningZone.InactiveWarningZone();
    }

    private void SpawnWarningZone()
    {
        StartCoroutine(SpawnGameObj(2f));
    }

    IEnumerator SpawnGameObj(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            SpawnGameObject();
        }
    }

    private void DespawnWarningZone(bool isRedDie)
    {
        StopAllCoroutines();
    }

    private void OnEndGame()
    {
        StopAllCoroutines();
    }
}
