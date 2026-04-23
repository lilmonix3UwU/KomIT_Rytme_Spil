using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damageMod = 1;
    float localDMod = 1;
    [SerializeField] float baseDamage;
    [SerializeField] float delay;
    public float animationTime;
    [SerializeField] PlayerController1 playerController;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource hit1;
    [SerializeField] AudioSource hit2;
    [SerializeField] AudioSource hit3;
    [SerializeField] GameObject hitbox;
    [SerializeField] float pushForce = 5;

    private void Start()
    {

    }

    private void Update()
    {

    }


    public void AttackStart(float dMod)
    {
        animator.SetBool("Attacking", true);
        localDMod = dMod;
        StartCoroutine(playerController.AddSpeedMod(0.01f, animationTime));
        StartCoroutine(playerController.Nudge(1000, delay, true));
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        StartCoroutine(playerController.FallSlow(animationTime + delay));

        yield return new WaitForSeconds(delay);
        
        GetComponent<BoxCollider2D>().enabled = true;
        hitbox.SetActive(true);

        if (animator.GetInteger("Combo") == 0)
        {
            yield return new WaitForSeconds((0.767f * 0.5f) - delay);
        }
        if (animator.GetInteger("Combo") == 1)
        {
            yield return new WaitForSeconds((0.683f * 0.5f) - delay);
        }
        if (animator.GetInteger("Combo") == 2)
        {
            yield return new WaitForSeconds((0.933f * 0.5f) - delay);
        }
        animator.SetBool("Attacking", false);
       
        GetComponent<BoxCollider2D>().enabled = false;
        hitbox.SetActive(false);
        if (damageMod > 1)
        {
            damageMod = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(collision.gameObject.GetComponent<Enemy>().TakeDamage(baseDamage * damageMod * localDMod));
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - playerController.gameObject.transform.position).normalized * pushForce, ForceMode2D.Impulse);
        }
    }

}
