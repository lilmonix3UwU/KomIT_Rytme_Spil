using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RythmAndComboController : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] Attack guitar;
    [SerializeField] Attack flute;

    [Header("Timers and Mods")]
    [SerializeField] float attackCooldown = 0.3f;
    [SerializeField] float greatWindow = 0.05f;
    [SerializeField] float greatMod = 2.0f;
    [SerializeField] float okWindow = 0.05f;
    [SerializeField] float okMod = 1.5f;
    [SerializeField] float yikesWindow = 0.05f;
    [SerializeField] float yikesMod = 1.0f;
    [SerializeField] float missMod = 0.2f;

    [Header("Other")]
    [SerializeField] float BPM = 90;
    [SerializeField] RectTransform metronome;
    [SerializeField] GameObject greatIconT;
    [SerializeField] GameObject okIconT;
    [SerializeField] GameObject yikesIconT;
    [SerializeField] GameObject missIconT;



    float beatTimer = 0;
    bool beatOn = true;
    float oneBeat;
    float currentAttackCooldown = 0;


    void Start()
    {
        oneBeat = 60 / BPM;
    }


    void Update()
    {
        metronome.position = new Vector3(Mathf.InverseLerp(0, oneBeat, beatTimer) * 500 , metronome.position.y, metronome.position.z);

        
        if (beatOn)
        {
            beatTimer += Time.deltaTime;
        }
        if (beatTimer >= oneBeat)
        {
            beatTimer -= oneBeat;
        }

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.J) && currentAttackCooldown <= 0)
        {
            TryAttack(flute);
            currentAttackCooldown = attackCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.K) && currentAttackCooldown <= 0)
        {
            TryAttack(guitar);
            currentAttackCooldown = attackCooldown;
        }


        if (currentAttackCooldown <= 0 && greatIconT.activeInHierarchy)
        {
            greatIconT.SetActive(false);
        }
        if (currentAttackCooldown <= 0 && okIconT.activeInHierarchy)
        {
            okIconT.SetActive(false);
        }
        if (currentAttackCooldown <= 0 && yikesIconT.activeInHierarchy)
        {
            yikesIconT.SetActive(false);
        }
        if (currentAttackCooldown <= 0 && missIconT.activeInHierarchy)
        {
            missIconT.SetActive(false);
        }
    }

    private void TryAttack(Attack weapon)
    {

        if ( (beatTimer > (oneBeat - greatWindow)) || (beatTimer < greatWindow) )
        {
            //display GREAT particle
            weapon.AttackStart(greatMod);
            greatIconT.SetActive(true);



        }
        else if ( (beatTimer > (oneBeat - (greatWindow + okWindow))) || (beatTimer < (greatWindow + okWindow)) )
        {
            //display OK particle
            weapon.AttackStart(okMod);
            okIconT.SetActive(true);



        }
        else if ((beatTimer > (oneBeat - (greatWindow + okWindow + yikesWindow))) || (beatTimer < (greatWindow + okWindow + yikesWindow)))
        {
            //display YIKES particle
            weapon.AttackStart(yikesMod);
            yikesIconT.SetActive(true);



        }
        else
        {
            //display MISS particle
            weapon.AttackStart(missMod);
            missIconT.SetActive(true);


        }




    }
}
