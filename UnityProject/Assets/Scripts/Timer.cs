using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public delegate void TimerEvent();
    public TimerEvent onEnd;

    private bool isTicking = false;
    private bool isCountdown = false;
    private float chrono = 0;

    public void start() { isTicking = true; }

    public void stop() { isTicking = false; }

    public void reset(float chronoR = 0)
    {
        chrono = chronoR;
        isCountdown = chrono != 0;
        updateChrono();
    }

    // Start is called before the first frame update
    void Start()
    {
        updateChrono();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTicking)
        {
            if (isCountdown)
            {
                chrono -= Time.deltaTime;
                if (chrono <= 0)
                {
                    chrono = 0;
                    stop();
                    onEnd?.Invoke();
                } 
            }
            else
            {
                chrono += Time.deltaTime;
            }
            updateChrono();
        }
    }

    void updateChrono()
    {
        int minutes = (int)(chrono / 60);
        int secondes = (int)(chrono % 60);
        int centieme = (int)(chrono * 100 % 100);

        gameObject.GetComponent<Text>().text = numberWith2digits(minutes) + ":" + numberWith2digits(secondes) + "." + numberWith2digits(centieme);
    }

    string numberWith2digits(int nb)
    {
        return (nb < 10 ? "0" : "") + nb;
    }
}
