using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RythmAndComboController : MonoBehaviour
{
    public static RythmAndComboController Instance;

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
    [SerializeField] RectTransform metronome2;
    [SerializeField] RectTransform metronomeIndicator;
    [SerializeField] GameObject greatIconT;
    [SerializeField] GameObject okIconT;
    [SerializeField] GameObject yikesIconT;
    [SerializeField] GameObject missIconT;
    [SerializeField] Animator animator;
    [SerializeField] NearByEnemies nearByEnemies;
    [SerializeField] Image note1;
    [SerializeField] Image note2;
    [SerializeField] Image note3;
    [SerializeField] Image note4;
    [SerializeField] Image note5;
    [SerializeField] Sprite note;
    [SerializeField] Sprite line;

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
    Camera cam;
    List<Image> notes;
    List<Enemy> enemies;

    void Start()
    {
        enemies = new List<Enemy>();
        playerController = GetComponent<PlayerController1>();
        metronomeAudioSource = GetComponent<AudioSource>();
        currentCombo = new List<bool>();
        combos = new List<bool[]>();
        validCombos = new List<bool[]>();
        oneBeat = 60 / BPM;
        cam = Camera.main;
        notes = new List<Image>();
        notes.Add(note1);
        notes.Add(note2);
        notes.Add(note3);
        notes.Add(note4);
        notes.Add(note5);

        combos.Add(new bool[] { true, true, false, true });
        combos.Add(new bool[] { true, false, true, true });
        combos.Add(new bool[] { true, false, true, false, true });
        for (int i = 0; i < combos.Count; i++)
        {
            bool[] temp = combos[i];
            validCombos.Add(temp);
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    void Update()
    {
        metronome.position = new Vector3((Mathf.InverseLerp(0, oneBeat, beatTimer) * (metronome2.transform.position.x - metronome.transform.position.x)) + metronomeIndicator.position.x, metronome.position.y, metronome.position.z);

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
            foreach (Enemy e in enemies)
            {
                e.beat();
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
                guitar.damageMod = 2.5f;
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
            if (comboCount == 1)
            {
                animator.SetInteger("Combo", 1);
            }
            if ((comboCount == 2 && currentCombo[1]) || (comboCount == 4 && currentCombo[3]) || comboCount == 0)
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

            if (hasAttackedThisBeat)
            {
                bool t = true;
                if (currentCombo.Count == 2)
                {
                    if (currentCombo[0] == true && currentCombo[1] == true)
                    {
                        currentCombo.Clear();
                        comboCount = 1;
                        bool f = true;
                        currentCombo.Add(f);

                    }
                    else
                    {
                        comboCount++;
                    }
                        
                }
                else
                {
                    comboCount++;
                }
                currentCombo.Add(t);
            }
            else if (comboCount != 0)
            {
                if (currentCombo[comboCount - 1])
                {
                    bool f = false;
                    currentCombo.Add(f);
                    comboCount++;
                }
                else
                {
                    validCombos.Clear();
                }
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
            }

            Scoring.Instance.comboCounter = comboCount;

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

        if (currentCombo.Count > 0)
        {
            if (currentCombo.Count > 4)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (currentCombo[i])
                    {
                        notes[i].sprite = note;
                    }
                    else
                    {
                        notes[i].sprite = line;
                    }
                    if (i != 0)
                    {
                        notes[i].gameObject.SetActive(true);
                    }
                    
                }
                note1.gameObject.transform.localPosition = new Vector2(-70, 80);
                note1.gameObject.SetActive(true);
            }
            else
            {
                float offset = 0;
                for (int i = 0; i < currentCombo.Count; i++)
                {
                    if (currentCombo[i])
                    {
                        notes[i].sprite = note;
                    }
                    else
                    {
                        notes[i].sprite = line;
                    }
                    notes[i].gameObject.SetActive(true);

                    if (i != 0)
                    {
                        notes[i].gameObject.SetActive(true);
                        offset -= 17.5f;
                    }

                }
                



                note1.gameObject.transform.localPosition = new Vector2(offset, 80);
                note1.gameObject.SetActive(true);
            }

        }
        else if(note1.IsActive() || note2.IsActive() || note3.IsActive() || note4.IsActive() || note5.IsActive())
        {
            note1.gameObject.SetActive(false);
            note2.gameObject.SetActive(false);
            note3.gameObject.SetActive(false);
            note4.gameObject.SetActive(false);
            note5.gameObject.SetActive(false);
        }


    }

    private bool TryAttack(Attack weapon)
    {
        if ( (beatTimer > (oneBeat - greatWindow)) || (beatTimer < greatWindow) )
        {
            //display GREAT particle
            weapon.AttackStart(greatMod);
            greatIconT.SetActive(true);
            Scoring.Instance.CurrentHit = "GREAT";
            return true;
        }
        else if ( (beatTimer > (oneBeat - (greatWindow + okWindow))) || (beatTimer < (greatWindow + okWindow)) )
        {
            //display OK particle
            weapon.AttackStart(okMod);
            okIconT.SetActive(true);
            Scoring.Instance.CurrentHit = "OK";
            return true;
        }
        else if ((beatTimer > (oneBeat - (greatWindow + okWindow + yikesWindow))) || (beatTimer < (greatWindow + okWindow + yikesWindow)))
        {
            //display YIKES particle
            weapon.AttackStart(yikesMod);
            yikesIconT.SetActive(true);
            Scoring.Instance.CurrentHit = "YIKES";
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
            Scoring.Instance.CurrentHit = "MISS";
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
