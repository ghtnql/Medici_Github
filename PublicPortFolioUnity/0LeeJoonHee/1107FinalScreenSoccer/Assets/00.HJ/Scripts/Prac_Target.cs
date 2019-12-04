using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prac_Target : MonoBehaviour
{
    Quaternion originalRotation;
    float rotSpeed = 1.0f;
    public static bool isGoal = false;

    void Start()
    {
        originalRotation = transform.rotation;
    }
    void Update()
    {
        ReturnPos();
    }

    private void ReturnPos()
    {
        if (gameObject.transform.localEulerAngles.x > 270f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * rotSpeed);
        }
    }
}