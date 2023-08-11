using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private const int MaxPrefabCount = 5;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnSlimeStart;
    }

    private void SpawnSlimeStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnSlimeStart;
        NetworkObjectPool.Singleton.InitializePool();

        for (int i = 0; i < 5; ++i)
        {
            SpawnSlime();
        }
        StartCoroutine(SpawnOverTime());
    }

    private void SpawnSlime()
    {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(_prefab, GetRandomPositionOnMap(), Quaternion.identity);
        obj.GetComponent<SlimeController>().prefab = _prefab;
        if (!obj.IsSpawned) obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap()
    {
        return new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
    }

    private IEnumerator SpawnOverTime()
    {
        while (NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(6f);
            if (NetworkObjectPool.Singleton.GetCurrentPrefabCount(_prefab) < MaxPrefabCount)
            {
                SpawnSlime();
            }
        }
    }
}
