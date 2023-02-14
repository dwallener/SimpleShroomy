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
        // this shouldn't be needed and will probably fuck with physics
        transform.position = new Vector3(0f, 0f, -2.5f);
        //transform.forward = GameManager.Instance._spinDir;
        //Vector3 _rotate = new Vector3(0f, 0f, GameManager.Instance._spinAngle);
        //transform.rotation = Quaternion.LookRotation(GameManager.Instance._spinDir);
    }

}
