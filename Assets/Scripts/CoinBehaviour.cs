using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public bool grabbable;
    public float nextTimeToApear;
    public float maxTimeToApear;
    public Animator coinAnimator;


    private void Start()
    {
        coinAnimator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        nextTimeToApear -= Time.deltaTime;

        if(nextTimeToApear <= 0)
        {
            coinAnimator.SetTrigger("Disapear");
            grabbable = false;
            nextTimeToApear = maxTimeToApear;
        }
        else if(nextTimeToApear <= 15 && !grabbable)
        {
            coinAnimator.SetTrigger("Appear");
            grabbable = true;
        }
    }

    public void ResetCoin()
    {
        nextTimeToApear = nextTimeToApear + maxTimeToApear;
        grabbable = false;
        coinAnimator.SetTrigger("Disapear");
    }
}
