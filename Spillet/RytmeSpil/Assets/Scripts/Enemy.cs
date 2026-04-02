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
    private void Start()
    {
        hitSFX = GetComponent<AudioSource>();
    }
    public IEnumerator TakeDamage(float damage)
    {
        if (dying)
            yield break;
        hitSFX.Stop();
        hitSFX.Play();
        HP -= damage;
        dmgText.text = damage.ToString();
        dmgText.gameObject.SetActive(true);
        if (HP <= 0)
            dying = true;
        yield return new WaitForSeconds(0.3f);
        dmgText.gameObject.SetActive(false);
        if (dying)
        {
            Destroy(gameObject);
        }
    }
}
