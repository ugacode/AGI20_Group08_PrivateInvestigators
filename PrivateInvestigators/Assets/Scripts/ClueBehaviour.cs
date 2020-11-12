using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueBehaviour : MonoBehaviour
{
    public float timerCountDown = 3.0f;
    public int itemHP = 3;


    public ARManager.toolList requiredTool = ARManager.toolList.glove;
    private bool isUncovered = false;


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public ARManager.toolList getRequiredTool()
    {
        return requiredTool;
    }

    public void onCollision(ARManager.toolList p_currentTool)
    {
        if (isUncovered)
            return;

        if (itemHP == 0)
            return;
        if (p_currentTool == requiredTool)
        {
            timerCountDown -= Time.deltaTime;
            if (timerCountDown < 0)
            {
                this.GetComponent<SpriteRenderer>().enabled = true;
                //this.GetComponent<SpriteRenderer>().material.color = Color.green;
                timerCountDown = 0;
                GameObject.Find("AR Session Origin").GetComponent<ARManager>().ActivatePopUp(true);
                isUncovered = true;
            }


        }
        else
        {
            timerCountDown -= Time.deltaTime;
            if (timerCountDown < 0)
            {
                isUncovered = true;
                itemHP--;
                if (itemHP == 0)
                    GameObject.Find("AR Session Origin").GetComponent<ARManager>().ActivatePopUp(false);

            }
        }


    }


}
