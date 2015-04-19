using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum PlayerDirections {
    N, NE, E, SE, S, SW, W, NW
};

public class Player : MonoBehaviour {
    private float speed = 4.0f;

    public string horizontalAxis;
    public string verticalAxis;
    public string fireButton;

    Animator animator;

    bool ready = false;

    bool powerUpEquipped = false;
    PowerUps activePowerUp;

    PlayerDirections direction = PlayerDirections.N;
    GameObject lastField;

    BoardLogic board;
    GameLogic game;
    GameObject status;

    public void Init(int number) {
        horizontalAxis = "Horizontal" + number;
        verticalAxis = "Vertical" + number;
        fireButton = "Fire" + number;
        status = GameObject.FindGameObjectWithTag("PlayerStatus" + number);
        ready = true;
    }

    void Start() {
        animator = GetComponent<Animator>();
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardLogic>();
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<GameLogic>();
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

    void SetRotation(float horizontal, float vertical) {
        float angle = 0.0f;

        if (horizontal > 0.0f) {
            if (vertical > 0.0f) {
                direction = PlayerDirections.NE;
                angle = -45.0f;
            } else if (vertical < 0.0f) {
                direction = PlayerDirections.SE;
                angle = -135.0f;
            } else {
                direction = PlayerDirections.E;
                angle = -90.0f;
            }
        } else if (horizontal < 0.0f) {
            if (vertical > 0.0f) {
                direction = PlayerDirections.NW;
                angle = 45.0f;
            } else if (vertical < 0.0f) {
                direction = PlayerDirections.SW;
                angle = 135.0f;
            } else {
                direction = PlayerDirections.W;
                angle = 90.0f;
            }
        } else {
            if (vertical > 0.0f) {
                direction = PlayerDirections.N;
                angle = 0.0f;
            } else if (vertical < 0.0f) {
                direction = PlayerDirections.S;
                angle = 180.0f;
            }
        }

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
    
    void CoordsInDirection(int x, int y, PlayerDirections dir, out int newX, out int newY) {
        newX = x;
        newY = y;

        switch (dir) {
            case PlayerDirections.N: newY--; break;
            case PlayerDirections.NE: newX++;  newY--; break;
            case PlayerDirections.E: newX++; break;
            case PlayerDirections.SE: newX++;  newY++; break;
            case PlayerDirections.S: newY++; break;
            case PlayerDirections.SW: newX--;  newY++; break;
            case PlayerDirections.W: newX--; break;
            case PlayerDirections.NW: newX--; newY--; break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PowerUp") {
            var powerUp = collider.GetComponent<PowerUp>();
            if (!powerUpEquipped && powerUp.IsActive()) {
                powerUp.PickUp();
                AddPowerUp(powerUp.GetActivePowerUp());
            }
        } else if (collider.tag == "Field") {
            lastField = collider.gameObject;
        }
    }

    void AddPowerUp(PowerUps powerUp) {
        powerUpEquipped = true;
        activePowerUp = powerUp;
        status.GetComponent<Text>().text = PowerUp.powerUpNames[(int)powerUp];
    }

    void UsePowerUp() {
        if (!powerUpEquipped) {
            return;
        }

        switch (activePowerUp) {
            case PowerUps.BLOCK:
                int x, y;
                var field = lastField.GetComponent<Field>();
                CoordsInDirection(field.x, field.y, direction, out x, out y);

                var newField = board.GetField(x, y);
                if (newField == null || newField.GetComponent<Field>().HasVisitors()) {
                    return;
                }
                
                if (!board.AddWall(y, x, '!')) {
                    return;
                }

                break;
            case PowerUps.RAGING:
                game.SetRaging();
                break;
            default:
                return;
        }

        powerUpEquipped = false;
        status.GetComponent<Text>().text = "";
    }
}