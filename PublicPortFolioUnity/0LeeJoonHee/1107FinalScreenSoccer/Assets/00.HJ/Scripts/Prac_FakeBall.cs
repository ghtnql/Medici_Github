using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prac_FakeBall : MonoBehaviour
{
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Prac_SoccerPlayer.isShoot == true)
        {
            meshRenderer.enabled = false;
        }
        else if (Prac_SoccerPlayer.isShoot == false)
        {
            meshRenderer.enabled = true;
        }
    }
}
