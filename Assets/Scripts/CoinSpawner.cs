using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {
    public GameObject[] coins;

    public float XMin, XMax;
    public float ZMin, ZMax;

    Vector3 spawnPosition;

    private void Start() {
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
            if (GameManager.instance.playerList.Count > 1)
                Instantiate(coins[0], RandomSpawnPosition(), transform.rotation);
        }
    }

    Vector3 RandomSpawnPosition() {
        float randomX = Random.Range(XMin, XMax);
        float randomZ = Random.Range(ZMin, ZMax);
        return new Vector3(randomX, .5f, randomZ);
    }
}
