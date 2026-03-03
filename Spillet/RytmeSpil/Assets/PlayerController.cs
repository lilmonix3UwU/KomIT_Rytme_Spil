using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] float moveSpeed;
    [SerializeField] float jummpForce;
    Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, rb.velocity.y);
        if (groundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 1) * jummpForce, ForceMode2D.Impulse);
            groundCheck.isGrounded = false;
        }
       
    }
}
