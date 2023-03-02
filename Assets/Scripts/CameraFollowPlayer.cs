using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    // standard camera offsets
    public float _x = 6.0f;
    public float _y = 6.0f;
    public float _z = -8f;
    public Vector3 _lookat = new Vector3(0f, 0f, -2.5f);

    // Start is called before the first frame update
    void Start()
    {
        // Pawn is at 0,0,-2.7, starts by facing north
        //transform.position = new Vector3(0f, -3f, -3.5f);
        //transform.LookAt(new Vector3(0f, 0f, -3f), new Vector3(0f, 0f, 1f));
        //transform.Rotate(0f, 0f, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Going to need this to position the camera in behind the little guy
         *             
        */

        // with atan2 angles are <-- -90  ^^ 0 vv 180 +90 -->
            var _lookAngle = Mathf.Ceil( Mathf.Atan2(GameManager.Instance._spinDir.x, GameManager.Instance._spinDir.y) * 180f / Mathf.PI);

        // look dir is opposite of spin dir!
        switch (_lookAngle)
        {
            // NN
            case < 22 and > -22:
                Debug.Log("Direction: N");
                //transform.localRotation = Quaternion.Euler(0, -90, 135);
                transform.position = new Vector3(0f, _y, _z);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                transform.Rotate(0f, 0f, 180f);
                break;
            // NE
            case < -22 and > (-90 + 22):
                Debug.Log("Direction: NE");
                //transform.localRotation = Quaternion.Euler(-45, -90, 135);
                transform.position = new Vector3(-_x, -_y, _z);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                break;
            // EE
            case > (-90 - 22) and < (-90 + 22):
                Debug.Log("Direction: E");
                transform.position = new Vector3(-_x, 0f, _z);
                //transform.localRotation = Quaternion.Euler(-90, -90, 135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                transform.Rotate(0f, 0f, -90f);
                break;
            // SE
            case < (-90 - 22) and > (-180 + 22):
                Debug.Log("Direction: SE");
                transform.position = new Vector3(-_x, _y, _z);
                //transform.localRotation = Quaternion.Euler(90, 45, -135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                break;
            // SS - special case...less than last negative bound, greater than last positive bound (180 is 180)
            case > (180 - 22) or < (-180 + 22):
                Debug.Log("Direction: S");
                transform.position = new Vector3(0f, -_y, _z);
                //transform.localRotation = Quaternion.Euler(0, 90, -135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                break;
            // SW
            case > (135 - 22) and < (135 + 22):
                Debug.Log("Direction: SW");
                transform.position = new Vector3(_x, _y, _z);
                //transform.localRotation = Quaternion.Euler(45, 90, -135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                break;
            // WW
            case > (90 - 22) and < (90 + 22):
                transform.position = new Vector3(_x, 0f, _z);
                //transform.localRotation = Quaternion.Euler(90, 90, -135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                Debug.Log("Direction: W");
                break;
            // NW
            case > 22 and < (45 + 22):
                transform.position = new Vector3(_x, -_y, _z);
                //transform.localRotation = Quaternion.Euler(-45, 90, -135);
                //transform.LookAt(GameManager.Instance._pawn.transform);
                transform.LookAt(_lookat);
                Debug.Log("Direction: NW");
                break;
        }

    }

}
