using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class Scoring : MonoBehaviour
{
    public static Scoring Instance;

    public string CurrentHit = "MISS";
    public int comboCounter = 1;
    public List<GameObject> enemies;

    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text scoringActive;
    [SerializeField] GameObject winScreen;

    float startTimer = 5;
    int score = 0;
    bool once = true;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {

    }

    void Update()
    {
        scoringActive.text = "Score: " + score;
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
        }
        else
        {
            if (enemies.Count == 0 && once)
            {
                //WIN
                Time.timeScale = 0;
                text.text = "YOU WIN\nYOUR SCORE IS:\n" + score;
                winScreen.SetActive(true);
                once = false;
            }
        }
    }

    public void EnemyHit()
    {
        if (CurrentHit == "MISS")
        {
            score += 10;
            return;
        }
        if (CurrentHit == "YIKES")
        {
            score += comboCounter * 100;
            return;
        }        
        if (CurrentHit == "OK")
        {
            score += comboCounter * 150;
            return;
        }        
        if (CurrentHit == "GREAT")
        {
            score += comboCounter * 250;
            return;
        }



    }
}
