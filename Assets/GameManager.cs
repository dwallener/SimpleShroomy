
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

    public GameObject _fps; // for debug only
    public TextMeshProUGUI _fpsText;

    public GameObject _timer;
    public TextMeshProUGUI _timerText;
    public GameObject _panel;

    // main gameObjects
    public GameObject _globe;
    public GameObject _pawn; // let's go with Unreal terminology, lol
    public GameObject _pawnExhaust; // the thing to spew from

    // Audio thingies
    public AudioSource[] _asAll;
    public AudioSource _asMusic;
    public AudioSource _asSFX1;
    public AudioSource _asSFX2;
    public AudioSource _asSFX3;
    public AudioClip _acMusic; // music loop
    public AudioClip _acSFX1; // mushroom pop sound
    public AudioClip _acSFX2; // win sound
    public AudioClip _acSFX3; // lose sound

    // player thingies
    public int _playerScore = 0;

    // timer thingies
    public int _countDown = 0;
    public float _countDownf = 0f;

    // bookkeeping thingies
    //public Quaternion _spinDir;
    public Vector3 _spinDir = Vector3.zero;
    public float _spinAngle = 0f;
    public bool _isLava = false;
    public bool _isExplode = false;
    public int _thisLevelGoal = 0;
    public bool _handlingLevelTransition = false;

    // demo/clip stuff
    private bool _cameraFollow = true;
    private bool _hudOff = true;

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

