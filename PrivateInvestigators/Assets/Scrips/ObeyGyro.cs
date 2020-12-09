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
     private const int degJump = 18;
     private int prevJump = degJump/3;
     public GameObject ClueMessage;
     public float startingPitch = 0.3f;
     public bool solved;
     private bool unloading;
     private System.DateTime start;
     private int winPos;

    // Start is called before the first frame update
    private void Start()
    {
      solved = false;
      unloading = false;
      audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.pitch = startingPitch;
      GyroManager.Instance.EnableGyro();
      baseRotation = transform.localRotation;
      var rand = new System.Random();
      winPos = rand.Next((int)(360/degJump)) * degJump; // because 360/15 = 24
      // Debug.Log("Winpos deg : " + winPos + " WinPos radians: " + (Math.PI/180)*winPos);

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
          //double winPos = (180/Math.PI) * (solution*15); // the random
          // double winPos = Math.PI/4; // win position is hard coded to be at 225 deg now, can be changed to be dynamic somehow.
          double sineValue = Math.Sin(((Math.PI / 180) * jump - (Math.PI +(Math.PI/180)*(double)winPos))/2.0);
          Debug.Log("sine: " + sineValue);
          audioSource.pitch = 1f + (float)Math.Pow(sineValue, 32.0) + (float)Math.Pow(sineValue, 4.0);
          // Debug.Log("pitch: " + audioSource.pitch);
          audioSource.PlayOneShot(clip, 0.5f);

          // To make the vibrations stronger closer to the goal:
          // double vibe = 20 + 0.5 * ((float)Math.Pow(sineValue, 32.0) + (float)Math.Pow(sineValue, 4.0)) * 235;
          // Vibration.Vibrate(15, (int)vibe, false);

          // To make the vibrations constant:
          // Vibration.Vibrate(15, 100, false);
        }
        // if(jump == 225){
        //   if(prevJump != 225){
        if(jump == winPos){
          if(prevJump != winPos){
            //Debug.Log("jump: " + jump);
            start = System.DateTime.Now;
            Vibration.Vibrate(5, 255, false);
            //Debug.Log("winPos reached: " + System.DateTime.Now);
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
