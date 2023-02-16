
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour
{

    // for the Canvas objects
    public GameObject _curtain; // for fadeouts

    public GameObject _message;
    public TextMeshProUGUI _messageText;

    public GameObject _score;
    public TextMeshProUGUI _scoreText;

    public GameObject _goal;
    public GameObject _level;

    public GameObject _timer;
    public TextMeshProUGUI _timerText;
    public GameObject _panel;

    // main gameObjects
    public GameObject _globe;
    public GameObject _pawn; // let's go with Unreal terminology, lol
    public GameObject _pawnExhaust; // the thing to spew from

    // Audio thingies
    public GameObject _audioSource;
    public AudioSource _audio;

    // player thingies
    public int _playerScore = 0;

    // timer thingies
    public int _countDown = 0;
    public float _countDownf = 0f;

    // bookkeeping thingies
    //public Quaternion _spinDir;
    public Vector3 _spinDir = Vector3.zero;
    public float _spinAngle = 0f;

    /// <summary>
    /// Constructor singleton
    /// </summary>
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("GameManager is NULL!!!");
            return _instance;
        }
    }

    // Let's think this through
    // Player level data and level requirements are persistent-stored in GameState
    // So in theory, GameManager doesn't need any persistence at all
    // just read level, level requirements, and count

    private void Awake()
    {
        // manage player prefs
        if (PlayerPrefs.HasKey("Level"))
        {
            GameState._level = PlayerPrefs.GetInt("Level");
            Debug.LogFormat("Prefs exist, Level {0}", GameState._level);
        }
   
        _instance = this;

        // set up the HUD objects
        _curtain = GameObject.Find("curtainBlack");

        _message = GameObject.Find("message");
        _messageText = _message.GetComponent<TextMeshProUGUI>();

        _score = GameObject.Find("score");
        _scoreText = _score.GetComponent<TextMeshProUGUI>();

        _goal = GameObject.Find("goal");
        _level = GameObject.Find("level");

        _timer = GameObject.Find("timer");
        _panel = GameObject.Find("panel");
        _timerText = _timer.GetComponent<TextMeshProUGUI>();

        // PLANET

        //int _planetIndex = Random.Range(0, GameState._prefabList.Length);
        int _planetIndex = Random.Range(0, 40);

        // choose between lava and not lava
        string _levelType = GameState._levelType[GameState._level % 12];

        if (_levelType.Contains("Lava"))
        {
            // structure is 40 normal worlds, 40 lava equivalents
            _globe = Instantiate(Resources.Load(
                GameState._prefabList[_planetIndex + 40], typeof(GameObject)), Vector3.zero, Quaternion.identity)
                as GameObject;
        }
        else
        {
            _globe = Instantiate(Resources.Load(
                GameState._prefabList[_planetIndex], typeof(GameObject)), Vector3.zero, Quaternion.identity)
                as GameObject;
        }
        Debug.LogFormat("Planet: {0}", GameState._prefabList[_planetIndex]);

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = GameState._skyboxList[_planetIndex];

        // set up our own gravity
        _globe.AddComponent<GravityAttractor>();
        _globe.AddComponent<GlobeActions>();
        _globe.AddComponent<Rigidbody>();
        _globe.GetComponent<Rigidbody>().useGravity = false;
        _globe.GetComponent<Rigidbody>().isKinematic = true;

        // sphere collider sucks, can we do a mesh collider?
        //_globe.AddComponent<SphereCollider>();
        // this almost works! and I think we're well under the 255 triangles limit
        // nope - over the limit
        MeshCollider _mc = _globe.AddComponent<MeshCollider>();
        _mc.convex = true;

        _globe.transform.position = Vector3.zero;
        _globe.transform.localScale = new Vector3(5f, 5f, 5f);
        _globe.tag = "Planet";

        //PlayBackgroundMusic();

        // PAWN
        GameObject _pawn = new GameObject();

        // let's try wrapping a texture around the capsule
        //_pawn = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // load a model prefab - made a variant with the skin etc
        _pawn = Instantiate(Resources.Load<GameObject>("Prefabs/Pawns/Pawn"), new Vector3(0f, 0f, -2.5f), Quaternion.identity);

        // set up location, physics, blah blah blah
        //_pawn.transform.position = new Vector3(0f, 0f, -2.7f);
        _pawn.transform.Rotate(0, 180, 0);
        _pawn.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        BoxCollider _bc = _pawn.AddComponent<BoxCollider>();
        // tallify the box in Z - tweak this again in #scene
        _bc.size = new Vector3(3f, 3f, 3f);

        // give it it's own intelligence
        _pawn.AddComponent<PawnActions>();
        _pawn.name = "Pawn";

        // use planet gravity attractor
        _pawn.AddComponent<GravityBody>();

        // pawn is colliding with shrooms successfully, but not with planet.
        Rigidbody _rb = _pawn.GetComponent<Rigidbody>();

        _rb.mass = 10000f;
        // turn off normal gravity
        _rb.useGravity = false;
        _rb.centerOfMass = new Vector3(0f, 0f, -0.5f);

    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        // always reset, early often late, always
        _playerScore = 0;
      
        // make the planet!
        NewPlanet(GameState._level);

        // fill in the HUD!
        _message.GetComponent<TextMeshProUGUI>().text = "Collect Shroomies!";
        _messageText.text = "Collect Shroomies!";

        //_score.GetComponent<TextMeshProUGUI>().text = "Score: " + 0.ToString() + " of ";
        _scoreText.text = "Score: " + _playerScore.ToString() + " of ";

        _goal.GetComponent<TextMeshProUGUI>().text = GameState._levelGoals[GameState._level / 5].ToString();
        _level.GetComponent<TextMeshProUGUI>().text = "LEVEL " + GameState._level.ToString();

        string _levelType = GameState._levelType[GameState._level % 12];

        // set timer if TT level
        if (_levelType.Contains("TT"))
        {
            _countDown = GameState._levelTimers[GameState._level / 6];
            _countDownf = (float)_countDown;
            _timer.SetActive(true);
        }
        else
        {
            _timer.SetActive(false);
        }
        // set up lava worlds (currently nothing)
        if (_levelType.Contains("Lava"))
        {

        }
        else
        {

        }

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // update the timer
        _countDownf -= Time.deltaTime;
        
        // this is just to debug scene management
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameState._level++;
            Debug.Log("Level: " + GameState._level);
            StartCoroutine(LoadNextScene());
        }

        // get level end conditions
        string _levelType = GameState._levelType[GameState._level % 6];

        // bucket the level types
        if (_levelType == "Collection")
        {

            _messageText.text = string.Format("Collect {0} Shroomies!", GameState._levelGoals[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }
        else if (_levelType == "Collection TT")
        {
            _messageText.text = string.Format("Collect {0} Shroomies in {1} seconds!",
                GameState._levelGoals[GameState._level / 5], GameState._levelTimers[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            // this needs timer check as well
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }
        else if (_levelType == "Find")
        {
            _messageText.text = string.Format("Find the magic Shroomie!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }

        if (_levelType == "Find TT")
        {
            _messageText.text = string.Format("Find the magic Shroomie in {0} seconds!",
                GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }

        if (_levelType == "Clearcut")
        {
            _messageText.text = string.Format("Collect all the shroomies!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }

        if (_levelType == "Clearcut TT")
        {
            _messageText.text = string.Format("Collect all the shroomies in {0} seconds!",
               GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                AdvanceLevel();
                return;
            }
        }

    }

    /// <summary>7
    /// We'll want to encapsulate some of the gameplay tracking above into here
    /// </summary>
    public void PlayLevel()
    {

    }


    // move to the next level
    /// <summary>
    /// One place to advance to the next level
    /// </summary>
    public void AdvanceLevel()
    {
        GameState._level++;
        PlayerPrefs.SetInt("Level", GameState._level);
        Debug.Log("Level: " + GameState._level);
        StartCoroutine(LoadNextScene());
    }

    /// <summary>
    /// Manage scene transistions
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadNextScene()
    {
        // wait for fade
        yield return FadeOut(3);

        // if we've done the level reading/randomization correctly
        // we should now only ever need one scene...?
        //SceneManager.LoadScene(GameState._level);
        //SceneManager.UnloadScene(GameState._level - 1);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(0);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

        yield return null;
    }

    /// <summary>
    /// Simple fade out for screen transitions
    /// </summary>
    /// <param name="_fadeSpeed"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Entry point for creating new planet
    /// </summary>
    /// <param name="_level"></param>
    public void NewPlanet(int _level)
    {
        Debug.Log("New Planet");
        //AddColliders();
        CreatePlanet(); 
    }

    /// <summary>
    /// Add box colliders to topographical features
    /// </summary>
    public void AddColliders()
    {
        // walk the mesh, add box collider wherever radius is bigger than some number
        Mesh _mesh = _globe.GetComponent<MeshFilter>().mesh;
        foreach (Vector3 _vertex in _mesh.vertices)
        {
            // dump the radius
            var _radius = Vector3.Distance(_vertex, Vector3.zero);
            Debug.LogFormat("Radius: {0} @ {1}, {2}, {3}", _radius, _vertex.x, _vertex.y, _vertex.z);
            if (_radius > 0.46)
            {
                Debug.Log("Adding box collider");
                var _bc = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _bc.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                _bc.transform.SetParent(_globe.transform);
                _bc.transform.position = _vertex * 5f;
                //StartCoroutine(ResetPosition(_bc, _vertex));
            }
        }
    }

    private IEnumerator ResetPosition(GameObject _go, Vector3 _v)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        _go.transform.position = _v;
    }

    /// <summary>
    /// Populate the new planet with baddies and whatnot
    /// </summary>
    public void CreatePlanet()
    {

        int _level = GameState._level; // less typing :)
        int _rngLevelMod5 = Random.Range(0, GameState._levelGoals.Length);
        int _rngNumShrooms = Random.Range(25, 76); // int Random is not inclusive
        float _fallHeight = Random.Range(5f, 11f);
        int _rngShroomIndex = Random.Range(1, 5);
        Vector3 _rng;

        string _shroomPrefab = "Prefabs/Shroom" + _rngShroomIndex;
        Debug.Log("Shroom Prefab: " + _shroomPrefab);

        GameObject[] _mushroom = new GameObject[_rngNumShrooms];

        // have some objects drop down on it
        for (int i = 0; i < _rngNumShrooms; i++)
        {
            _rng = Random.onUnitSphere * _fallHeight;
            //Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load(_shroomPrefab, typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>();
            _mushroom[i].transform.parent = _globe.transform;
            _mushroom[i].name = "Shroomie";
        }

    }

    /// <summary>
    /// Let's share some tunes!
    /// </summary>
    public void PlayBackgroundMusic()
    {

        AudioClip _song;

        // Loading a new planet should come with music, yes?
        //_audio = _globe.AddComponent<AudioSource>();
        _audioSource = GameObject.Find("MusicBG");
        _audio = _audioSource.GetComponent<AudioSource>();
        string _songName = "Music/gypsy" + (GameState._level % 3).ToString();
        _song = Resources.Load(_songName, typeof(AudioClip)) as AudioClip;
        _audio.clip = _song;
        _audio.Play();

    }
}
