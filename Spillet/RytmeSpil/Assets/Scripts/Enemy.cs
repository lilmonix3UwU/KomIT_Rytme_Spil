using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    [SerializeField] TMP_Text dmgText;
    AudioSource hitSFX;
    bool dying = false;
    bool once = true;

    GameObject player;

    private void Start()
    {
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




    }
    public IEnumerator TakeDamage(float damage)
    {
        if (dying)
            yield break;
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
}
