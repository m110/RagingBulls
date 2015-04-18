using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

    public Transform herdPrefab;
    public Transform playerPrefab;

    public Transform spawnPoint;
    public Transform[] playerSpawns;

    private int totalHerds = 5;
    private int spawnedHerds = 0;

    private float spawnTimer = 0.0f;
    private float spawnInterval = 1.0f;

    private int increaseTimerAfter = 3;
    private float increaseTimerBy = 4.0f;

    private float powerupTimer = 0.0f;
    private float powerupInterval = 5.0f;

    private Transform[] players;

	void Start () {
        players = new Transform[2];
        spawnTimer = spawnInterval;
        SpawnPlayers();
	}

	void Update () {
        if (spawnedHerds < totalHerds) {
            if (spawnTimer <= 0) {
                SpawnHerd();
                spawnedHerds++;

                if (spawnedHerds == increaseTimerAfter) {
                    spawnTimer = spawnInterval + increaseTimerBy;
                } else {
                    spawnTimer = spawnInterval;
                }
            } else {
                spawnTimer -= Time.deltaTime;
            }
        }

        if (powerupTimer <= 0) {
            var powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            if (powerUps.Length > 0) {
                var powerUp = powerUps[Random.Range(0, powerUps.Length)];
                powerUp.GetComponent<PowerUp>().SetReady();

                powerupTimer = powerupInterval;
            }
        } else {
            powerupTimer -= Time.deltaTime;
        }
	}

    void SpawnPlayers() {
        for (int i = 0; i < 2; i++) {
            players[i] = Instantiate(playerPrefab, playerSpawns[i].position, Quaternion.identity) as Transform;
            players[i].GetComponent<Animator>().SetInteger("Player", i + 1);
            players[i].GetComponent<Player>().horizontalAxis = "Horizontal" + (i + 1);
            players[i].GetComponent<Player>().verticalAxis = "Vertical" + (i + 1);
            players[i].GetComponent<Player>().fireButton = "Fire" + (i + 1);
        }
    }

    Transform SpawnHerd() {
        var herd = Instantiate(herdPrefab, Vector3.zero, Quaternion.identity) as Transform;

        herd.localPosition = spawnPoint.position;
        herd.parent = transform;

        herd.GetComponent<HerdAI>().SpawnBull();
        herd.GetComponent<HerdAI>().MoveNorth();

        return herd;
    }
}
