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

            SetRotation(moveHorizontal, moveVertical);
        } else {
            animator.SetBool("Moving", false);
            rigidbody.velocity = Vector2.zero;
        }
    }

    void SetRotation(float moveHorizontal, float moveVertical) {
        float angleHorizontal = 0.0f, angleVertical = 0.0f, angle = 0.0f;

        if (moveHorizontal < 0.0f) {
            angleHorizontal = 90.0f;
        } else if (moveHorizontal > 0.0f) {
            angleHorizontal = -90.0f;
        }

        if (moveVertical < 0.0f) {
            angleVertical = 180.0f;
        } else if (moveVertical > 0.0f) {
            angleVertical = 0.0f;
        }

        if (moveVertical == 0.0f) {
            angle = angleHorizontal;
        } else if (moveHorizontal == 0.0f) {
            angle = angleVertical;
        } else {
            float sign = Mathf.Sign(angleHorizontal);

            angle = sign * ((Mathf.Abs(angleHorizontal) + angleVertical) / 2.0f);
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
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