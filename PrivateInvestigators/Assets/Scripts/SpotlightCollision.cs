using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightCollision : MonoBehaviour
{

    public GameObject spotLight;
    //public GameObject distanceUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.rotation = Camera.main.transform.rotation;

        gameObject.transform.position = Camera.main.transform.position;
        // RaycastHit sphereHit;
        // Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0f));
        // if (Physics.SphereCast(ray, 5, out sphereHit, 25.0f))
        //{

        //    if (sphereHit.transform.tag == "Selectable")
        //    {
        //        distanceUI.GetComponent<UnityEngine.UI.Text>().text = "collision ray";
        //    }
        //}
        //if (Physics.Raycast(ray, out sphereHit, 25.0f))
        //{
        //    if (sphereHit.transform.tag == "Selectable")
        //    {
        //        distanceUI.GetComponent<UnityEngine.UI.Text>().text = "collision ray";
        //    }
        //}

    }
    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.transform.tag == "Selectable")
    //    {
    //        collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        spotLight.gameObject.GetComponent<LightPositionScript>().ChangeColor(Color.green);

    //       // distanceUI.GetComponent<UnityEngine.UI.Text>().text = "collision";
    //        //  distanceUI.GetComponent<UnityEngine.UI.Text>().text = Vector3.Distance(collision.transform.position, Camera.main.transform.position).ToString();
    //    }
    //    //distanceUI.GetComponent<UnityEngine.UI.Text>().text = collision.gameObject.transform.tag;
    //    Debug.Log("collision");
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.transform.tag == "Selectable")
    //         {
    //        //        collision.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    //        //        spotLight.gameObject.GetComponent<LightPositionScript>().ChangeColor(Color.green);

    //        //       // distanceUI.GetComponent<UnityEngine.UI.Text>().text = "collision";
    //        //        //  distanceUI.GetComponent<UnityEngine.UI.Text>().text = Vector3.Distance(collision.transform.position, Camera.main.transform.position).ToString();
    //        Debug.Log("collision");

    //}
    //    Debug.Log("collision");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("aaaaaaaaa");
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("aaaaaaaaa");
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("aaaaaaaaa");
    //}
  

}
