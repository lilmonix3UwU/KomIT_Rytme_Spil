using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damageMod = 1;
    [SerializeField] float baseDamage;
    [SerializeField] float delay;
    [SerializeField] float animationTime;
    PlayerController1 playerController;

    [SerializeField] GameObject hitbox;

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController1>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AttackStart();
        }

    }


    public void AttackStart()
    {
        StartCoroutine(playerController.AddSpeedMod(0.01f, delay + animationTime));
        StartCoroutine(playerController.Nudge(1000, delay, true));
        StartCoroutine(AttackHitBox());
    }

    private IEnumerator AttackHitBox()
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(playerController.FallSlow(animationTime));
        GetComponent<BoxCollider2D>().enabled = true;
        hitbox.SetActive(true);

        yield return new WaitForSeconds(animationTime);

        GetComponent<BoxCollider2D>().enabled = false;
        hitbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().HP -= baseDamage * damageMod;
        }
    }

}
