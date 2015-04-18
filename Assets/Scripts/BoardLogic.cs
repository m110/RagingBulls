using UnityEngine;
using System;
using System.Collections;

public class BoardLogic : MonoBehaviour {

    public Transform solidWall;
    public Transform fragileWall;
    public Transform invisibleWall;

    const int BOARD_HEIGHT = 14;
    const int BOARD_WIDTH = 21;

    const char NO_WALL = '_';
    const char SOLID_WALL = '#';
    const char FRAGILE_WALL = '!';
    const char INVISIBLE_WALL = '*';

    Transform[,] walls;

	void Start () {
        LoadLevel(1);
	}
	
	void Update () {
	
	}

    void LoadLevel(int level) {
        walls = new Transform[BOARD_HEIGHT, BOARD_WIDTH];

        string levelPath = String.Format("Levels/level{0}.dat", level);

        var lines = System.IO.File.ReadAllLines(levelPath);
        for (int i = 0; i < lines.Length; i++) {
            for (int j = 0; j < lines[i].Length; j++) {
                AddWall(i, j, lines[i][j]);
            }
        }
    }

    void AddWall(int y, int x, char symbol) {
        Transform wallPrefab;
        switch (symbol) {
            case NO_WALL:
                return;
            case SOLID_WALL:
                wallPrefab = solidWall;
                break;
            case FRAGILE_WALL:
                wallPrefab = fragileWall;
                break;
            case INVISIBLE_WALL:
                wallPrefab = invisibleWall;
                break;
            default:
                return;
        }

        var wall = Instantiate(wallPrefab, Vector2.zero, Quaternion.identity) as Transform;
        wall.parent = transform;

        var size = wall.GetComponent<BoxCollider2D>().bounds.size.x;
        var offsetX = BOARD_WIDTH * size / 2.0f - size * 0.5f;
        var offsetY = BOARD_HEIGHT * size / 2.0f - size * 0.5f;
        wall.localPosition = new Vector2(x * size - offsetX, -y * size + offsetY);

        wall.GetComponent<Wall>().SetPosition(x, y);

        walls[y, x] = wall;
    }

    public Transform GetWall(int x, int y) {
        if (x < 1 || y < 1 || x >= BOARD_WIDTH - 1 || y >= BOARD_HEIGHT - 1){
            throw new IndexOutOfRangeException("Invalid wall coords");
        }

        return walls[y, x];
    }
}