using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    public PostProcessVolume postProcVol;
    
    public CityRatPlayer player;

    public bool Blur;

    private int fixedCounter;
    private float lastPlayerSpeed = 0.0f;
    private float targetDof = 30.0f;
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
        
        if (player && player.speed != lastPlayerSpeed)
        {
            
            var playerSpeed = player.speed;
            if (playerSpeed > 6.8f)
            {
                targetDof = 30.0f - playerSpeed * 3.15f;
            }
            else
            {
               targetDof = 30.0f;

            }
            lastPlayerSpeed = playerSpeed;
        }
    }

    void Update()
    {
        if (Blur)
        {
            DepthOfField dof = null;
            postProcVol.profile.TryGetSettings(out dof);
            dof.focusDistance.value = dof.focusDistance.value * (1-(Time.deltaTime/4)) + targetDof * (Time.deltaTime/4);
        }
    }
}

