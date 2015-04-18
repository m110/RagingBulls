using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private float speed = 4.0f;

    public string horizontalAxis;
    public string verticalAxis;
    public string fireButton;

    void Start() {
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        var rigidbody = GetComponent<Rigidbody2D>();
        var movement = new Vector2(moveHorizontal, moveVertical);
        rigidbody.velocity = movement * speed;
    }
}
