using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotManager : MonoBehaviour
{
    public Text carrotText;
    public Animator carrotUIAnimator;

    private int carrotCount;

    private void Start()
    {
        carrotCount = 0;
        carrotText.text = carrotCount.ToString();
    }

    public void AcquireCarrot()
    {
        carrotCount += 1;
        carrotText.text = carrotCount.ToString();
        carrotUIAnimator.Play("CarrotUI");
    }

    public void AcquireGoldenCarrot()
    {
        carrotCount += 20;
        carrotText.text = carrotCount.ToString();
        carrotUIAnimator.Play("CarrotUI");
    }
}
