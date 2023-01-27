
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    // for the Canvas objects
    public GameObject _curtain; // for fadeouts
    public GameObject _message;
    public GameObject _score;
    public GameObject _goal;
    public GameObject _level;


    public GameObject _globe;
 
    // Let's think this through
    // Player level data and level requirements are persistent-stored in GameState
    // So in theory, GameManager doesn't need any persistence at all
    // just read level, level requirements, and count

    private void Awake()
    {

        _curtain = GameObject.Find("curtainBlack");

        _message = GameObject.Find("message");
        _score = GameObject.Find("score");
        _goal = GameObject.Find("goal");
        _level = GameObject.Find("level");

        // let's instantiate a planet from the prefab list
        int _planetIndex = Random.Range(0,GameState._prefabList.Length);
        _globe = Instantiate(Resources.Load(GameState._prefabList[_planetIndex], typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;

        _globe.AddComponent<GravityAttractor>();
        _globe.AddComponent<GlobeActions>();
        _globe.AddComponent<SphereCollider>();
        _globe.transform.position = Vector3.zero;
        _globe.transform.localScale = new Vector3(5f, 5f, 5f);

        // this was the naked sphere way
        //_globe = GameObject.Find("Globe");

    }

    // Start is called before the first frame update
    void Start()
    {
        // make the planet!
        NewPlanet(GameState._level);
        // fill in the HUD!
        _message.GetComponent<TextMeshProUGUI>().text = "Collect Shroomies!";
        _score.GetComponent<TextMeshProUGUI>().text = "Score: " + 0.ToString() + " of ";
        _goal.GetComponent<TextMeshProUGUI>().text = GameState._levelGoals[GameState._level].ToString();
        _level.GetComponent<TextMeshProUGUI>().text = "LEVEL " + GameState._level.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        // this is just to debug scene management
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameState._level++;
            Debug.Log("Level: " + GameState._level);
            StartCoroutine(LoadNextScene());
        }

    }

    public IEnumerator LoadNextScene()
    {
        // wait for fade
        yield return FadeOut(3);

        SceneManager.UnloadScene(GameState._level - 1);
        SceneManager.LoadScene(GameState._level);
        yield return null;
    }


    public IEnumerator FadeOut(int _fadeSpeed)
    {
        Color _fadeColor = _curtain.GetComponent<Image>().color;
        float _fadeAmount;

        while (_curtain.GetComponent<Image>().color.a < 1)
        {
            _fadeAmount = _fadeColor.a + (_fadeSpeed * Time.deltaTime);
            _fadeColor = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _fadeAmount);
            _curtain.GetComponent<Image>().color = _fadeColor;

            yield return null;
        }

        //yield return new WaitForEndOfFrame();
    }

    public void NewPlanet(int _level)
    {
        Debug.Log("New Planet");
        switch (_level)
        {
            case 0: Planet00(); break;
            case 1: Planet01(); break;
            case 2: Planet02(); break;
            case 3: Planet03(); break;
            case 4: Planet04(); break;
            default:
                break;
        }
    }

    public void Planet00()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Prefabs/Shroom1", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
        }
    }

    public void Planet01()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            _rng = Random.onUnitSphere * (float)Random.Range(6,11);
            _mushroom[i] = Instantiate(Resources.Load("Prefabs/Shroom2", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
        }
    }

    public void Planet02()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Prefabs/Shroom3", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
        }
    }

    public void Planet03()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Prefabs/Shroom4", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
        }
    }

    public void Planet04()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Prefabs/Shroom1", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
        }
    }

}
