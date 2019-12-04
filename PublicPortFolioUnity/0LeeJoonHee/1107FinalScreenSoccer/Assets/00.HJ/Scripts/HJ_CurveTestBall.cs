using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HJ_CurveTestBall : MonoBehaviour
{
    private Rigidbody rigid;

    Vector3 ballPos;
    Vector3 currentPos;
    private Vector3 mousePos;
    //private Vector3 targetPos;

    [Range(-365.0f, 365.0f)]
    public float spinTop = 0f;

    [Range(-365.0f, 365.0f)]
    public float spinSide = 0f;

    [Range(0, 150f)]
    public float ballSpeed = 0f;

    public Camera testBallCam;

    public GameObject IceSPear;

    float mouseX;
    float mouseY;
    float mouseZ;
    MeshRenderer meshRend;

    bool isShoot = false;

    void Awake()
    {
        ballPos = transform.position;
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        currentPos = transform.position;
        mousePos = testBallCam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, mouseZ));

        Debug.Log(mouseX + "," + mouseY + "," + mouseZ);

        //targetPos = mousePos - ballPos;
        ballPos = currentPos;
        ShootBall();
        if (isShoot == true)
        {
            transform.Rotate(spinTop, spinSide, 0);
            Invoke("Curving", 3f);
            IceEffect();

        }
    }
    private void ShootBall()
    {
        //Debug.Log("ballPos.y" + ballPos.y);
        //Debug.Log("currentPos.y" + currentPos.y);
        if (Input.GetMouseButtonDown(0))
        {
            isShoot = true;

            rigid.AddForce(new Vector3(0, 15, 50) * ballSpeed * Time.deltaTime);
            //rigid.velocity = new Vector3(0, 10, 10);

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
            mouseZ = Input.mousePosition.z;
        }
    }
    void Curving()
    {
        if (spinTop > 0 && spinSide > 0)
        {
            // Debug.Log("ballPos.y" + ballPos.y);
            // Debug.Log("currentPos.y" + currentPos.y);
            rigid.AddForce(new Vector3(-10, 0, 0), ForceMode.Force);
            //rigid.velocity = new Vector3(10, 0, 0) * Time.deltaTime;
            if (currentPos.y < ballPos.y)
            {
                rigid.AddRelativeForce(new Vector3(-100, 0, 0), ForceMode.Force);
                //rigid.velocity = new Vector3(-10, 0, 0) ;
                Debug.Log("이거 타자");
            }
        }

        else if (spinTop > 0 && spinSide < 0)
        {
            rigid.AddRelativeForce(new Vector3(-100, 0, 10) * Time.deltaTime, ForceMode.Impulse);
            if (currentPos.y < ballPos.y)
            {
                rigid.AddRelativeForce(new Vector3(1000, 0, 0) * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
    void IceEffect()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = new Color(0.5882f, 0.6431f, 1, 1);
        Debug.Log(" 파랑으로 바뀌어라");

        IceSPear.SetActive(true);
    }
}
