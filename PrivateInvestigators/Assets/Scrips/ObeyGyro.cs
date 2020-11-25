//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ObeyGyro : MonoBehaviour
{
    [Header("Adjustments")]
    [SerializeField] private Quaternion baseRotation = new Quaternion(0,0,1,0);

    // Start is called before the first frame update
    private void Start()
    {
      GyroManager.Instance.EnableGyro();
    }

    // Update is called once per frame
    private void Update()
    {
      transform.localRotation = GyroManager.Instance.GetRotation() * baseRotation;
    }
}
