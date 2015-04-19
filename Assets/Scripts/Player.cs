using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private float speed = 4.0f;

    public string horizontalAxis;
    public string verticalAxis;
    public string fireButton;

    Animator animator;

    bool ready = false;

    bool powerUpEquipped = false;
    PowerUps activePowerUp;

    public void Init(int number) {
        horizontalAxis = "Horizontal" + number;
        verticalAxis = "Vertical" + number;
        fireButton = "Fire" + number;
        ready = true;
    }

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!ready) {
            return;
        }

        if (Input.GetAxis(fireButton) > 0.0f) {
            UsePowerUp();
        }
   }

    void FixedUpdate() {
        if (!ready) {
            return;
        }

        float moveHorizontal = Input.GetAxis(horizontalAxis);
        float moveVertical = Input.GetAxis(verticalAxis);

        var rigidbody = GetComponent<Rigidbody2D>();

        if (moveHorizontal != 0.0f || moveVertical != 0.0f) {
            animator.SetBool("Moving", true);

            var movement = new Vector2(moveHorizontal, moveVertical);
            rigidbody.velocity = movement * speed;
        } else {
            animator.SetBool("Moving", false);
            rigidbody.velocity = Vector2.zero;
        }
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