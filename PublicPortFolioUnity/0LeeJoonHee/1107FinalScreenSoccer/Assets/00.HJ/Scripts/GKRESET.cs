using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKRESET : MonoBehaviour
{
    Transform gkTr;
    Quaternion gkQt;
    // Start is called before the first frame update
    void Start()
    {
        gkTr = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    public void GKReset()
    {
        gkTr.position = new Vector3(0,0, 51.15f);
        gkTr.rotation = new Quaternion(0,180,0,0);
    }
}
