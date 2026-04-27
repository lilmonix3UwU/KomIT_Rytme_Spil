using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    [SerializeField] GameObject menu;
    private void Start()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        RythmAndComboController.Instance.baseBeat.Pause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (menu.activeInHierarchy)
            {
                if (!RythmAndComboController.Instance.baseBeat.isPlaying)
                {
                    StartCoroutine(FUCK());
                }
                menu.SetActive(false);
                Time.timeScale = 1;
                RythmAndComboController.Instance.baseBeat.UnPause();
            }
            else if (!menu.activeInHierarchy)
            {
                menu.SetActive(true);
                Time.timeScale = 0;
                RythmAndComboController.Instance.baseBeat.Pause();
            }
        }
    }
    private IEnumerator FUCK()
    {
        yield return new WaitForSeconds(0.4f);
        RythmAndComboController.Instance.baseBeat.Play();
    }
}