/*
 *      Debug.Log("Angle 0.5, 0.5 :: " + Mathf.Atan2(0.5f, 0.5f) * 180f / Mathf.PI);
        Debug.Log("Angle -0.5, -0.5 :: " + Mathf.Atan2(-0.5f, -0.5f) * 180f / Mathf.PI);
        Debug.Log("Angle -0.5, 0.5 :: " + Mathf.Atan2(-0.5f, 0.5f) * 180f / Mathf.PI);
        Debug.Log("Angle 0.5, -0.5 :: " + Mathf.Atan2(0.5f, -0.5f) * 180f / Mathf.PI);
        Debug.Log("Angle 0, 1 :: " + Mathf.Atan2(0f, 1f) * 180f / Mathf.PI);
        Debug.Log("Angle 1, 0 :: " + Mathf.Atan2(1f, 0f) * 180f / Mathf.PI);
        Debug.Log("Angle -1, 0 :: " + Mathf.Atan2(-1f, 0f) * 180f / Mathf.PI);
        Debug.Log("Angle 0, -1 :: " + Mathf.Atan2(0f, -1f) * 180f / Mathf.PI);
*/

        // this is super important...mobile locks to a max fps based on screen refresh...
        // in Editor fps is 100s to 1000s of fps...causes physics problems.
        // we need explicit control of this

        Application.targetFrameRate = 60;

        // manage player prefs
        if (PlayerPrefs.HasKey("Level"))
        {
            GameState._level = PlayerPrefs.GetInt("Level");
            Debug.LogFormat("Prefs exist, Level {0}", GameState._level);
        }
   
        _instance = this;

        // set up the HUD objects
        _curtain = GameObject.Find("curtainBlack");

        _fps = GameObject.Find("fps");
        _fpsText = _fps.GetComponent<TextMeshProUGUI>();

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
        Debug.Log("Planet Index: " + _planetIndex);
        Debug.Log("Planet Name: " + GameState._prefabList[_planetIndex]);

        // choose between lava and not lava
        string _levelType = GameState._levelType[GameState._level % 12];

        if (_levelType.Contains("Lava"))
        {
            // some of the planets are too tricky for naive lava detection
            // Tundra5, Tundra2
            if (_planetIndex == 24)
            {
                _planetIndex = Random.Range(0, 20);
                Debug.Log("Skip Tundra5");
            }
            if (_planetIndex == 21) {
                _planetIndex = Random.Range(0, 20);
                Debug.Log("Skip Tundra2");
            }

            _isLava = true;
            // structure is 40 normal worlds, 40 lava equivalents
            _globe = Instantiate(Resources.Load(
                GameState._prefabList[_planetIndex + 40], typeof(GameObject)), Vector3.zero, Quaternion.identity)
                as GameObject;
            // should this be + or - ?
            var _newCenter = _globe.transform.position + _globe.GetComponent<MeshFilter>().mesh.bounds.center;
            _globe.transform.position = _newCenter;
            _globe.transform.localScale = GameState._startingScale[_planetIndex];
        }
        else
        {
            _isLava = false;
            _globe = Instantiate(Resources.Load(
                GameState._prefabList[_planetIndex], typeof(GameObject)), Vector3.zero, Quaternion.identity)
                as GameObject;
        }
        Debug.LogFormat("Planet: {0}", GameState._prefabList[_planetIndex]);

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = GameState._skyboxList[_planetIndex];

        // set up our own gravity and physics
        _globe.AddComponent<GravityAttractor>();
        _globe.AddComponent<GlobeActions>();
        _globe.AddComponent<Rigidbody>();
        _globe.GetComponent<Rigidbody>().useGravity = false;
        _globe.GetComponent<Rigidbody>().isKinematic = true;
        _globe.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

        // issue with 256 triangle limit on meshcollider
        MeshCollider _mc = _globe.AddComponent<MeshCollider>();
        _mc.convex = true;

        // adjust the position and scale of the planet
        _globe.transform.position = Vector3.zero;
        //_globe.transform.localScale = new Vector3(5f, 5f, 5f);
        _globe.transform.localScale = Vector3.Scale(new Vector3(5f, 5f, 5f), GameState._startingScale[_planetIndex]);
        _globe.transform.Rotate(GameState._startingRot[_planetIndex]);
        _globe.tag = "Planet";

        //PlayBackgroundMusic();

        // PAWN
        GameObject _pawn = new GameObject();

        // load a model prefab - made a variant with the skin etc
        _pawn = Instantiate(Resources.Load<GameObject>("Prefabs/Pawns/Pawn"), new Vector3(0f, 0f, -2.5f), Quaternion.identity);
        _pawn.AddComponent<Rigidbody>();

        // set up location, physics, blah blah blah
        //_pawn.transform.position = new Vector3(0f, 0f, -2.7f);
        _pawn.transform.Rotate(0, 180, 0);
        _pawn.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        // give it it's own intelligence
        _pawn.AddComponent<PawnActions>();
        _pawn.name = "Pawn";

        BoxCollider _bc = _pawn.AddComponent<BoxCollider>();
        // tallify the box in Z - tweak this again in #scene
        _bc.size = new Vector3(3f, 3f, 3f);

        // pawn is colliding with shrooms successfully, but not with planet.
        Rigidbody _rb = _pawn.GetComponent<Rigidbody>();
        _rb.mass = 1000f;
        _rb.useGravity = false;
        _rb.centerOfMass = new Vector3(0f, 0f, -0.5f);

        // use planet gravity attractor
        _pawn.AddComponent<GravityBody>();

    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

        // add audiosources
        _asAll = Camera.main.GetComponents<AudioSource>();
        // get clips
        _acSFX1 = Resources.Load<AudioClip>("SFX/pop1");
        _acSFX2 = Resources.Load<AudioClip>("SFX/success1");
        _acSFX3 = Resources.Load<AudioClip>("SFX/fail2"); // sad whistle
        var _musicClip = "SFX/gypsy_ml_" + Random.Range(1, 8).ToString();
        _acMusic = Resources.Load<AudioClip>(_musicClip);
        //_acMusic = Resources.Load<AudioClip>("SFX/gypsy1");

        // assign music channel
        _asMusic = _asAll[0];
        _asMusic.clip = _acMusic;
        _asMusic.volume = 1f;
        _asMusic.loop = true;
        // turn off for gameplay capture
        //_asMusic.Play(0);

        // assign sfx1 channel
        _asSFX1 = _asAll[1];
        _asSFX1.loop = false;
        _asSFX1.clip = _acSFX1;
        _asSFX1.volume = 1f;

        // assign sfx2 channel
        _asSFX2 = _asAll[2];
        _asSFX2.loop = false;
        _asSFX2.clip = _acSFX2;
        _asSFX2.volume = 1f;

        // assign sfx2 channel
        _asSFX3 = _asAll[3];
        _asSFX3.loop = false;
        _asSFX3.clip = _acSFX3;
        _asSFX3.volume = 1f;

        // always reset, early often late, always
        _playerScore = 0;
      
        // make the planet!
        NewPlanet(GameState._level);

        // fill in the HUD!
        _message.GetComponent<TextMeshProUGUI>().text = "Collect Shroomies!";
        _messageText.text = "Collect Shroomies!";

        //_score.GetComponent<TextMeshProUGUI>().text = "Score: " + 0.ToString() + " of ";
        _scoreText.text = "Score: " + _playerScore.ToString() + " of ";

        // demo /capture setup
        if (_cameraFollow)
        {
            Camera.main.gameObject.AddComponent<CameraFollowPlayer>();
        }

        if (_hudOff)
        {
            GameObject.Find("CanvasHUD").SetActive(false);
        }

        // need different goal for each type
        string _levelType = GameState._levelType[GameState._level % 12];

        if (_levelType.Contains("Collect"))
        {
            _goal.GetComponent<TextMeshProUGUI>().text = GameState._levelGoals[GameState._level / 5].ToString();
        }
        if (_levelType.Contains("Clearcut"))
        {
            _goal.GetComponent<TextMeshProUGUI>().text = _thisLevelGoal.ToString();
        }
        if (_levelType.Contains("Find"))
        {
            _goal.GetComponent<TextMeshProUGUI>().text = "1";
        }

        _level.GetComponent<TextMeshProUGUI>().text = "LEVEL " + GameState._level.ToString();

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

        Debug.Log("Level goal: " + _thisLevelGoal);

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // update fps
        _fpsText.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);

        // update the timer
        _countDownf -= Time.deltaTime;

        // this is just to debug scene management
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // don't forget to reset score, even in debug!
            _playerScore = 0;
            GameState._level++;
            Debug.Log("Level: " + GameState._level);
            StartCoroutine(LoadNextScene(true));
        }

        // get level end conditions
        string _levelType = GameState._levelType[GameState._level % 12];

        // bucket the level types
        // can only do one of these during a scene...then everything gets destroyed

        // !Lava

        if (_levelType == "Collection")
        {
            _messageText.text = string.Format("Collect {0} Shroomies!", GameState._levelGoals[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Collection TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect {0} Shroomies in {1} seconds!",
                GameState._levelGoals[GameState._level / 5], GameState._levelTimers[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));

            // exit conditions - if timer goes off first, fail - if score first, win
            if (_countDownf <= 0f)
            {
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Find" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Find the magic Shroomie!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            // find levels only need 1 shroomie
            if (_playerScore >= 1)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Find TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Find the magic Shroomie in {0} seconds!",
                GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_countDownf <= 0f)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= 1)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Clearcut" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect all the shroomies!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_playerScore >= GameManager.Instance._thisLevelGoal)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Clearcut TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect all the shroomies in {0} seconds!",
               GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_countDownf < 0f)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameManager.Instance._thisLevelGoal)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        // !Lava
        else if (_levelType == "Collection Lava" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect {0} Shroomies! Lava! Bad!", GameState._levelGoals[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            if (_isExplode)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Collection Lava TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect {0} Shroomies in {1} seconds! Lava! Bad!",
                GameState._levelGoals[GameState._level / 5], GameState._levelTimers[GameState._level / 5]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of ";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_isExplode || _countDownf <= 0f)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameState._levelGoals[GameState._level / 6])
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Find Lava" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Find the magic Shroomie! Lava! Bad!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of 1";
            if (_isExplode)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= 1)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Find Lava TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Find the magic Shroomie in {0} seconds! Lava! Bad!",
                GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of 1";
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_isExplode || _countDownf <= 0f)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= 1)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Clearcut Lava" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect all the shroomies! Lava! Bad!");
            _scoreText.text = "Score: " + _playerScore.ToString() + " of " + _thisLevelGoal;
            if (_isExplode)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameManager.Instance._thisLevelGoal)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }
        else if (_levelType == "Clearcut Lava TT" && !_handlingLevelTransition)
        {
            _messageText.text = string.Format("Collect all the shroomies in {0} seconds! Lava! Bad!",
               GameState._levelTimers[GameState._level / 6]);
            _scoreText.text = "Score: " + _playerScore.ToString() + " of " + _thisLevelGoal;
            _timerText.text = string.Format("{0} s", Mathf.RoundToInt(_countDownf));
            if (_isExplode || _countDownf <= 0)
            {
                _playerScore = 0;
                //AdvanceLevel(false);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(false));
                return;
            }
            if (_playerScore >= GameManager.Instance._thisLevelGoal)
            {
                // force player score to 0...?
                _playerScore = 0;
                //AdvanceLevel(true);
                _handlingLevelTransition = true;
                StartCoroutine(LevelAdvance(true));
                return;
            }
        }

    }

    public IEnumerator LevelAdvance(bool _won)
    {

        // only advance level if we win!
        if (_won)
        {
            GameState._level++;
            _asSFX2.Play();
            Debug.Log("Playing Success sound");
        }
        else
        {
            Debug.Log("Playing Fail sound");
            _asSFX3.Play();
        }
        PlayerPrefs.SetInt("Level", GameState._level);
        //Debug.Log("Level: " + GameState._level);
        yield return StartCoroutine(LoadNextScene(_won));

    }

    // move to the next level
    /// <summary>
    /// One place to advance to the next level
    /// </summary>
    public void AdvanceLevel(bool _won)
    {
        // only advance level if we win!
        if (_won)
        {
            GameState._level++;
            _asSFX2.Play();
        }
        else
        {
            Debug.Log("Playing Fail sound");
            _asSFX3.Play();
        }
        PlayerPrefs.SetInt("Level", GameState._level);
        //Debug.Log("Level: " + GameState._level);
        StartCoroutine(LoadNextScene(_won));
    }

    /// <summary>
    /// Manage scene transistions
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadNextScene(bool _won)
    {
        yield return FadeOut(0.3f);

        // wait for fade
        // keep consistent fadeout, change the win/lost message
        if (_won)
        {
        }
        else
        {
        }

        // if we've done the level reading/randomization correctly
        // we should now only ever need one scene...?
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
    public IEnumerator FadeOut(float _fadeSpeed)
    {
        _curtain.GetComponent<Image>().color = new Color32(0, 0, 0, 100); // Color.black;
        Color _fadeColor = _curtain.GetComponent<Image>().color;
        float _fadeAmount;

        while (_curtain.GetComponent<Image>().color.a < 1)
        {
            _fadeAmount = _fadeColor.a + (_fadeSpeed * Time.deltaTime);
            _fadeColor = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _fadeAmount);
            _curtain.GetComponent<Image>().color = _fadeColor;

            yield return null;
        }
    }

    /// <summary>
    /// General purpose fade screen for loss scenario
    /// </summary>
    /// <param name="_fadeSpeed"></param>
    /// <returns></returns>
    public IEnumerator FadeOutLoser(float _fadeSpeed)
    {
        _curtain.GetComponent<Image>().color = new Color32(255, 0, 0, 100); // Color.red;
        Color _fadeColor = _curtain.GetComponent<Image>().color;
        float _fadeAmount;

        while (_curtain.GetComponent<Image>().color.a < 1)
        {
            _fadeAmount = _fadeColor.a + (_fadeSpeed * Time.deltaTime);
            _fadeColor = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _fadeAmount);
            _curtain.GetComponent<Image>().color = _fadeColor;

            yield return null;
        }
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
        float _fallHeight = Random.Range(5f, 11f);
        Vector3 _rng;

        // the thinking here...
        // "Find" levels use 1 pink shroom
        // "Collect" levels use blue shrooms
        // "Clearcut" levels use green shrooms

        string _shroomPrefab = "";
        int _rngNumShrooms = 0; // int Random is not inclusive

        switch (GameState._level % 12)
        {
            case 2 or 3 or 8 or 9: // "Find"
                _shroomPrefab = "Prefabs/pinkMushroom";
                _rngNumShrooms = Random.Range(1, 3); // int Random is not inclusive
                _thisLevelGoal = 1;
                break;
            case 0 or 1 or 6 or 7: // Collect
                _shroomPrefab = "Prefabs/blueMushroom";
                _rngNumShrooms = Random.Range(25, 76); // int Random is not inclusive
                _thisLevelGoal = GameState._levelGoals[GameState._level / 5];
                break;
            case 4 or 5 or 10 or 11: // Clearcut
                _shroomPrefab = "Prefabs/greenMushroom";
                // have a modifier to increase difficulty...?
                _rngNumShrooms = Random.Range(25, 35); // int Random is not inclusive
                _thisLevelGoal = _rngNumShrooms;
                break;
            default:
                break;
        }
        Debug.Log("Shroom Prefab: " + _shroomPrefab);

        string _levelType = GameState._levelType[GameState._level % 12];

        GameObject[] _mushroom = new GameObject[_rngNumShrooms+1];

        // have some objects drop down on it
        for (int i = 0; i < _rngNumShrooms; i++)
        {
            _rng = Random.onUnitSphere * _fallHeight;
            //Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load(_shroomPrefab, typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            BoxCollider _bc = _mushroom[i].GetComponent<BoxCollider>();
            //_bc.size = new Vector3(0.2f, 0.2f, 0.2f);
            //_bc.center = new Vector3(-0.5f, -0.5f, -0.5f);
            _mushroom[i].transform.parent = _globe.transform;
            _mushroom[i].name = "Shroomie";
        }

        // drop one bad mushroom
        _shroomPrefab = "Prefabs/yellowMushroom";
        _rng = Random.onUnitSphere * _fallHeight;
        //Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
        _mushroom[_rngNumShrooms] = Instantiate(Resources.Load(_shroomPrefab, typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
        _mushroom[_rngNumShrooms].AddComponent<MushroomActions>();
        _mushroom[_rngNumShrooms].AddComponent<Rigidbody>();
        _mushroom[_rngNumShrooms].AddComponent<GravityBody>(); // make it feel gravity
        _mushroom[_rngNumShrooms].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        _mushroom[_rngNumShrooms].transform.parent = _globe.transform;
        _mushroom[_rngNumShrooms].name = "BadShroomie";

    }

    /// <summary>
    /// Let's share some tunes!
    /// </summary>
    public void PlayBackgroundMusic()
    {

    }

}
