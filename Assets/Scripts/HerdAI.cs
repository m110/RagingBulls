using UnityEngine;
using System.Collections;

public class HerdAI : MonoBehaviour {

    public Transform bullPrefab;

    bool moving = false;
    Vector3 direction;

    float movementSpeed = 4.0f;

	void Start () {
	
	}
	
	void Update () {
        if (!moving) {
            return;
        }

        transform.position = Vector3.Lerp(transform.position,
                                          transform.position + direction * movementSpeed,
                                          Time.deltaTime);

        var bulls = GetComponentsInChildren<BullAI>();
        if (bulls.Length == 0) {
            Destroy(gameObject);
        }
	}

    public void SpawnBull() {
        var bull = Instantiate(bullPrefab, transform.position, transform.rotation) as Transform;
        AddBull(bull);
    }

    public void AddBull(Transform bull) {
        bull.transform.parent = transform;
    }

    public void HitWall() {
        moving = false;
    }

    public void MoveNorth() {
        Move(transform.up);
    }

    public void MoveEast() {
        Move(transform.right);
    }

    public void MoveSouth() {
        Move(-transform.up);
    }

    public void MoveWest() {
        Move(-transform.right);
    }

    void Move(Vector3 moveDirection) {
        moving = true;
        direction = moveDirection;
    }
}
