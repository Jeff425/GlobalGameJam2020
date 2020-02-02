using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    LayerMask platformLayer;
    [SerializeField]
    float swapTimeSeconds;
    [SerializeField]
    float flipTimeSeconds;

    Rigidbody2D rigid;
    BoxCollider2D col;
    Direction gravityDirection;
    float startTime;
    float lastTranstion;
    float oldGravityMagnitude;
    float startRotation;
    IEnumerator delayedPhysics;
    Vector2 startPosition;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        gravityDirection = Direction.down;
        lastTranstion = -swapTimeSeconds - 1;
        delayedPhysics = null;
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        CheckGravity();
        float direction = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("horz", Mathf.Abs(direction));
        float currentTime = Time.time;
        bool noControl = currentTime - swapTimeSeconds < lastTranstion;
        float oldGravity = 0f;
        if (noControl) {
            float percent = (currentTime - lastTranstion) / swapTimeSeconds;        
            oldGravity = Mathf.Lerp(oldGravityMagnitude, 0, percent);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, startRotation), 
                Quaternion.Euler(0, 0, DirectionToPlayerRotation(gravityDirection)), percent);
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
        
        if (GameVariables.IsVertical(gravityDirection)) {
            //animator.SetFloat("horz", Mathf.Abs(rigid.velocity.x));
            animator.SetFloat("vert", Mathf.Abs(rigid.velocity.y));
            animator.SetBool("falling", gravityDirection == Direction.down ? rigid.velocity.y < 0f : rigid.velocity.y > 0f);
            Vector2 scale = transform.localScale;
            if (gravityDirection == Direction.down ? rigid.velocity.x < 0f : rigid.velocity.x > 0f) {
                scale.x = -1;               
            } else if (gravityDirection == Direction.down ? rigid.velocity.x > 0f : rigid.velocity.x < 0f){
                scale.x = 1;
            }
            transform.localScale = scale;
        } else {
            //animator.SetFloat("horz", Mathf.Abs(rigid.velocity.y));
            animator.SetFloat("vert", Mathf.Abs(rigid.velocity.x));
            animator.SetBool("falling", gravityDirection == Direction.right ? rigid.velocity.x > 0f : rigid.velocity.x < 0f);
            animator.SetBool("left", gravityDirection == Direction.right ? rigid.velocity.y < 0f : rigid.velocity.y > 0f);
            Vector2 scale = transform.localScale;
            if (gravityDirection == Direction.right ? rigid.velocity.y < 0f : rigid.velocity.y > 0f) {
                scale.x = -1;               
            } else if (gravityDirection == Direction.right ? rigid.velocity.y > 0f : rigid.velocity.y < 0f){
                scale.x = 1;
            }
            transform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Button")) {
            col.GetComponent<ButtonController>().Trigger();
        } else if (col.gameObject.layer == LayerMask.NameToLayer("Fire")) {
            GameController.Instance.Reset();
            transform.position = startPosition;
            rigid.velocity = Vector2.zero;
        }
    }

    void CheckGravity() {
        Direction tmpDirection = GameController.Instance.GetDirectionFromPoint(transform.position);
        if (tmpDirection == Direction.notset || tmpDirection == gravityDirection) {
            return;
        }
        if (delayedPhysics != null) {
            StopCoroutine(delayedPhysics);
            delayedPhysics = null;
        }
        if (GameVariables.IsVertical(gravityDirection) != GameVariables.IsVertical(tmpDirection)) {
                oldGravityMagnitude = GameVariables.IsVertical(gravityDirection) ? rigid.velocity.y : rigid.velocity.x;
        } else {
            // Technically movement magnitude
            oldGravityMagnitude = GameVariables.IsVertical(gravityDirection) ? rigid.velocity.x : rigid.velocity.y;
        }
        startRotation = transform.eulerAngles.z;
        lastTranstion = Time.time;
        Direction oldDirection = gravityDirection;
        gravityDirection = tmpDirection;
        Vector2 newGrav = Vector2.zero;
        switch (gravityDirection) {
            case Direction.down:
                newGrav = Vector2.down * 9.8f;
                break;
            case Direction.left:
                newGrav = Vector2.left * 9.8f;
                break;
            case Direction.right:
                newGrav = Vector2.right * 9.8f;
                break;
            case Direction.up:
                newGrav = Vector2.up * 9.8f;
                break;
        }
        if (GameVariables.IsVertical(gravityDirection) == GameVariables.IsVertical(oldDirection)) {
            delayedPhysics = WaitAndChangeGravity(newGrav);
            StartCoroutine(delayedPhysics);
        } else {
            Physics2D.gravity = newGrav;
        }
    }

    IEnumerator WaitAndChangeGravity(Vector2 newGrav) {
        yield return new WaitForSeconds(flipTimeSeconds);
        Physics2D.gravity = newGrav;
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

    float DirectionToPlayerRotation(Direction direction) {
        switch (direction) {
            case Direction.up:
                return 180f;
            case Direction.right:
                return 90f;
            case Direction.left:
                return 270f;
        }
        return 0f;
    }
}
