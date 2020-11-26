using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotManager : MonoBehaviour
{
    public Text carrotText;
    public Animator carrotUIAnimator;

    public bool mainMenu;

    private ScoreManager scoreManager;

    private int carrotCount;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }

    private void Start()
    {
        if(!mainMenu)
        {
            carrotCount = 0;
            carrotText.text = carrotCount.ToString();
        }
    }

    public void AcquireCarrot()
    {
        carrotCount += 1;
        carrotText.text = carrotCount.ToString();
        carrotUIAnimator.Play("CarrotUI");
        scoreManager.GainScore(5);
    }

    public void AcquireGoldenCarrot()
    {
        carrotCount += 20;
        carrotText.text = carrotCount.ToString();
        carrotUIAnimator.Play("CarrotUI");
        scoreManager.GainScore(100);
    }
}
