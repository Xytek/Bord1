using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Win winScript;

    const float TIMER = 120.0f;

    [SerializeField]
    Text TimerText = null;

    float seconds = TIMER;

    bool isTimerStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        TimerText.text = time.ToString(@"mm\:ss");
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerStarted)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            seconds -= Time.deltaTime;
            TimerText.text = time.ToString(@"mm\:ss");
            if (seconds < 0)
            {
                winScript.decideWhoWon();
                //Logic to start new round or to show scores / end game.
            }

        }
    }


    void startTimer()
    {
        isTimerStarted = true;
    }

    void resetTimer()
    {
        seconds = TIMER;
        isTimerStarted = false;
    }
}
