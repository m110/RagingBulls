using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public int x, y;

	public void SetPosition (int newX, int newY) {
        x = newX;
        y = newY;
	}
}
