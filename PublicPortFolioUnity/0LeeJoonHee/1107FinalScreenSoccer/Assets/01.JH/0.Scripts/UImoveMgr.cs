using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImoveMgr : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.TweenUI += TweenMove;
    }

    private void OnDisable()
    {
        GameManager.TweenUI -= TweenMove;
    }

    public void TweenMove(GameObject gameObject)
    {
        float vector = 0.1f;
        float time = 2.0f;

        Hashtable ht1 = new Hashtable();
        ht1.Add("x", vector);
        ht1.Add("y", vector);
        ht1.Add("time", time);
        ht1.Add("easetype", iTween.EaseType.easeOutExpo);

        iTween.ShakeScale(gameObject, ht1);
    }
}
