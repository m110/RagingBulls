using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public int x, y;
    public bool breakable;

    BoardLogic board;

    void Start() {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardLogic>();
    }

	public void SetPosition (int newX, int newY) {
        x = newX;
        y = newY;
	}

    public void Break() {
        if (breakable) {
            board.BreakWall(x, y);
        }
    }
}
