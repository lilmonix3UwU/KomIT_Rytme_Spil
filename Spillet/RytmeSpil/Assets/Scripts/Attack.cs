using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float delay;
    PlayerController1 playerController;


    private void Start()
    {
        playerController = GetComponentInParent<PlayerController1>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(playerController.AddSpeedMod(0.01f, 0.5f));
            StartCoroutine(playerController.Nudge(1000, 0.1f, true));

        }

    }


    private IEnumerator AttackHitBox()
    {
        yield return new WaitForSeconds(delay);

    }

}
