// Author: Karin Lagrelius nov 2020
using RDG;
using System;
using System.Collections;
//using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ObeyGyro : MonoBehaviour
{
    [Header("Adjustments")]
    [SerializeField] private Quaternion baseRotation;

     public AudioSource audioSource;
     public AudioClip clip;
     private const int degJump = 15;
     private int prevJump = degJump/3;
     public GameObject ClueMessage;
     public float startingPitch = 0.3f;
     public bool solved;
     private bool unloading;
     private System.DateTime start;

    // Start is called before the first frame update
    private void Start()
    {
      solved = false;
      unloading = false;
      audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.pitch = startingPitch;
      GyroManager.Instance.EnableGyro();
      baseRotation = transform.localRotation;

      // transform.localRotation = Quaternion.Euler(0, 0, 0) * baseRotation;
    }

    // Update is called once per frame
    private void Update()
    {
      //   transform.localRotation = baseRotation * GyroManager.Instance.GetRotation();
      //  Quaternion quat =  GyroManager.Instance.GetRotation();
      if (solved == false)
      {
        Vector3 rot = GyroManager.Instance.GetRotation().eulerAngles;

        // int jump = (int)Math.Floor((rot.z - 15/2)/15)*15;
        int jump = (int)Math.Floor((rot.z)/degJump)*degJump;
        //Debug.Log("jump" + jump + " prevJump" + prevJump + " z-angle:" + rot.z);

        if(prevJump != degJump/3 && prevJump != jump){
          transform.localRotation = Quaternion.Euler(0, 0, (float)jump) * baseRotation;
          double winPos = Math.PI/4; // win position is hard coded to be at 225 deg now, can be changed to be dynamic somehow.
          double sineValue = Math.Sin(((Math.PI / 180) * jump - winPos)/2.0);
          audioSource.pitch = 1f + (float)Math.Pow(sineValue, 32.0) + (float)Math.Pow(sineValue, 4.0);
          // Debug.Log("pitch: " + audioSource.pitch);
          audioSource.PlayOneShot(clip, 0.5f);

          // To make the vibrations stronger closer to the goal:
          // double vibe = 20 + 0.5 * ((float)Math.Pow(sineValue, 32.0) + (float)Math.Pow(sineValue, 4.0)) * 235;
          // Vibration.Vibrate(15, (int)vibe, false);

          // To make the vibrations constant:
          // Vibration.Vibrate(15, 100, false);
        }
        if(jump == 225){
          if(prevJump != 225){
            start = System.DateTime.Now;
            Vibration.Vibrate(15, 100, false);
            //Debug.Log("225 reached: " + System.DateTime.Now);
          }
          
          // int timeDiff = System.DateTime.Now - start;
          System.TimeSpan timeDiff = System.DateTime.Now - start;
          //Debug.Log("timeDiff: " + timeDiff);

          if (timeDiff.Seconds >= 3){
            solved = true;
          }
        }

        prevJump = jump;
      }
      else if (unloading == false) {
        unloading = true;
        Debug.Log("You opened the safe! Clue collected.");
        ClueMessage.SetActive(true);
        StartCoroutine(UnloadVaultSceneAsync());
      }
    }

    private IEnumerator UnloadVaultSceneAsync()
    {
        yield return new WaitForSeconds(3.0f);
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("GyroGame");

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}
