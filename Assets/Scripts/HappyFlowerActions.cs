using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyFlowerActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
        transform.localScale = Vector3.Lerp(new Vector3(0.05f, 0.05f, 0.05f), new Vector3(0f, 0f, 0f), Time.deltaTime * 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Random.rotation;
    }
}
