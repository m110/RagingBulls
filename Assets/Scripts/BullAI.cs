using UnityEngine;
using System.Collections;

public class BullAI : MonoBehaviour {

    HerdAI herdAI;

	void Start () {
        herdAI = transform.parent.GetComponent<HerdAI>();
	}
	
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Wall") {
            herdAI.HitWall(collider.gameObject);
        }
    }
}
