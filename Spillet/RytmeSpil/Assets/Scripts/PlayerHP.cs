using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] int hp = 100;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text hpText;
    [SerializeField] GameObject loseScreen;





    public void Damage(int DMG)
    {
        hp -= DMG;
        StartCoroutine(flicker());
        hpText.text = "HP: " + hp;
        if (hp <= 0)
        {
            Time.timeScale = 0;
            loseScreen.SetActive(true);
            RythmAndComboController.Instance.baseBeat.Pause();
        }
    }
    private IEnumerator flicker()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.125f);
        spriteRenderer.color = Color.white;
    }
}
