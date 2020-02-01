using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    LayerMask platformLayer;

    Rigidbody2D rigid;
    BoxCollider2D col;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        Physics2D.gravity = Vector3.right * 9.8f;
    }

    void Update() 
    {
        float direction = Input.GetAxisRaw("Horizontal");
        //bool grounded = IsCollided(Vector2.down);
        bool grounded = IsCollided(Vector2.right);
        if (direction > 0) {
            //bool hitRightWall = IsCollided(Vector2.right);
            bool hitRightWall = IsCollided(Vector2.up);
            if (hitRightWall) {
                direction = 0;
            }
        } else {
            //bool hitLeftWall = IsCollided(Vector2.left);
            bool hitLeftWall = IsCollided(Vector2.down);
            if (hitLeftWall) {
                direction = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            //rigid.velocity = new Vector2(direction * speed, jumpForce);
            rigid.velocity = new Vector2(-jumpForce, direction * speed);
        } else {          
            //rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
            rigid.velocity = new Vector2(rigid.velocity.x, direction * speed);
        }
    }

    bool IsCollided(Vector2 direction) {
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, direction, 0.01f, platformLayer);
        return raycast.collider != null;
    }
}
