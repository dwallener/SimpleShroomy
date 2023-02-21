using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulatePlanet : MonoBehaviour
{

    /// <summary>
    /// Start - not currently used
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update - not currently used
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// With streamlined scene management this is currently redundant
    /// </summary>
    /// <param name="_level"></param>
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

    /// <summary>
    /// The helper method for NewPlanet() - currently unused for the same reasons
    /// </summary>
    public void Planet01()
    {

        // drop things on the planet
        int _numShrooms = 50;
        GameObject[] _mushroom = new GameObject[_numShrooms];
        Vector3 _rng;

        // have some objects drop down on it
        for (int i = 0; i < _numShrooms; i++)
        {
            BoxCollider _bc;
            _rng = Random.onUnitSphere * 7;
            Debug.Log("@: " + _rng.x + " " + _rng.y + " " + _rng.z);
            _mushroom[i] = Instantiate(Resources.Load("Shroom1", typeof(GameObject)), _rng, Quaternion.identity) as GameObject;
            _mushroom[i].AddComponent<MushroomActions>();
            _mushroom[i].AddComponent<Rigidbody>();
            _mushroom[i].AddComponent<GravityBody>(); // make it feel gravity
            _bc = _mushroom[i].AddComponent<BoxCollider>();
            Debug.Log("ShroomieBC position" + _bc.transform.position);
        }
    }

}

