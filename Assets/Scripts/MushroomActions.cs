using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomActions : MonoBehaviour
{

    public float _force = 0f;
    public float _radius = 2f;
    public int _cubesPerAxis = 4;
    public string _shroomPrefab;

    GameObject _thisgo;

    // Start is called before the first frame update
    void Start()
    {
        _force = 1000f;
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


    // CreatePrimitive is the problem here...it's getting stripped at build time
    // solution is to "declare private properties of these types."

    public void MakeBang()
    {
        for (int x = 0; x < _cubesPerAxis; x++)
        {
            for (int y = 0; y < _cubesPerAxis; y++)
            {
                for (int z = 0; z < _cubesPerAxis; z++)
                {
                    // confetti time!

                    int _shroomIndex = (x + 1) * (y + 1) * (z + 1) % 3;
                    switch (_shroomIndex)
                    {
                        case 0:
                            _shroomPrefab = "Prefabs/pinkMushroom";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        case 1:
                            _shroomPrefab = "Prefabs/blueMushroom";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        case 2:
                            _shroomPrefab = "Prefabs/greenMushroom";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        default:
                            _shroomPrefab = "Prefabs/pinkMushroom";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                    }

                    _thisgo.name = "ExplodingSphere";

                    _thisgo.AddComponent<Rigidbody>();
                    _thisgo.GetComponent<Rigidbody>().useGravity = false;
                    _thisgo.GetComponent<Rigidbody>().mass = 10f;
                    _thisgo.AddComponent<GravityBody>();

                    //_thisgo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    _thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0f, 0f, 0f), Time.deltaTime * 20);

                    // position is guaranteed to be close to Pawn position
                    //_thisgo.transform.position = new Vector3(0f + Random.Range(0f,0.2f), 0f + Random.Range(0f, 0.2f), -3.2f + Random.Range(0f, 0.2f));
                    _thisgo.transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
                    _thisgo.GetComponent<Rigidbody>().AddExplosionForce(_force, new Vector3(0f, 0f, -2.7f), _radius);
                    _thisgo.AddComponent<ShroomParticleActions>();

                    Destroy(_thisgo, 3f);
                }
            }
        }
    }

}
