using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MushroomActions : MonoBehaviour
{
    // shroomie stuff
    public float _force = 0f;
    public float _radius = 2f;
    public int _cubesPerAxis = 4;
    public string _shroomPrefab;
    public string _flowerSprite;

    // local go
    GameObject _thisgo;

    // postprocessing thingies
    public PostProcessVolume _volume;
    public Bloom _bloom = null;
    public ChromaticAberration _chromAb = null;

    // flower interval things
    private float _intervalTime = 1f;
    private float _intervalTimer;
    private int _flowerCounter = 60;


    // Start is called before the first frame update
    void Start()
    {
        _force = 1000f;

        // amazingly this seems to work!
        // cache references for postprocessing
        _volume = GameObject.FindWithTag("pp").GetComponent<PostProcessVolume>();
        //Debug.Log("ppVolume: " + _volume);
        if (!_volume.profile.TryGetSettings(out _bloom))
        {
            Debug.Log("Can't find bloom");
        }
        if (!_volume.profile.TryGetSettings(out _chromAb))
        {
            Debug.Log("Can't find ChromAb");
        }
        _volume.profile.TryGetSettings(out _bloom);
        _volume.profile.TryGetSettings(out _chromAb);

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnCollisionEnter(Collision collision)
    private IEnumerator OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pawn")
        {
            // make sound
            GameManager.Instance._asSFX1.Play(0);

            // update score
            GameManager.Instance._playerScore++;
            Debug.Log("Help! I've Fallen And I Can't Get Up!");
            if (this.gameObject.name == "BadShroomie")
            {
                //MakeFlowerBang();
                //MakeFlower();
                //InvokeRepeating("MakeFlower", 0f, 0.5f);
                //StartCoroutine(MakeFlowers(300));
                for (int i = 0; i < 20; i++)
                {
                    MakeFlower();
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else
            {
                MakeBang();
            }
            Destroy(gameObject);

            // if we are the bad shroomie, kick of the visual effects
            if (this.gameObject.name == "BadShroomie")
            {
                Debug.Log("Bad Shroomie! Bad Trip!");
                // find teh volume/component
                // Turn off for now...
                //_bloom.enabled.value = true;
                //_chromAb.enabled.value = true;
            }
        }
    }

    IEnumerator MakeFlowers(int _numFlowers)
    {
        int i = 0;
        //while (i < _numFlowers)
        while (true)
        {
            i++;
            //MakeFlower();
            var _flowerSprite = "Prefabs/flowerPower02";
            _thisgo = Instantiate(Resources.Load(_flowerSprite, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
            /*
            _thisgo.name = "HappyFlower";

            _thisgo.AddComponent<Rigidbody>();
            _thisgo.GetComponent<Rigidbody>().useGravity = false;
            _thisgo.GetComponent<Rigidbody>().mass = 1000000f;
            _thisgo.AddComponent<GravityBody>();

            //_thisgo.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            _thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0f, 0f, 0f), Time.deltaTime * 20);

            // position is guaranteed to be close to Pawn position
            //_thisgo.transform.position = new Vector3(0f + Random.Range(0f,0.2f), 0f + Random.Range(0f, 0.2f), -3.2f + Random.Range(0f, 0.2f));
            _thisgo.transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
            _thisgo.transform.rotation = Random.rotation;
            //_thisgo.GetComponent<Rigidbody>().AddExplosionForce(_force / 1000f, new Vector3(0f, 0f, -2.7f), _radius);
            _thisgo.GetComponent<Rigidbody>().AddForce(1f * new Vector3(0f, 0f, -2.7f), ForceMode.Impulse);
            //_thisgo.AddComponent<ShroomParticleActions>();

            //Destroy(_thisgo, 3f);
            */
            yield return new WaitForSeconds(1f);
        }
        //yield return 0;
    }



    public void MakeFlower()
    {
        // kill it if we're done
        //if (--_flowerCounter == 0) { CancelInvoke("MakeFlower"); _flowerCounter = 60; Debug.Log("Kill Invoke"); }
        //else { Debug.LogFormat("Make another flower {0}", _flowerCounter); }

        var _flowerSprite = "Prefabs/flowerPower02";
        _thisgo = Instantiate(Resources.Load(_flowerSprite, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;

        _thisgo.name = "HappyFlower";

        _thisgo.AddComponent<Rigidbody>();
        _thisgo.GetComponent<Rigidbody>().useGravity = false;
        _thisgo.GetComponent<Rigidbody>().mass = 1000000f;
        _thisgo.AddComponent<GravityBody>();
        _thisgo.AddComponent<HappyFlowerActions>();

        //_thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, 0f, 0f), Time.deltaTime * 10);

        // position is guaranteed to be close to Pawn position
        //_thisgo.transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
        //_thisgo.transform.rotation = Random.rotation;
        _thisgo.GetComponent<Rigidbody>().AddForce(1f * new Vector3(0f, 0f, -2.7f), ForceMode.Impulse);

        Destroy(_thisgo, 3f);

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

    public void MakeFlowerBang()
    {
        float _intervalTimer = _intervalTime;

        while (_flowerCounter > 0)
        {
            Debug.LogFormat("Flower# {0} Countdown {1}", _flowerCounter, _intervalTimer); 
            _intervalTimer -= Time.deltaTime;
            if (_intervalTimer <= 0f)
            {
                _intervalTimer = _intervalTime;
                _flowerCounter--;
                MakeFlower();
            }
        }

        /*
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
                            _shroomPrefab = "Prefabs/flowerPower02";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        case 1:
                            _shroomPrefab = "Prefabs/flowerPower02";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        case 2:
                            _shroomPrefab = "Prefabs/flowerPower02";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                        default:
                            _shroomPrefab = "Prefabs/flowerPower02";
                            _thisgo = Instantiate(Resources.Load(
                                _shroomPrefab, typeof(GameObject)), new Vector3(0f + Random.Range(0.2f, 0.5f), 0f + Random.Range(0.2f, 0.5f), -2.7f + Random.Range(0.2f, 0.5f)), Quaternion.identity) as GameObject;
                            break;
                    }

                    _thisgo.name = "ExplodingFlower";

                    _thisgo.AddComponent<Rigidbody>();
                    _thisgo.GetComponent<Rigidbody>().useGravity = false;
                    _thisgo.GetComponent<Rigidbody>().mass = 1000f;
                    _thisgo.AddComponent<GravityBody>();

                    _thisgo.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0f, 0f, 0f), Time.deltaTime * 20);

                    // position is guaranteed to be close to Pawn position
                    _thisgo.transform.position = new Vector3(0f + Random.Range(0f, 0.2f), 0f + Random.Range(0f, 0.2f), -2.7f + Random.Range(0f, 0.2f));
                    _thisgo.transform.rotation = Random.rotation;
                    _thisgo.GetComponent<Rigidbody>().AddForce(_force/100f * new Vector3(0f, 0f, -2.7f));
                    _thisgo.AddComponent<ShroomParticleActions>();

                    Destroy(_thisgo, 3f);
                }
            }
        }
        */
    }
}
