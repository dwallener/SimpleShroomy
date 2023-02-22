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
            if ((_hit.distance < 0.11f) && (_rb != null))
            {
                //Debug.Log("Explode!");
                GameManager.Instance._isExplode = true;
                // blow him up!!!
                // power, origin, radius
                GetComponent<Rigidbody>().AddExplosionForce(2000f, new Vector3(Random.Range(0f,1f),Random.Range(0f,1f), 0f), 5f);
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
