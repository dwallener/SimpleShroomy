using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f, 0f, -2.7f);
        // let's come back to this...
        //RaycastHit _hit;
        //Physics.Raycast(transform.position, (Vector3.zero - transform.position), out _hit);
        
    }
}
