using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperAniCtrl : MonoBehaviour
{
    public Transform ball;
    public Animator animator;
    public Transform keeper;
    Vector3 position;
    Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        ball = gameObject.transform;
        position = new Vector3(0, 0, 51.15f);
        rotation = new Quaternion(0, 180, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetFloat("Pos_X", 1);
        animator.SetFloat("Pos_Y", 1);
        animator.SetTrigger("IsGuard");
        Debug.LogFormat("X값:{0}  Y값:{1} ", ball.position.x, ball.position.y);
        Invoke("Reset", 5f);
    }
    void Reset()
    {
        keeper.root.position = position;
        keeper.root.rotation = rotation;
        //keeperAction = false;
    }

}
