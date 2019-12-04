using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChild : MonoBehaviour
{
    //by준희, 해당 오브젝트의 자식
    int childs;

    private void OnEnable()
    {
        GameManager.FireBall += FireEffect;
    }
    private void OnDisable()
    {
        GameManager.FireBall -= FireEffect;
    }
    private void Awake()
    {
        FireEffect(false);
    }

    void FireEffect(bool effect)
    {
        childs = gameObject.transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            if (i < childs-1)  //by준희, 불꽃슛 제어
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(effect);
            }
            else
            {   //by준희, 일반 슛 트레일 제어
                gameObject.transform.GetChild(i).gameObject.SetActive(!effect);

            }
            
            
            
        }
    }
}
