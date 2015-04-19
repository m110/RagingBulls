using UnityEngine;
using System.Collections;

public enum PowerUps {
    BLOCK,
};

public class PowerUp : MonoBehaviour {

    bool active = false;
    PowerUps powerUp;

	void Start () {
        PickUp();
	}
	
	void Update () {
	
	}
    
    public void SetReady() {
        if (active) {
            return;
        }

        active = true;

        System.Array values = System.Enum.GetValues(typeof(PowerUps));
        powerUp = (PowerUps)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void PickUp() {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public bool IsActive() {
        return active;
    }

    public PowerUps GetActivePowerUp() {
        return powerUp;
    }
}