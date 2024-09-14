using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private bool isCountDown = true;
    [SerializeField]
    private float maxTimeLimit = 0f;

    private bool isTimerStopped = false;
    private float displayTime = 0;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        if (isCountDown) displayTime = maxTimeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerStopped) return;

        elapsedTime += Time.deltaTime;

        if (isCountDown)
        {
            displayTime = maxTimeLimit - elapsedTime;
            if (displayTime <= 0)
            {
                displayTime = 0;
                StopTimer();
            }
        }
        else
        {
            displayTime = elapsedTime;
            if (displayTime >= maxTimeLimit)
            {
                displayTime = maxTimeLimit;
                StopTimer();
            }
        }
    }

    public float getMaxTimeLimit()
    {
        return maxTimeLimit;
    }

    public void StopTimer()
    {
        isTimerStopped = true;
    }

    public float getDisplayTime()
    {
        return displayTime;
    }
}
