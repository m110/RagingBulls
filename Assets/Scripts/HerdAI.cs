using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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

    float movementSpeed = 7.0f;

    GameObject board;

	void Start () {
        board = GameObject.FindGameObjectWithTag("Board");
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

    public void HitWall(GameObject collider) {
        moving = false;

        int x = collider.GetComponent<Wall>().x;
        int y = collider.GetComponent<Wall>().y;
        int currentX, currentY;
        CoordsInDirection(x, y, OppositeDirection(direction), out currentX, out currentY);

        List<Directions> availableDirections = new List<Directions>();

        foreach (var dir in Enum.GetValues(typeof(Directions)).Cast<Directions>()) {
            if (dir == direction) {
                continue;
            }

            int wallX, wallY;
            CoordsInDirection(currentX, currentY, dir, out wallX, out wallY);

            try {
                if (board.GetComponent<BoardLogic>().GetWall(wallX, wallY) == null) {
                    availableDirections.Add(dir);
                }
            } catch (IndexOutOfRangeException) { 
                Debug.Log("Exception"); 
            }
        }

        if (availableDirections.Count == 0) {
            // Should never happen.
            return;
        }

        if (availableDirections.Count > 1) {
            availableDirections.Remove(OppositeDirection(direction));
        }

        var choice = UnityEngine.Random.Range(0, availableDirections.Count);
        var new_direction = availableDirections[choice];

        switch (new_direction) {
            case Directions.NORTH: MoveNorth(); break;
            case Directions.EAST: MoveEast(); break;
            case Directions.SOUTH: MoveSouth(); break;
            case Directions.WEST: MoveWest(); break;
        }
    }

    void CoordsInDirection(int x, int y, Directions dir, out int newX, out int newY) {
        newX = x;
        newY = y;

        switch (dir) {
            case Directions.NORTH: newY--; break;
            case Directions.EAST: newX++; break;
            case Directions.SOUTH: newY++; break;
            case Directions.WEST: newX--; break;
        }
    }

    Directions OppositeDirection(Directions dir) {
        switch (dir) {
            case Directions.NORTH: return Directions.SOUTH;
            case Directions.EAST: return Directions.WEST;
            case Directions.SOUTH: return Directions.NORTH;
            case Directions.WEST: return Directions.EAST;
            default: return Directions.NORTH;
        }
    }

    public void MoveNorth() {
        Move(Directions.NORTH, 0.0f);
    }

    public void MoveEast() {
        Move(Directions.EAST, 270.0f);
    }

    public void MoveSouth() {
        Move(Directions.SOUTH, 180.0f);
    }

    public void MoveWest() {
        Move(Directions.WEST, 90.0f);
    }

    void Move(Directions moveDirection, float rotation) {
        moving = true;
        direction = moveDirection;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
