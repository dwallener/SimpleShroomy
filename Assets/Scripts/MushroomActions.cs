using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomActions : MonoBehaviour
{

    public float _force = 0f;
    public float _radius = 2f;
    public int _cubesPerAxis = 4;

    public AudioSource _asSFX1;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
        _force = 50000f;
#endif

#if UNITY_EDITOR
        _force = 5000f;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pawn")
        {
            // make sound
            GameManager.Instance._asSFX1.Play(0);

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
                    Debug.Log("create gameobject");
                    var _thisgo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _thisgo.name = "ExplodingSphere";

                    Debug.Log("Get renderer component");
                    _thisgo.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    // xcode reporting that SphereCollider doesn't exist! Argh...

                    Debug.Log("Add spherecollider if necessary");
                    if (_thisgo.GetComponent<SphereCollider>() == null)
                    {
                        _thisgo.gameObject.AddComponent<SphereCollider>();
                        //_thisgo.AddComponent<SphereCollider>();
                    }

                    Debug.Log("Add rigidbody");
                    _thisgo.AddComponent<Rigidbody>();
                    Debug.Log("clear gravity flag");
                    _thisgo.GetComponent<Rigidbody>().useGravity = false;

                    //_thisgo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    _thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0f, 0f, 0f), Time.deltaTime * 20);

                    // position is guaranteed to be close to Pawn position
#if UNITY_IOS
                    _thisgo.transform.position = new Vector3(0f + Random.Range(0f,0.2f), 0f + Random.Range(0f, 0.2f), -3.2f + Random.Range(0f, 0.2f));
#endif
#if UNITY_EDITOR
                    _thisgo.transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
#endif
                    _thisgo.GetComponent<Rigidbody>().AddExplosionForce(_force, new Vector3(0f, 0f, -2.7f), _radius);
                    _thisgo.AddComponent<ShroomParticleActions>();

                    Destroy(_thisgo, 3f);
                }
            }
        }
    }

}
