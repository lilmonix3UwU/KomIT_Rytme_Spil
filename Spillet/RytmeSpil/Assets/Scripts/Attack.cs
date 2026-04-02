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

    private void Start()
    {

    }

    private void Update()
    {

    }


    public void AttackStart(float dMod)
    {
        localDMod = dMod;
        StartCoroutine(playerController.AddSpeedMod(0.01f, animationTime));
        StartCoroutine(playerController.Nudge(1000, delay, true));
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        animator.SetBool("Attacking", true);
        StartCoroutine(playerController.FallSlow(animationTime + delay));

        yield return new WaitForSeconds(delay);
        
        GetComponent<BoxCollider2D>().enabled = true;
        hitbox.SetActive(true);
        

        yield return new WaitForSeconds(animationTime - delay - 0.01f);
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.01f);
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
        }
    }

}
