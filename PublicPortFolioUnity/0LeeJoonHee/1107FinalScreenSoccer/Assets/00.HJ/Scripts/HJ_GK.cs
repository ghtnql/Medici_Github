using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HJ_GK : MonoBehaviour
{
    public Animator animator;
    public Transform keeper;
    public static bool keeperAction;

    float Shootx;
    float Shooty;

    Vector3 position;
    Quaternion rotation;

    float cent_min_x = -6;
    float cent_max_x = 6;
    float cent_min_y = -9;
    float cent_max_y = 3;      //16



    private void OnEnable()
    {
        GameManager.KeepPos += KeeperMove;
    }

    private void OnDisable()
    {
        GameManager.KeepPos -= KeeperMove;
    }

    void Start()
    {
        //animator = gameObject.GetComponentInParent<Animator>();
        position = new Vector3(0, 0, 51.15f);
        rotation = new Quaternion(0, 180, 0, 0);
    }

    void Reset()
    {
        //transform.root.position = position;
        //transform.root.rotation = rotation;
        keeper.position = position;
        keeper.rotation = rotation;
        keeperAction = false;
    }

    /// <summary>
    /// by준희, 슛방향에 따른 키퍼의 움직임 제어
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void KeeperMove(float x, float y)
    {
        if (keeperAction == false)
        {
            if (x < cent_min_x && y > cent_max_y)
            {
                x = -1; y = 1;
            }
            else if (x > cent_min_x && x < cent_max_x && y > cent_max_y)
            {
                x = 0; y = 1;
            }
            else if (x > cent_max_x && y > cent_max_y)
            {
                x = 1; y = 1;
            }
            ///////////////////////////////////////

            else if (x < cent_min_x && y > cent_min_y && y < cent_max_y)
            {
                x = -1; y = 0;
            }
            else if (x > cent_min_x && x < cent_max_x && y > cent_min_y && y < cent_max_y)
            {
                x = 0; y = 0;
            }
            else if (x > cent_max_x && y > cent_min_y && y < cent_max_y)
            {
                x = 1; y = -1;
            }
            /////////////////////////////////////

            else if (x < cent_min_x && y < cent_min_y)
            {
                x = -1; y = -1;
            }
            else if (x > cent_min_x && x < cent_max_x && y < cent_min_y)
            {
                x = 0; y = -1;
            }
            else if (x > cent_max_x && y < cent_min_y)
            {
                x = 1; y = -1;
            }




            animator.SetFloat("Pos_X", x);
            animator.SetFloat("Pos_Y", y);
            Debug.LogFormat("x: {0}, y: {1}", x, y);
            animator.SetTrigger("IsGuard");
            keeperAction = true;

            // Debug.Log(localPosition.x + localPosition.y);

            Invoke("Reset", 5f);

        }
    }
}
