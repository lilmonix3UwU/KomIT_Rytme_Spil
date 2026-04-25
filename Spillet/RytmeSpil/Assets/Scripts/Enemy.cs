using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    [SerializeField] float speed;
    [SerializeField] float huntingSpeed;
    [SerializeField] TMP_Text dmgText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDetector playerDetector;
    AudioSource hitSFX;
    bool dying = false;
    bool once = true;
    bool hurting = false;
    float currentHurtTime = 0;
    bool attacking = false;
    float currentAttackTime = 0;
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
                    Debug.Log(hit.collider);
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

            if (currentAttackTime <= 0 && currentHurtTime <= 0 && !hunting)
            {
                transform.position += new Vector3(-transform.localScale.x * speed * Time.deltaTime * 0.1f, 0, 0);
            }
            else if (hunting && currentAttackTime <= 0 && currentHurtTime <= 0)
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
            else if (currentAttackTime > 0 && currentHurtTime <= 0)
            {
                currentAttackTime -= Time.deltaTime;
                animator.SetBool("Attacking", true);
            }
            else if (currentHurtTime > 0)
            {
                currentAttackTime = 0;
                currentHurtTime -= Time.deltaTime;
                animator.SetBool("Hurting", true);
            }


            if (currentAttackTime <= 0 && attacking)
            {
                animator.SetBool("Attacking", false);
                hurting = false;
            }
            if (currentHurtTime <= 0 && hurting)
            {
                animator.SetBool("Hurting", false);
                hurting = false;
            }
        }



    }
    public IEnumerator TakeDamage(float damage)
    {
        if (dying)
            yield break;
        hurting = true;
        currentHurtTime = 0.5f;
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
        if (playerDetectedInAttackRange && currentAttackTime <= 0 && currentHurtTime <= 0)
        {
            attacking = true;
            currentAttackTime = 2;
        }
    }
}
