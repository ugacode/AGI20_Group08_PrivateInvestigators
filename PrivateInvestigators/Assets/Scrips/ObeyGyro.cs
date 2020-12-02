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
        transform.localRotation = Quaternion.Euler(0, 0, (float)jump) * baseRotation;
        audioSource.PlayOneShot(clip, 0.5f);
        Vibration.Vibrate(15, 80, false);
        Debug.Log("jump!");
      }
      prevJump = jump;
    }
}
