using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJ_CameraViewforTarget : MonoBehaviour
{
    public Transform target;
    Transform camPos;
    private float height = 2;
    public static float dist = -7f;

    void Start()
    {
        camPos = new GameObject("CamPos").transform;
        camPos.parent = target;

        transform.position = camPos.position;
    }

    void LateUpdate()
    {
        camPos.localPosition = new Vector3(0, height, -dist);
        transform.position = Vector3.Lerp(transform.position, camPos.position, 12 * Time.deltaTime);

        //y축 회전만 유지
        Vector3 ang = transform.eulerAngles;
        Vector3 tarAng = camPos.eulerAngles;
        ang.x = Mathf.LerpAngle(ang.x, tarAng.x, 20 * Time.deltaTime);

        // x축 회전
        // if (Input.GetMouseButton(0))
        // {
        //     float x = Input.GetAxis("Mouse X");
        //     ang.y += -x * 30 * Time.deltaTime;
        //     transform.eulerAngles = ang;
        // }
    }
}


