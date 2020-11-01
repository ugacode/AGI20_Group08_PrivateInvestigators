using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class CityRatPlayer : MonoBehaviour
{
    [SerializeField]
	AbstractMap _map;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
