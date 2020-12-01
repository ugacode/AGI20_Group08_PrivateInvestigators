// Author: Karin Lagrelius nov 2020
using RDG;
using System;
//using System.Collections.Generic;


using UnityEngine;

public class ObeyGyro : MonoBehaviour
{
    [Header("Adjustments")]
    [SerializeField] private Quaternion baseRotation;

     public AudioSource audioSource;
     public AudioClip clip;
     private int prevJump = -1;


    // Start is called before the first frame update
    private void Start()
    {
      audioSource = gameObject.AddComponent<AudioSource>();
      GyroManager.Instance.EnableGyro();
      baseRotation = transform.localRotation;
    }

    // Update is called once per frame
    private void Update()
    {
    //   transform.localRotation = baseRotation * GyroManager.Instance.GetRotation();

    //  Quaternion quat =  GyroManager.Instance.GetRotation();
      Vector3 rot = GyroManager.Instance.GetRotation().eulerAngles;

      // int jump = (int)Math.Floor((rot.z - 15/2)/15)*15;
      int jump = (int)Math.Floor(rot.z/15)*15;

      if(prevJump != -1 && prevJump != jump){
        transform.localRotation = Quaternion.Euler(0, 0, 180f-(float)jump) * baseRotation;
        audioSource.PlayOneShot(clip, 0.5f);
        // Handheld.Vibrate();
        //   Vibration.Vibrate(10000, 100, false);
        Vibration.Vibrate(1000);
        // Handheld.Vibrate();
        //   if (Application.isEditor) {Handheld.Vibrate();}
        // Vibration.Cancel();
        Debug.Log("jump!");
      }
      prevJump = jump;
    }
}





/*
private static readonly AndroidJavaObject Vibrator =
    new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
    .GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
    .Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.

static void KyVibrator()
{
    // Trick Unity into giving the App vibration permission when it builds.
    // This check will always be false, but the compiler doesn't know that.
    if (Application.isEditor) Handheld.Vibrate();
}

public static void Vibrate(long milliseconds)
{
    Vibrator.Call("vibrate", milliseconds);
}




// NYTT;
public class KyVibrator : MonoBehaviour{

     private static readonly AndroidJavaObject Vibrator =
         new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
         .GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
         .Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.


         static KyVibrator()
         {
             // Trick Unity into giving the App vibration permission when it builds.
             // This check will always be false, but the compiler doesn't know that.
             if (Application.isEditor) Handheld.Vibrate();
         }

         public static void Vibrate(long milliseconds)
         {
             Vibrator.Call("vibrate", milliseconds);
         }
}
// SLUT NYTT

*/
