using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{  
    public Text scoreText;

    public bool mainMenu;

    private TerrainSpawner terrainSpawner;

    private float score;
    private bool isScoring;

    private void Awake()
    {
        terrainSpawner = FindObjectOfType<TerrainSpawner>();
    }

    private void Start()
    {
        if(!mainMenu)
        {
            scoreText.text = "0";
            StartScoring();
        }
    }

    void Update()
    {
        if(isScoring)
        {
            score += Mathf.Pow(-terrainSpawner.GetMoveSpeed(), 2) * Time.deltaTime;
            scoreText.text = Mathf.Floor(score).ToString();
        }
    }

    // Begins incrementing the score
    public void StartScoring()
    {
        isScoring = true;
    }

    public void StopScoring()
    {
        isScoring = false;
    }

    public void GainScore(float gained)
    {
        score += gained;
    }

    public float GetScore()
    {
        return score;
    }
}
