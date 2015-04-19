using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour {

    public Sprite idleSprite;
    public Sprite activeSprite;

    bool visitors = true;

    public int x, y;

	void Start () {
	}

    void Update() {
        visitors = false;
    }
	
    void OnTriggerStay2D(Collider2D collider) {
        visitors = true;
    }

    public bool HasVisitors() {
        return visitors;
    }

	public void SetPosition (int newX, int newY) {
        x = newX;
        y = newY;
	}
}
