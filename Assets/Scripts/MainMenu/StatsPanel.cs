using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    public Text highScore;
    public Text totalRuns;
    public Text totalCarrots;
    public Text totalDistance;

    private void OnEnable()
    {
        // Gets the saved stats from PlayerPrefs
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        totalRuns.text = PlayerPrefs.GetInt("TotalRuns", 0) + " runs";
        totalCarrots.text = PlayerPrefs.GetInt("TotalCarrots", 0).ToString();
        totalDistance.text = PlayerPrefs.GetFloat("TotalDistance", 0).ToString("f2") + "km";
    }
}
