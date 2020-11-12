using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    public PostProcessVolume postProcVol;
    private int fixedCounter;
    // Start is called before the first frame update
    void Start()
    {
        fixedCounter = 1;
    }

    void FixedUpdate()
    {
        ++fixedCounter;
        Bloom bloomLayer = null;
        postProcVol.profile.TryGetSettings(out bloomLayer);
        if (fixedCounter <= 50)
        {
            bloomLayer.intensity.value -= 0.05f;
        }
        else if (fixedCounter < 100)
        {
            bloomLayer.intensity.value += 0.05f;
        } else
        {
            //Debug.Log(bloomLayer.intensity.value);
            fixedCounter = 1;
        }
        
    }
}

