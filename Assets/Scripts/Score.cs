using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private float score = 0.0f;
    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 2;
    private int scoreToNextLevel = 100;
    private bool isDeath = false;
    public Text scoreText;
    public DeathMenu deathMenu;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(isDeath){
            return;
        }
        if(score >= scoreToNextLevel) {
            LevelUp();
        }
        score += Time.deltaTime;
        scoreText.text = ((int)score).ToString();
    }

    void LevelUp() {
        if(difficultyLevel == maxDifficultyLevel) {
            return;
        }
        scoreToNextLevel *= 2;
        difficultyLevel++;
        GetComponent<CharacterControl>().SetSpeed(difficultyLevel);

        Debug.Log(difficultyLevel);
    }

    public void OnDeath() {
        isDeath = true;
        PlayerPrefs.SetFloat("Highscore", score);
        deathMenu.ToggleEndMenu (score);
    }
}
