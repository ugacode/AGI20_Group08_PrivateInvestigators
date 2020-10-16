using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

/// <summary>
	/// The MouseClickLocationProvider is responsible for providing mock location and heading data
	/// based on mouse clicks.
	/// </summary>
public class MouseClickLocationProvider : AbstractEditorLocationProvider
{
    /// <summary>
    /// The mock "latitude, longitude" location, respresented with a string.
    /// You can search for a place using the embedded "Search" button in the inspector.
    /// This value can be changed at runtime in the inspector.
    /// </summary>
    [SerializeField]
    [Geocode]
    string _initialLatitudeLongitude;

    [SerializeField]
    AbstractMap _map;

    /// <summary>
    /// The mock heading value.
    /// </summary>
    [SerializeField]
    [Range(0, 359)]
    float _heading;

    private Vector2d previousLocation;
    private Vector2d newLocation = Vector2d.zero;
    Vector2d LatitudeLongitude
    {
        get
        {
            if (newLocation.Equals(previousLocation) == false)
            {
                if (newLocation.Equals(Vector2d.zero) == false)
                {
                    previousLocation = newLocation;
                }
            }

            return previousLocation;
        }
    }

    protected override void SetLocation()
    {
        _currentLocation.UserHeading = _heading;
        _currentLocation.LatitudeLongitude = LatitudeLongitude;
        _currentLocation.Accuracy = _accuracy;
        _currentLocation.Timestamp = UnixTimestampUtils.To(DateTime.UtcNow);
        _currentLocation.IsLocationUpdated = true;
        _currentLocation.IsUserHeadingUpdated = true;
    }

#if UNITY_EDITOR
    override protected void Awake()
    {
        base.Awake();
        previousLocation = Conversions.StringToLatLon(_initialLatitudeLongitude);
    }
#else
    void Awake()
    {
        previousLocation = Conversions.StringToLatLon(_initialLatitudeLongitude);
    }
#endif

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosScreen = Input.mousePosition;

	        mousePosScreen.z = Camera.main.transform.localPosition.y;
	        var pos = Camera.main.ScreenToWorldPoint(mousePosScreen);

	        var latlongDelta = _map.WorldToGeoPosition(pos);
            newLocation = latlongDelta;
	        Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y);
        }
    }
}
