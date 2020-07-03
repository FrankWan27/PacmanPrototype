using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public int timeRemaining = 0000;
    public RectTransform rt;

    private void Update()
    {
        rt.sizeDelta = new Vector2(timeRemaining / 5, 10);
        timeRemaining--;
    }

    public void ResetTimer()
    {
        timeRemaining = 3000;
    }

    public void ResetTimer(int time)
    {
        timeRemaining = time;
    }

    public void ClearTimer()

    {
        timeRemaining = 0;
    }
}
