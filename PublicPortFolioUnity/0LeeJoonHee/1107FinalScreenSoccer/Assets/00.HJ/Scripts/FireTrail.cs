using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    Transform fireTrailTr;
    // Start is called before the first frame update
    void Start()
    {
        fireTrailTr = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fireTrailTr != null)
        {
            fireTrailTr.position = new Vector3(fireTrailTr.position.x, 0, fireTrailTr.position.z);
        }
    }
}
