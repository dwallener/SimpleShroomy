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
            // -90 on x puts it upright
            // -45 on z puts it upright and looking left, up
            // +45 on z puts it upright and looking right, down
            // -45 on y puts it upright and looking left, up
            // +45 on y puts it upright and looking right, down
            // -45 on y,z puts it on its back looking up
            // +45 on y,z puts it face down
            // -45 on x,z puts it looking NW    <--
            // +45 on x,z puts it on its side looking SW
            //transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

            /* all these have problems
             * 
            // this rotates ShroomieKing on correct access...now to figure out correct direction
            //var _newRot = new Vector3(0f, 1f, 0f);
            //var _angle = Mathf.Atan2(GameManager.Instance._spinDir.y, GameManager.Instance._spinDir.x) * Mathf.Rad2Deg;
            //Debug.Log("Angle: " + _angle);
            //var _newRot = new Vector3(0f, _angle, 0f);

            //var _lookPos = new Vector3(100f * GameManager.Instance._spinDir.x, 100f * GameManager.Instance._spinDir.y, -2.7f);
            //Debug.Log("LookAt: " + _lookPos);

            //Quaternion _lookRot = Quaternion.LookRotation(_lookPos - transform.position);
            //transform.rotation = _lookRot;

            // transform.LookAt(_lookPos); // rotates on all axis

            //transform.Rotate(_newRot);
            //transform.LookAt(new Vector3(GameManager.Instance._spinDir.x, GameManager.Instance._spinDir.y, -2.7f));

            *
            */
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
                GetComponent<Rigidbody>().AddExplosionForce(4000f, new Vector3(Random.Range(0f,1f),Random.Range(0f,1f), 0f), 5f);
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
