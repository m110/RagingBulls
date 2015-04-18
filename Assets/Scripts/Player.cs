using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private float speed = 4.0f;

    public string horizontalAxis;
    public string verticalAxis;
    public string fireButton;

    Animator animator;

    bool powerUpEquipped = false;
    PowerUps activePowerUp;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetAxis(fireButton) > 0.0f) {
            UsePowerUp();
        }
   }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        if (moveHorizontal != 0.0f || moveVertical != 0.0f) {
            animator.SetBool("Moving", true);
        } else {
            animator.SetBool("Moving", false);
        }

        var rigidbody = GetComponent<Rigidbody2D>();
        var movement = new Vector2(moveHorizontal, moveVertical);
        rigidbody.velocity = movement * speed;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PowerUp") {
            var powerUp = collider.GetComponent<PowerUp>();
            if (!powerUpEquipped && powerUp.IsActive()) {
                powerUp.PickUp();
                AddPowerUp(powerUp.GetActivePowerUp());
            }
        }
    }

    void AddPowerUp(PowerUps powerUp) {
        powerUpEquipped = true;
        activePowerUp = powerUp;
    }

    void UsePowerUp() {
        powerUpEquipped = false;

        switch (activePowerUp) {
            case PowerUps.BLOCK:
                break;
        }
    }
}