using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

    public Transform herdPrefab;
    public Transform spawnPoint;

	void Start () {
        var herd = SpawnHerd(spawnPoint.position, Quaternion.identity);
        herd.GetComponent<HerdAI>().SpawnBull();
        herd.GetComponent<HerdAI>().MoveNorth();
	}
	
	void Update () {
	
	}

    Transform SpawnHerd(Vector3 position, Quaternion rotation) {
        var herd = Instantiate(herdPrefab, transform.position, transform.rotation) as Transform;

        herd.localPosition = position;
        herd.localRotation = rotation;
        herd.parent = transform;

        return herd;
    }
}
