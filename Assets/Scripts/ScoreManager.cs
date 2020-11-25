using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private TerrainSpawner terrainSpawner;

    private float score;
    private bool isScoring;

    private void Awake()
    {
        terrainSpawner = FindObjectOfType<TerrainSpawner>();
    }

    private void Start()
    {
        scoreText.text = "0";
        StartScoring();
    }

    void Update()
    {
        if(isScoring)
        {
            score += -terrainSpawner.GetMoveSpeed()/10;
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
}
