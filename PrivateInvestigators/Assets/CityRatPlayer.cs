using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class CityRatPlayer : MonoBehaviour
{
    [SerializeField]
	AbstractMap _map;

    public float speed;

    private DateTime _lastSpeedUpdate;
    private List<Vector3> _playerLocations = new List<Vector3>();

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
        
        if (timeDiff.TotalSeconds > 3)
        {
            lock(_lockObj)
            {
                nowTime = DateTime.UtcNow;
                timeDiff = nowTime - _lastSpeedUpdate;

                speed = 0.0f;
                var distance = 0.0f;

                for (int i = 1; i < _playerLocations.Count; ++i)
                {
                    var loc1 = _playerLocations[i-1];
                    var loc2 = _playerLocations[i];
                    distance += Math.Abs(loc2.x - loc1.x) + Math.Abs(loc2.y - loc1.y);
                }
                speed = distance / (float)timeDiff.TotalSeconds;
                
                _lastSpeedUpdate = nowTime;
                _playerLocations.Clear();
            }    
        }
    }

    public void PlayerMoved(Vector3 newPosition)
    {
        lock(_lockObj)
        {
            _playerLocations.Add(newPosition);
        }
    }
}
