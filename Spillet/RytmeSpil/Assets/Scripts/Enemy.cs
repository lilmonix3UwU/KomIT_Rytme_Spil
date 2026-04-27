using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    [SerializeField] int dmg;
    [SerializeField] float knockback;
    [SerializeField] float speed;
    [SerializeField] float huntingSpeed;
    [SerializeField] float LaunchForce;
    [SerializeField] TMP_Text dmgText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDetector playerDetector;
    [SerializeField] EnemyHurtBox FUCKBOX;
    AudioSource hitSFX;
    bool dying = false;
    bool once = true;
    bool hurting = false;
    float currentHurtTime = 0;
    bool attacking = false;
    bool hunting = false;
    public bool hitWall = false;
    public bool playerDetectedInAttackRange = false;

    GameObject player;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitSFX = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        if (once)
        {
            Scoring.Instance.enemies.Add(gameObject);
            player = Scoring.Instance.gameObject;
            RythmAndComboController.Instance.enemies.Add(this);
            once = false;
        }




        if (!dying)
        {

            if (playerDetector.playerDetected)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        hunting = true;
                        playerDetector.BigBox(true);
                    }
                }
                
            }
            else
            {
                hunting = false;
                playerDetector.BigBox(false);
            }

            if (hitWall)
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }

            if (!attacking && currentHurtTime <= 0 && !hunting)
            {
                transform.position += new Vector3(-transform.localScale.x * speed * Time.deltaTime * 0.1f, 0, 0);
            }
            else if (hunting && !attacking && currentHurtTime <= 0)
            {
                if (transform.position.x < player.transform.position.x)
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    transform.position += new Vector3(1 * huntingSpeed * Time.deltaTime * 0.1f, 0, 0);
                }
                else
                {
                    if (transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    transform.position += new Vector3(-1 * huntingSpeed * Time.deltaTime * 0.1f, 0, 0);
                }
                
            }
            else if (currentHurtTime > 0)
            {
                attacking = false;
                currentHurtTime -= Time.deltaTime;
                animator.SetBool("Hurting", true);
            }

            if (currentHurtTime <= 0 && hurting)
            {
                animator.SetBool("Hurting", false);
                hurting = false;
            }
        }


        if (transform.localScale.x > 0)
        {
            dmgText.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else if (transform.localScale.x < 0)
        {
            dmgText.gameObject.transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
        }
    }
    public IEnumerator TakeDamage(float damage)
    {
        if (dying)
            yield break;
        hurting = true;
        currentHurtTime = 0.5f;
        StartCoroutine(flicker());
        hitSFX.Stop();
        hitSFX.Play();
        HP -= damage;
        Scoring.Instance.EnemyHit();
        dmgText.text = damage.ToString();
        dmgText.gameObject.SetActive(true);
        if (HP <= 0)
            dying = true;
        yield return new WaitForSeconds(0.3f);
        dmgText.gameObject.SetActive(false);
        if (dying)
        {
            Scoring.Instance.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    public void beat()
    {
        if (playerDetectedInAttackRange && !attacking && currentHurtTime <= 0)
        {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        bool right = true;
        attacking = true;
        animator.SetBool("Attacking", true);
        if (player.transform.position.x < transform.position.x)
        {
            right = true;
        }
        if (player.transform.position.x > transform.position.x)
        {
            right= false;
        }
        yield return new WaitForSeconds(0.5f);
        if (!attacking)
        {
            animator.SetBool("Attacking", false);
            yield break;
        }
        if (right)
        {
            rb.AddForce(-Vector2.right * LaunchForce, ForceMode2D.Impulse);
        }
        if (!right)
        {
            rb.AddForce(Vector2.right * LaunchForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
        if (!attacking)
        {
            animator.SetBool("Attacking", false);
            yield break;
        }
        if (FUCKBOX.playerinFUCKBox)
        {
            player.GetComponent<PlayerHP>().Damage(dmg);
            player.GetComponent<PlayerController1>().enabled = false;
            if (!right)
            {
                Debug.Log("aaa");
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * knockback, ForceMode2D.Impulse);
            }
            if (right)
            {
                Debug.Log("eeee");
                player.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * knockback, ForceMode2D.Impulse);
            }
        }
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<PlayerController1>().enabled = true;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        
    }
    private IEnumerator flicker()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = Color.white;
    }
}
