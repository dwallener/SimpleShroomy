using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomActions : MonoBehaviour
{

    public float _force = 100f;
    public float _radius = 2f;
    public int _cubesPerAxis = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pawn")
        {
            // update score
            GameManager.Instance._playerScore++;
            Debug.Log("Help! I've Fallen And I Can't Get Up!");
            MakeBang();
            Destroy(gameObject);
        }
    }

    public void MakeBang()
    {
        for (int x = 0; x < _cubesPerAxis; x++)
        {
            for (int y = 0; y < _cubesPerAxis; y++)
            {
                for (int z = 0; z < _cubesPerAxis; z++)
                {
                    // confetti time!
                    var _thisgo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _thisgo.name = "ExplodingSphere";
                    _thisgo.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    _thisgo.AddComponent<Rigidbody>();
                    _thisgo.GetComponent<Rigidbody>().useGravity = false;

                    //_thisgo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    _thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, 0f, 0f), Time.deltaTime * 20);

                    // position is guaranteed to be close to Pawn position
                    _thisgo.transform.position = new Vector3(0f + Random.Range(0f,0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
                    _thisgo.GetComponent<Rigidbody>().AddExplosionForce(_force, new Vector3(0f, 0f, 2.7f), _radius);
                    _thisgo.AddComponent<ShroomParticleActions>();

                    Destroy(_thisgo, 3f);
                }
            }
        }
    }

}
