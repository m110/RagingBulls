using UnityEngine;
using System.Collections;

enum Directions {
    NORTH,
    EAST,
    SOUTH,
    WEST,
};

public class HerdAI : MonoBehaviour {

    public Transform bullPrefab;

    bool moving = false;
    Directions direction;

    float movementSpeed = 6.0f;

	void Start () {
	
	}
	
	void Update () {
        if (!moving) {
            return;
        }

        transform.position = Vector3.Lerp(transform.position,
                                          transform.position + transform.up * movementSpeed,
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

        var choice = Random.Range(0, 2);

        Directions new_direction;
        if (direction == Directions.NORTH || direction == Directions.SOUTH) {
            new_direction = choice == 0 ? Directions.EAST : Directions.WEST;
        } else {
            new_direction = choice == 0 ? Directions.NORTH : Directions.SOUTH;
        }

        switch (new_direction) {
            case Directions.NORTH: MoveNorth(); break;
            case Directions.EAST: MoveEast(); break;
            case Directions.SOUTH: MoveSouth(); break;
            case Directions.WEST: MoveWest(); break;
        }
    }

    public void MoveNorth() {
        Move(Directions.NORTH, 0.0f);
    }

    public void MoveEast() {
        Move(Directions.EAST, 90.0f);
    }

    public void MoveSouth() {
        Move(Directions.SOUTH, 180.0f);
    }

    public void MoveWest() {
        Move(Directions.WEST, 270.0f);
    }

    void Move(Directions moveDirection, float rotation) {
        moving = true;
        direction = moveDirection;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
