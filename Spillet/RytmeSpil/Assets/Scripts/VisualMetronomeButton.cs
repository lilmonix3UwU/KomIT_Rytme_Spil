using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualMetronomeButton : MonoBehaviour
{
    [SerializeField] GameObject metronomeThingy1;
    [SerializeField] GameObject metronomeThingy2;
    [SerializeField] TMP_Text text;


    public void Button()
    {
        if (text.text == "Visual Metronome: On")
        {
            metronomeThingy1.SetActive(false);
            metronomeThingy2.SetActive(false);
            text.text = "Visual Metronome: Off";
        }
        else if (text.text == "Visual Metronome: Off")
        {
            metronomeThingy1.SetActive(true);
            metronomeThingy2.SetActive(true);
            text.text = "Visual Metronome: On";
        }
    }
}
