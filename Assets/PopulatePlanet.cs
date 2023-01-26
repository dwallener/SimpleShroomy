using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulatePlanet : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewPlanet(int _level)
    {
        Debug.Log("New Planet");
        switch(_level)
        {
            case 0: Planet01(); break;
            case 1: break;
            case 2: break;
            case 3: break;
            case 4: break;
            default:
                break;
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
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Shroom1", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _mushroom[i].AddComponent<BoxCollider>(); 
        }
    }

}

