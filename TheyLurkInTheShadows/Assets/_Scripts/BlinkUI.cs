using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkUI : MonoBehaviour
{
    [SerializeField] Image playerBlinkIcon;
    [SerializeField] bool isPlayer = false;

    [SerializeField]
    public float timerDelay = 5f;
    public float currentTime;

    void UpdateUI()
    {
        playerBlinkIcon.fillAmount = currentTime / timerDelay;
    }
}
