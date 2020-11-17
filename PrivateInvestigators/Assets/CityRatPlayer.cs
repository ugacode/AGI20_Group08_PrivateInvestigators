using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using UnityEngine;

public class CityRatPlayer : MonoBehaviour
{
    [SerializeField]
	AbstractMap _map;

    public float speed;
    public bool walking = false;
    public bool running = false;

    private DateTime _lastSpeedUpdate;
    private List<Mapbox.Utils.Vector2d> _playerLocations = new List<Mapbox.Utils.Vector2d>();

    private object _lockObj = new object();

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.0f;
        _lastSpeedUpdate = DateTime.UtcNow;
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
    }

    void FixedUpdate()
    {
        var nowTime = DateTime.UtcNow;
        var timeDiff = nowTime - _lastSpeedUpdate;
        
        if (timeDiff.TotalSeconds > 1)
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
}
