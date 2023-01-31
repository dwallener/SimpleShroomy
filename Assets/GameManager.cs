
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
    public GameObject _pawn; // let's go with Unreal terminology, lol
 
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
        // for debugging
        _planetIndex = 0;
        _globe = Instantiate(Resources.Load(GameState._prefabList[_planetIndex], typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        RenderSettings.skybox.SetColor("_Tint", GameState._skyboxList[_planetIndex]);
        _globe.AddComponent<GravityAttractor>();
        _globe.AddComponent<GlobeActions>();
        // sphere collider sucks, can we do a mesh collider?
        //_globe.AddComponent<SphereCollider>();
        MeshCollider _mc = _globe.AddComponent<MeshCollider>();
        _mc.convex = true;

        _globe.transform.position = Vector3.zero;
        _globe.transform.localScale = new Vector3(5f, 5f, 5f);
        _globe.tag = "Planet";

        // let's add our player sprite
        GameObject _pawn = new GameObject();
        _pawn.AddComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/ShroomKing1-preview", typeof(Sprite)) as Sprite;
        _pawn.transform.position = new Vector3(0f, 0f, -2.7f);
        _pawn.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

    }

    // Start is called before the first frame update
    void Start()
    {
        // make the planet!
        NewPlanet(GameState._level);
        // fill in the HUD!
        _message.GetComponent<TextMeshProUGUI>().text = "Collect Shroomies!";
        _score.GetComponent<TextMeshProUGUI>().text = "Score: " + 0.ToString() + " of ";
        _goal.GetComponent<TextMeshProUGUI>().text = GameState._levelGoals[GameState._level%5].ToString();
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

        // if we've done the level reading/randomization correctly
        // we should now only ever need one scene...?
        //SceneManager.LoadScene(GameState._level);
        //SceneManager.UnloadScene(GameState._level - 1);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadScene(0);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));

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
        CreatePlanet();
    }


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
        }

    }

}
