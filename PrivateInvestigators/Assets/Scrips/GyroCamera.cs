// Author: Karin Lagrelius nov 2020

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour {
    const float xRes = 9;
    const float yRes = 16; // for aspect ratio 9:16

    void Start()
    {
      float preferredRatio = xRes / yRes;
      float screenRatio = (float)Screen.height / (float)Screen.width;

      if (Math.Abs(screenRatio - preferredRatio) < 0.0000005){
        Debug.Log("screen perfect ratio");
        GetComponent<Camera>().rect = new Rect(0, 0, 1.0f, 1.0f);

      } else if(screenRatio < preferredRatio){
        Debug.Log("screen a bit wide"); // add padding on x axis
        Debug.Log("screenRatio:" + screenRatio);
        Debug.Log("preferredRatio:" + preferredRatio);
        Debug.Log("Screen.width:" + (float)Screen.width);
        Debug.Log("Screen.height:" + (float)Screen.height);

        float xWidth = ((float)Screen.width / preferredRatio);
        float xMargin = ((float)Screen.height - xWidth);
        Debug.Log("xMargin:" + xMargin);
        float scaledXMargin = (xMargin / (float)Screen.height) / 2.0f;
        //float scaledXWidth = xWidth/(float)Screen.width;

        Debug.Log("scaledXMargin:" + scaledXMargin); // add padding on x axis
      //  GetComponent<Camera>().rect = new Rect(scaledXMargin, 0, 1.0f-scaledXMargin, 1.0f);
        GetComponent<Camera>().rect = new Rect(scaledXMargin, 0, xWidth, 1f);

        // float yMargin = ((float)Screen.height - ((float)Screen.width / preferredRatio))/ 2.0f;
        // float scaledYMargin = yMargin/(float)Screen.height;
        // GetComponent<Camera>().rect = new Rect(0, scaledYMargin, 1.0f, 1.0f-scaledYMargin);

      } else if(screenRatio > preferredRatio){
        Debug.Log("screen a bit long"); // add padding on x axis
        Debug.Log("screenRatio:" + screenRatio);
        Debug.Log("preferredRatio:" + preferredRatio);
        Debug.Log("Screen.width:" + (float)Screen.width);
        Debug.Log("Screen.height:" + (float)Screen.height);

        float yMargin = ((float)Screen.height - ((float)Screen.width / preferredRatio))/ 2.0f;
        float scaledYMargin = yMargin/(float)Screen.height;
        GetComponent<Camera>().rect = new Rect(0, scaledYMargin, 1.0f, 1.0f-scaledYMargin);

        // float xMargin = ((float)Screen.width - ((float)Screen.height * preferredRatio));
        // Debug.Log("xMargin:" + xMargin);
        // float scaledXMargin = (xMargin / (float)Screen.width) / 2.0f;
        // Debug.Log("scaledXMargin:" + scaledXMargin); // add padding on x axis
        // GetComponent<Camera>().rect = new Rect(scaledXMargin, 0, 1.0f-scaledXMargin, 1.0f);
        // rect (xmin, ymin, xmax, ymax)
      }
    }

    // Update is called once per frame
    // void Update()
    // {
    // }
}
