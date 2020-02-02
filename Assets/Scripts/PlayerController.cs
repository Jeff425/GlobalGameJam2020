using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    GameController gameController;
    [SerializeField]
    float speed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    LayerMask platformLayer;
    [SerializeField]
    float swapTimeSeconds;

    Rigidbody2D rigid;
    BoxCollider2D col;
    Direction gravityDirection;
    float startTime;
    float lastTranstion;
    Direction lastDirection;
    float oldGravityMagnitude;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        gravityDirection = Direction.down;
        lastTranstion = -swapTimeSeconds - 1;
        lastDirection = Direction.notset;
    }

    void Update() 
    {
        CheckGravity();
        float direction = Input.GetAxisRaw("Horizontal");
        float currentTime = Time.time;
        bool noControl = currentTime - swapTimeSeconds < lastTranstion;
        float oldGravity = 0f;
        if (noControl && lastDirection != Direction.notset) {
            float percent = (currentTime - lastTranstion) / swapTimeSeconds;
            oldGravity = Mathf.Lerp(oldGravityMagnitude, 0, percent);
        }
        Vector2 down = new Vector2();
        Vector2 right = new Vector2();
        Vector2 move = new Vector2();
        Vector2 jump = new Vector2();
        float moveSpeed;
        switch (gravityDirection) {
            case Direction.down:
                down = Vector2.down;
                right = Vector2.right;
                direction = ClampedDirection(direction, down, right);
                moveSpeed = noControl ? oldGravity : direction * speed;
                jump = new Vector2(moveSpeed, jumpForce);
                move = new Vector2(moveSpeed, rigid.velocity.y);
                break;
            case Direction.up:
                down = Vector2.up;
                right = Vector2.left;
                direction = ClampedDirection(direction, down, right);
                moveSpeed = noControl ? oldGravity : -direction * speed;
                jump = new Vector2(moveSpeed, -jumpForce);
                move = new Vector2(moveSpeed, rigid.velocity.y);
                break;
            case Direction.right:
                down = Vector2.right;
                right = Vector2.up;
                direction = ClampedDirection(direction, down, right);
                moveSpeed = noControl ? oldGravity : direction * speed;
                jump = new Vector2(-jumpForce, moveSpeed);
                move = new Vector2(rigid.velocity.x, moveSpeed);
                break;
            case Direction.left:
                down = Vector2.left;
                right = Vector2.down;
                direction = ClampedDirection(direction, down, right);
                moveSpeed = noControl ? oldGravity : -direction * speed;
                jump = new Vector2(jumpForce, moveSpeed);
                move = new Vector2(rigid.velocity.x, moveSpeed);
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && IsCollided(down)) {
            rigid.velocity = jump;
        } else {          
            rigid.velocity = move;
        }
    }

    void CheckGravity() {
        Direction tmpDirection = gameController.GetDirectionFromPoint(transform.position);
        if (tmpDirection == Direction.notset || tmpDirection == gravityDirection) {
            return;
        }
        if (((gravityDirection == Direction.up || gravityDirection == Direction.down) && tmpDirection != Direction.up && tmpDirection != Direction.down) ||
            ((gravityDirection == Direction.left || gravityDirection == Direction.right) && tmpDirection != Direction.left && tmpDirection != Direction.right)) {
                oldGravityMagnitude = gravityDirection == Direction.up || gravityDirection == Direction.down ? rigid.velocity.y : rigid.velocity.x;
                lastDirection = gravityDirection;
        } else {
            lastDirection = Direction.notset;
        }
        lastDirection = gravityDirection;
        lastTranstion = Time.time;
        gravityDirection = tmpDirection;
        switch (gravityDirection) {
            case Direction.down:
                Physics2D.gravity = Vector2.down * 9.8f;
                break;
            case Direction.left:
                Physics2D.gravity = Vector2.left * 9.8f;
                break;
            case Direction.right:
                Physics2D.gravity = Vector2.right * 9.8f;
                break;
            case Direction.up:
                Physics2D.gravity = Vector2.up * 9.8f;
                break;
        }
        
    }

    bool IsCollided(Vector2 direction) {
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, direction, 0.01f, platformLayer);
        return raycast.collider != null;
    }

    float ClampedDirection(float direction, Vector2 down, Vector2 right) {
        bool grounded = IsCollided(down);
        if (direction > 0) {
            bool hitRightWall = IsCollided(right);
            if (hitRightWall) {
                return 0;
            }
        } else {
            bool hitLeftWall = IsCollided(-right);
            if (hitLeftWall) {
                return 0;
            }
        }
        return direction;
    }
}
