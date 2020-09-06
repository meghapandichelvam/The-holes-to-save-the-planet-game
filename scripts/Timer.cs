using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{

    public Text TimerText;
    public int mins;
    public int sec;

    int Total_Seconds =0;
    int TOTAL_SECONDS =0;
    
    void Start()
    {
        TimerText.text = mins + ":" + sec;
        if (mins > 0)
            Total_Seconds += mins * 60;
        if (sec > 0)
            Total_Seconds += sec;
        TOTAL_SECONDS = Total_Seconds;
        StartCoroutine(seconds());
    }

    void Update()
    {
        EndCheck();
    }
    public bool EndCheck()
    {
        if (sec == 0 && mins == 0)
        {
            TimerText.text = "Time is Up";
            StopAllCoroutines();
            return true;
        }else
        {
            return false;
        }
    }
    IEnumerator seconds()
    {
        yield return new WaitForSeconds(1f);
        if (sec > 0)
            sec--;

        if(sec ==0 && mins!=0)
        {
            sec = 60;
            mins--;
        }
        TimerText.text = mins + ":" + sec;
        StartCoroutine(seconds());
    }
}
