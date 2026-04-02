using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RythmAndComboController : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] Attack guitar;
    [SerializeField] Attack flute;

    [Header("Timers and Mods")]
    [SerializeField] float greatWindow = 0.05f;
    [SerializeField] float greatMod = 2.0f;
    [SerializeField] float okWindow = 0.05f;
    [SerializeField] float okMod = 1.5f;
    [SerializeField] float yikesWindow = 0.05f;
    [SerializeField] float yikesMod = 1.0f;
    [SerializeField] float missMod = 0.2f;
    [SerializeField] float comboPushForce = 5.0f;

    [Header("Sounds")]
    [SerializeField] AudioSource baseBeat;
    [SerializeField] AudioSource guitar1;
    [SerializeField] AudioSource guitar2;
    [SerializeField] AudioSource guitar3;

    [Header("Other")]
    [SerializeField] float BPM = 90;
    [SerializeField] RectTransform metronome;
    [SerializeField] GameObject greatIconT;
    [SerializeField] GameObject okIconT;
    [SerializeField] GameObject yikesIconT;
    [SerializeField] GameObject missIconT;
    [SerializeField] Animator animator;
    [SerializeField] NearByEnemies nearByEnemies;

    public bool metronomeSFXOn = true;

    bool hasAttackedThisBeat;
    int comboCount = 0;
    List<bool> currentCombo;
    List<bool[]> combos;
    List<bool[]> validCombos;

    float beatTimer = 0;
    bool beatOn = true;
    float oneBeat;
    float currentAttackCooldown = 0;
    bool toggle = false;
    float attackCooldown;
    AudioSource metronomeAudioSource;
    PlayerController1 playerController;


    void Start()
    {
        playerController = GetComponent<PlayerController1>();
        metronomeAudioSource = GetComponent<AudioSource>();
        currentCombo = new List<bool>();
        combos = new List<bool[]>();
        validCombos = new List<bool[]>();
        oneBeat = 60 / BPM;

        combos.Add(new bool[] { true, true, false, true });
        combos.Add(new bool[] { true, false, true, true });
        combos.Add(new bool[] { true, false, true, false, true });
        for (int i = 0; i < combos.Count; i++)
        {
            bool[] temp = combos[i];
            validCombos.Add(temp);
        }
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
            if (metronomeSFXOn)
            {
                metronomeAudioSource.Stop();
                metronomeAudioSource.Play();
            }

        }

        /*if (Input.GetKeyDown(KeyCode.J) && currentAttackCooldown <= 0)
        {
            TryAttack(flute);
            currentAttackCooldown = flute.animationTime;
        }*/
        if (Input.GetKeyDown(KeyCode.K) && currentAttackCooldown <= 0)
        {
            List<bool> l = new List<bool>();
            for (int i = 0; i < combos[0].Count() - 1 ; i++)
            {
                l.Add(combos[0][i]);
            }
            if (CompareCombos(currentCombo, l))
            {
                guitar.damageMod = 1.5f;
            }
            hasAttackedThisBeat = TryAttack(guitar);
            currentAttackCooldown = guitar.animationTime;
            if (hasAttackedThisBeat)
            {
                if (comboCount == 0)
                    guitar1.Play();
                if (comboCount == 1 || comboCount == 2)
                    guitar2.Play();
                if (comboCount == 3 || comboCount == 4)
                    guitar3.Play();
            }

        }

        if (beatTimer >= (oneBeat - (greatWindow + okWindow + yikesWindow)) && toggle)
        {
            hasAttackedThisBeat = false;
            toggle = false;
        }
        else if (beatTimer < (oneBeat - (greatWindow + okWindow + yikesWindow)) && beatTimer > greatWindow + okWindow + yikesWindow && !toggle)
        {
            toggle = true;
            if (hasAttackedThisBeat)
            {
                bool t = true;
                currentCombo.Add(t);
                comboCount++;
            }
            else if (comboCount != 0)
            {
                bool f = false;
                currentCombo.Add(f);
                comboCount++;
            }
            for (int i = 0; i < validCombos.Count; i++)
            {
                if (validCombos[i].Length < comboCount)
                {
                    validCombos.Remove(validCombos[i]);
                    continue;
                }
                for (int j = 0; j < currentCombo.Count; j++)
                {
                    if (validCombos[i][j] != currentCombo[j])
                    {
                        validCombos.Remove(validCombos[i]);
                        break;
                    }
                }
            }

            if (comboCount == 1)
            {
                animator.SetInteger("Combo", 1);
            }
            if ((comboCount == 2 && currentCombo[1]) || (comboCount == 4 && currentCombo[3]))
            {
                animator.SetInteger("Combo", 0);
            }
            if (comboCount == 2 && !currentCombo[1])
            {
                animator.SetInteger("Combo", 1);
            }
            if (comboCount == 3)
            {
                animator.SetInteger("Combo", 2);
            }

            if (validCombos.Count == 0)
            {
                currentCombo.Clear();
                comboCount = 0;
                for (int i = 0; i < combos.Count; i++)
                {
                    bool[] temp = combos[i];
                    validCombos.Add(temp);
                }
                animator.SetInteger("Combo", 0);
                if (hasAttackedThisBeat)
                {
                    bool t = true;
                    currentCombo.Add(t);
                    comboCount++;
                }
            }

            if (validCombos.Count == 1)
            {

                if (CompareCombos(currentCombo, combos[1].ToList()))
                {
                    foreach (GameObject g in nearByEnemies.enemies)
                    {
                        Vector2 pushDirection = (g.transform.position - gameObject.transform.position).normalized;
                        g.GetComponent<Rigidbody2D>().AddForce(pushDirection * comboPushForce, ForceMode2D.Impulse);
                    }
                }
                if (CompareCombos(currentCombo, combos[2].ToList()))
                {
                    StartCoroutine(playerController.AddSpeedMod(1.3f, 5));
                }




            }
        }

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
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

    private bool TryAttack(Attack weapon)
    {
        if ( (beatTimer > (oneBeat - greatWindow)) || (beatTimer < greatWindow) )
        {
            //display GREAT particle
            weapon.AttackStart(greatMod);
            greatIconT.SetActive(true);
            return true;
        }
        else if ( (beatTimer > (oneBeat - (greatWindow + okWindow))) || (beatTimer < (greatWindow + okWindow)) )
        {
            //display OK particle
            weapon.AttackStart(okMod);
            okIconT.SetActive(true);
            return true;
        }
        else if ((beatTimer > (oneBeat - (greatWindow + okWindow + yikesWindow))) || (beatTimer < (greatWindow + okWindow + yikesWindow)))
        {
            //display YIKES particle
            weapon.AttackStart(yikesMod);
            yikesIconT.SetActive(true);
            return true;
        }
        else
        {
            //display MISS particle
            weapon.AttackStart(missMod);
            missIconT.SetActive(true);
            currentCombo.Clear();
            comboCount = 0;
            for (int i = 0; i < combos.Count; i++)
            {
                bool[] temp = combos[i];
                validCombos.Add(temp);
            }
            animator.SetInteger("Combo", 0);
            return false;
        }
    }

    private bool CompareCombos(List<bool> c1, List<bool> c2)
    {
        if (c1.Count != c2.Count)
            return false;
        for (int i = 0; i < c1.Count; i++)
        {
            if (c1[i] != c2[i])
                return false;            
        }
        return true;
    }

}
