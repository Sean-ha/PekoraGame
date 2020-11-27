using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnSliderRelease : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.CarrotPickUp);
    }
}
