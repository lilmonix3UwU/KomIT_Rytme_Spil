using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{

    [SerializeField] GroundCheck groundCheck;
    [SerializeField] float moveSpeed;
    [SerializeField] float animationSpeedMod;
    [SerializeField] float jummpForce;
    [SerializeField] Animator animator;
    [SerializeField] GameObject boostArrow;
    Rigidbody2D rb;
    List<float> negSpeedMods;
    float negSpeedMod = 1f;
    List<float> posSpeedMods;
    float posSpeedMod = 1f;
    [SerializeField] float maxFallSpeed;
    float currentMaxFallSpeed;
    bool canJump = true;
    public bool facingRight = true;


    private void Start()
    {
        
        currentMaxFallSpeed = maxFallSpeed;
        negSpeedMods = new List<float>();
        posSpeedMods = new List<float>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y <= -currentMaxFallSpeed)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * posSpeedMod * negSpeedMod, -currentMaxFallSpeed);
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * posSpeedMod * negSpeedMod, rb.velocity.y);
        }
    }

    void Update()
    {

        if (animator.GetBool("Attacking") && animator.speed != 2)
        {
            animator.speed = 2;
        }
        else if (animator.GetBool("Walking") && animator.speed != moveSpeed * animationSpeedMod && !animator.GetBool("Attacking"))
        {
            animator.speed = moveSpeed * animationSpeedMod;
        }
        else if (animator.speed != 1 && !animator.GetBool("Walking") && !animator.GetBool("Attacking"))
        {
            animator.speed = 1;
        }



        if (canJump && groundCheck.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 1) * jummpForce, ForceMode2D.Impulse);
            groundCheck.isGrounded = false;
            animator.SetTrigger("Jump");
        }

        if (groundCheck.isGrounded && !animator.GetBool("OnGround"))
        {
            animator.SetBool("OnGround", true);
        }
        else if (!groundCheck.isGrounded && animator.GetBool("OnGround"))
        {
            animator.SetBool("OnGround", false);
        }

        if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight && !animator.GetBool("Attacking"))
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && facingRight && !animator.GetBool("Attacking"))
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        if ((rb.velocity.x < -0.5f || rb.velocity.x > 0.5f) && groundCheck.isGrounded)
        {
            animator.SetBool("Walking", true);
        }
        else if (animator.GetBool("Walking"))
        {
            animator.SetBool("Walking", false);
            animator.speed = 1;
        }



        if (negSpeedMods.Count > 0)
        {
            negSpeedMods.Sort((x, y) => x.CompareTo(y));
            negSpeedMod = negSpeedMods[0];
        }
        else
        {
            negSpeedMod = 1;
        }
        if (posSpeedMods.Count > 0)
        {
            boostArrow.SetActive(true);
            posSpeedMods.Sort((x, y) => -x.CompareTo(y));
            posSpeedMod = posSpeedMods[0];
        }
        else
        {
            boostArrow.SetActive(false);
            posSpeedMod = 1;
        }

    }

    public IEnumerator AddSpeedMod(float mod, float duration)
    {

        if (mod < 1)
        {
            negSpeedMods.Add(mod);
        }
        if (mod > 1)
        {
            posSpeedMods.Add(mod);
        }

        yield return new WaitForSeconds(duration);
        
        if (mod < 1)
        {
            negSpeedMods.Remove(mod);
        }
        if (mod > 1)
        {
            posSpeedMods.Remove(mod);
        }
    }

    public IEnumerator Nudge(float force, float delay, bool withDirection)
    {
        yield return new WaitForSeconds(delay);

        if ((withDirection && facingRight) || (!withDirection && facingRight))
        {
            
            rb.AddForce(new Vector2(force, 0));
        }
        else
        {
            rb.AddForce(new Vector2(-force, 0));
        }
    }

    public IEnumerator FallSlow(float time)
    {
        currentMaxFallSpeed = 1;
        yield return new WaitForSeconds(time);
        currentMaxFallSpeed = maxFallSpeed;
    }
}
