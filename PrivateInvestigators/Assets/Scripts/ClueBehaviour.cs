using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueBehaviour : MonoBehaviour
{
    public float timerCountDown = 3.0f;
    public int itemHP = 3;

    private bool isUncovered = false;
    Camera m_mainCamera;
    SpriteRenderer m_sprite;


    // Start is called before the first frame update
    void Start()
    {
        m_mainCamera = Camera.main;
        m_sprite = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        Color tmp = m_sprite.color;
        tmp.a =  1/distance;
        m_sprite.color = tmp;

    }



    


}
