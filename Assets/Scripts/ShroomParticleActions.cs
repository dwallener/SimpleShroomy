using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomParticleActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // all we want to do is shrink

        var _newScale = transform.localScale * 0.98f;
        transform.localScale = _newScale;

        // and metallisize
    }

}
