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
        //Physics2D.gravity = new Vector2(1f, 0);
    }

    void Update() 
    {
        float direction = Input.GetAxisRaw("Horizontal");
        bool grounded = IsGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rigid.velocity = new Vector2(direction * speed, jumpForce);
        } else {
            rigid.velocity = new Vector2(direction * speed, rigid.velocity.y);
        }
    }

    bool IsGrounded() {
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, platformLayer);
        return raycast.collider != null;
    }
}
