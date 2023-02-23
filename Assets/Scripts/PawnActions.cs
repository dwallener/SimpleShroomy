using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnActions : MonoBehaviour
{

    MeshCollider _mc;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _mc = GameManager.Instance._globe.GetComponent<MeshCollider>();
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        // want different behavior for lava and not-lava

        if (!GameManager.Instance._isExplode)
        {
            // this shouldn't be needed and will probably fuck with physics
            transform.position = new Vector3(0f, 0f, -2.5f);

            // z: +45 faces NW, on its side
            // z: -45 faces SW, on its side

            // z: 135 faces SE but upside down
            // z: -135 faces NE but upside down
            // y: 90, z: -45 faces N, perfectly
            // y: 90, z: 0 faces camera, perfectly
            // x: 90, z:135 faces E, perfectly <---
            // x: 45, y: 45, z:-135 faces NE <---
            // y: 90, z: -90 faces NE
            // y: -90, z: 90 faces SW
            // y: -90, z: 135 faces S, perfectly   <---
            // y: 90, z: -45 faces N

            // perfectly - HOLY CRAP was that a lot of work!

            // y: 90, z: -135 faces N
            // x: 45, y: 90, z: -135 faces NE ... 90deg W is -45, 90, -135
            // x: 90, y: 90, z:-135 faces E
            // x: 90, y: -45, z: 135 faces SE
            // y: -90 z: 135 faces S
            // x: -45, y: -90, z:135 faces SW
            // x: -90, y: -90, z:135 faces W 
            // x: 90, y: 45, z: -135 faces NW ... also -45 90 -135

            //transform.localRotation = Quaternion.Euler(-45, 90, -135);

        }
        else
        {
            // tumble!
            var _newRot = new Vector3(0.5f, 0.5f, 0.5f);
            transform.Rotate(_newRot);
        }

        // this works for tracking elevation to surface!
        RaycastHit _hit;
        Physics.Raycast(new Vector3(0f, 0f, -2.7f), new Vector3(0f, 0f, 1f), out _hit);
        //Debug.Log("Hit distance: " + _hit.distance);

        if (GameManager.Instance._isLava)
        {
            if ((_hit.distance < 0.15f) && (_rb != null))
            {
                //Debug.Log("Explode!");
                GameManager.Instance._isExplode = true;
                // blow him up!!!
                // power, origin, radius
                GetComponent<Rigidbody>().AddExplosionForce(8000f, new Vector3(Random.Range(0f,1f),Random.Range(0f,1f), 0f), 5f);
            }
        }
        else
        {
            // nothing
        }

        // come back to rotations later

        //transform.forward = GameManager.Instance._spinDir;
        //Vector3 _rotate = new Vector3(0f, 0f, GameManager.Instance._spinAngle);
        //transform.rotation = Quaternion.LookRotation(GameManager.Instance._spinDir);
    }

}
