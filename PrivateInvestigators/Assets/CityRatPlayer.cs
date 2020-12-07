using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityRatPlayer : MonoBehaviour
{
    [SerializeField]
	AbstractMap _map;

    public float speed;

    public int collectedClues;
    public int state = 0;
    public int nextState = 0;
    public GameObject rootObject;

    private DateTime _lastSpeedUpdate;
    private List<Mapbox.Utils.Vector2d> _playerLocations = new List<Mapbox.Utils.Vector2d>();

    private object _lockObj = new object();

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        collectedClues = 0;
        speed = 0.0f;
        _lastSpeedUpdate = DateTime.UtcNow.AddSeconds(-2);
        _anim = GetComponentsInChildren<Animator>().First();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("mouse down");
        //     var mousePosScreen = Input.mousePosition;

	    //     mousePosScreen.z = Camera.main.transform.localPosition.y;
	    //     var pos = Camera.main.ScreenToWorldPoint(mousePosScreen);

	    //     var latlongDelta = _map.WorldToGeoPosition(pos);
	    //     Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y);
        // }
        if (nextState != state)
        {
            state = nextState;
            _anim.SetInteger("state", state);
        }
    }

    void FixedUpdate()
    {
        var nowTime = DateTime.UtcNow;
        var timeDiff = nowTime - _lastSpeedUpdate;
        
        if (timeDiff.TotalSeconds > 2)
        {
            lock(_lockObj)
            {
                nowTime = DateTime.UtcNow;
                timeDiff = nowTime - _lastSpeedUpdate;

                if (_playerLocations.Count > 1)
                {
                    double distance = 0.0;
                    for (int i = 1; i < _playerLocations.Count; ++i)
                    {
                        var loc1 = Conversions.LatLonToMeters(_playerLocations[i-1]);
                        var loc2 = Conversions.LatLonToMeters(_playerLocations[i]);
                        distance += Math.Abs(loc2.x - loc1.x) + Math.Abs(loc2.y - loc1.y);
                    }
                    speed = (float)distance / (float)timeDiff.TotalSeconds;
                    
                    _lastSpeedUpdate = nowTime;
                    var currentLocation = _playerLocations.Last();
                    _playerLocations.Clear();
                    _playerLocations.Add(currentLocation);
                }
                else
                {
                    speed = speed / 4.0f;
                }
                if (speed > 0.5)
                {
                    if (speed > 7.5)
                    {
                        nextState = 2;
                    }
                    else
                    {
                        nextState = 1;
                    }
                }
                else
                {
                    nextState = 0;
                }
            }    
        }
    }

    public void PlayerMoved(Mapbox.Utils.Vector2d newPosition)
    {
        lock(_lockObj)
        {
            _playerLocations.Add(newPosition);
        }
    }

    public void ClueCollected()
    {
        StartCoroutine(LoadVaultSceneAsync());
        ++collectedClues;
    }

    IEnumerator LoadVaultSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GyroGame", LoadSceneMode.Additive);
        rootObject.SetActive(false);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        rootObject.SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MapScene"));
    }
}
