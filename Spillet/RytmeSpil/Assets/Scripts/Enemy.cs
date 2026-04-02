using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    [SerializeField] TMP_Text dmgText;
    AudioSource hitSFX;
    private void Start()
    {
        hitSFX = GetComponent<AudioSource>();
    }
    public IEnumerator TakeDamage(float damage)
    {
        hitSFX.Stop();
        hitSFX.Play();
        HP -= damage;
        dmgText.text = damage.ToString();
        dmgText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        dmgText.gameObject.SetActive(false);
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
