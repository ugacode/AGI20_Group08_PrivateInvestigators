using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPositionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
        gameObject.transform.position = Camera.main.transform.position;
    }

    public void ChangeColor(Color p_color)
    {

        gameObject.GetComponent<Light>().color = p_color;

    }

}
