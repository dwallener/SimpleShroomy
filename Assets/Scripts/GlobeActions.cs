using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeActions : MonoBehaviour
{

    private Vector3 speed = new Vector3();
    private Vector3 avgSpeed = new Vector3();
    private bool dragging = false;
    //private Vector3 targetSpeedX = new Vector3();

    private float rotationSpeed = 10.0f;
    private float lerpSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) && dragging)
        {
            speed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
            avgSpeed = Vector3.Lerp(avgSpeed, speed, Time.deltaTime * 5);
        }
        else
        {
            if (dragging)
            {
                speed = avgSpeed;
                dragging = false;
            }
            var i = Time.deltaTime * lerpSpeed;
            speed = Vector3.Lerp(speed, Vector3.zero, i);
        }

        transform.Rotate(Camera.main.transform.up * speed.x * rotationSpeed, Space.World);
        transform.Rotate(Camera.main.transform.right * speed.y * rotationSpeed, Space.World);

        GameManager.Instance._spinDir = new Vector3 (-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        GameManager.Instance._spinAngle = Mathf.Atan2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

    }

    private void OnMouseDown()
    {
        dragging = true;
    }


}
