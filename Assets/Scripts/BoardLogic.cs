﻿using UnityEngine;
using System;
using System.Collections;

public class BoardLogic : MonoBehaviour {

    public Transform solidWall;
    public Transform fragileWall;
    public Transform invisibleWall;
    public Transform powerUp;

    const int BOARD_HEIGHT = 14;
    const int BOARD_WIDTH = 21;

    const char NO_WALL = '_';
    const char SOLID_WALL = '#';
    const char FRAGILE_WALL = '!';
    const char INVISIBLE_WALL = '*';
    const char POWER_UP = 'P';

    Transform[,] walls;

    float wallSize;

	void Start () {
        var tmp = Instantiate(invisibleWall, Vector2.zero, Quaternion.identity) as Transform;
        wallSize = tmp.GetComponent<BoxCollider2D>().bounds.size.x;
        Destroy(tmp.gameObject);

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
        Transform prefab;
        bool wall = false;

        switch (symbol) {
            case NO_WALL:
                return;
            case SOLID_WALL:
                prefab = solidWall;
                wall = true;
                break;
            case FRAGILE_WALL:
                prefab = fragileWall;
                wall = true;
                break;
            case INVISIBLE_WALL:
                prefab = invisibleWall;
                wall = true;
                break;
            case POWER_UP:
                prefab = powerUp;
                break;
            default:
                return;
        }

        var obj = Instantiate(prefab, Vector2.zero, Quaternion.identity) as Transform;
        obj.parent = transform;

        var offsetX = BOARD_WIDTH * wallSize / 2.0f - wallSize * 0.5f;
        var offsetY = BOARD_HEIGHT * wallSize / 2.0f - wallSize * 0.5f;
        obj.localPosition = new Vector2(x * wallSize - offsetX, -y * wallSize + offsetY);

        if (wall) {
            obj.GetComponent<Wall>().SetPosition(x, y);
            walls[y, x] = obj;
        }
    }

    public Transform GetWall(int x, int y) {
        if (x < 1 || y < 1 || x >= BOARD_WIDTH - 1 || y >= BOARD_HEIGHT - 1){
            throw new IndexOutOfRangeException("Invalid wall coords");
        }

        return walls[y, x];
    }
}