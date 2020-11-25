// ﻿using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

  public class GyroManager : MonoBehaviour
  {
    #region Instance
      private static GyroManager instance;
      public static GyroManager Instance{
        get {
          if(instance == null){
            instance = FindObjectOfType<GyroManager>();
            if(instance == null){
              instance = new GameObject("Generated GyroManager", typeof(GyroManager)).GetComponent<GyroManager>();
            }
          }
          return instance;
        }
        set {
          instance = value;
        }
      }
    #endregion

    [Header("Logic")]
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro(){
      if(gyroActive){ return; }

      if(SystemInfo.supportsGyroscope){
        Debug.Log("system supports gyro");
        gyro = Input.gyro;
        gyro.enabled = true;
        gyroActive = gyro.enabled;
       } else{
         Debug.Log("system doesn't support gyro");
       }
    }

    // Update is called once per frame
    private void Update() {
      if(gyroActive){
        rotation = gyro.attitude;// TODO Input.gyro.attitude;
      }
    }

    public Quaternion GetRotation(){
      return rotation;
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //
    // }
  }








//
