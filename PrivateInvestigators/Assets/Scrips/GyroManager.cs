// Author: Karin Lagrelius nov 2020


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
    private bool gyroInUse;
    private Gyroscope gyro;
    private Quaternion rotation;

    public void EnableGyro(){
      if(gyroInUse){ return; }

      if(SystemInfo.supportsGyroscope){
        Debug.Log("system supports gyro");
        gyro = Input.gyro;
        gyro.enabled = true;
        gyroInUse = true;
       } else {
         Debug.Log("System doesn't support gyro :(");
       }
    }

    public Quaternion GetRotation(){
      return rotation;
    }

    // Update is called once per frame
    private void Update() {
      if(gyroInUse){ rotation = gyro.attitude; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
  }








//
