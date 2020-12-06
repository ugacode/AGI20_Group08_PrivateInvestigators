using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



[RequireComponent(typeof(ARRaycastManager))]


public class ARManager: MonoBehaviour
{
    public GameObject currentObject;


    public enum toolList
    {
        glove,
        hammer,
    }

    public toolList currentTool = toolList.glove;
    private GameObject spawmedObject;
    private ARRaycastManager m_arCastManager;
    private Vector2 touchPosition;
    private ARSessionOrigin arOrigin;
    private bool isLocked = false;
  ///  public GameObject particles;
  //  public GameObject popUP;
    public List<string> clueList;
    private List<string> foundClues = new List<string>();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isUVLightOn = false;
    public GameObject UVLight;
    private GameObject lightTarget;
    public GameObject lightDistanceUI;

    AudioSource aud;

    private void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();

        aud = GetComponent<AudioSource>();
        aud.clip = Microphone.Start(null, true, 15, 44100);
        aud.loop = true;
        while (!(Microphone.GetPosition(null) > 0)){ }
    }


    private void Awake()
    {
        m_arCastManager = GetComponent<ARRaycastManager>();
        //particles.SetActive(false);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public void ScaleUp()
    {
        if (spawmedObject == null)
            return;

        spawmedObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);


    }

    public void ScaleDown()
    {
        if (spawmedObject == null)
            return;

        spawmedObject.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
    }

    public void ChangeToolToGlove()
    {
        currentTool = toolList.glove;
    }
    public void ChangeToolToHammer()
    {
        currentTool = toolList.hammer;
    }



    public void Lock()
    {
        if (isLocked)
        {
            isLocked = false;
        }
        else
        {
            isLocked = true;
        }


    }

    public void SetObject(GameObject p_object)
    {
        Destroy(spawmedObject);

        currentObject = p_object;
        MoveObject();
    }

    public void DestroyDust()
    {
        if(spawmedObject) {
            ParticleSystem ps = spawmedObject.GetComponent<ParticleSystem>();
            Destroy(ps);
        } 
    }


    private void MoveObject()
    {

        if (m_arCastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;

            if (spawmedObject == null)
            {
                spawmedObject = Instantiate(currentObject.gameObject, hitPos.position, hitPos.rotation);

            }
            else
            {
                spawmedObject.transform.position = hitPos.position;
            }
            spawmedObject.SetActive(true);

        }
        else
        {
            if (spawmedObject == null)
            {
                spawmedObject.SetActive(false);
            }
        }


    }


    void CheckObjectCollisions()
    {
        TryGetTouchPosition(out touchPosition);
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hitObject;
        if (Physics.Raycast(ray, out hitObject, 25.0f))
        {
            if (hitObject.transform.tag == "Selectable")
            {
                hitObject.transform.GetComponent<ClueBehaviour>().onCollision(currentTool);

                //particles.SetActive(true);
                //particles.transform.position = hitObject.point;
            }
        }
        else
        {
           // particles.SetActive(false);
        }



    }

    public void ActivatePopUp(bool isFound)
    {
       // popUP.SetActive(true);
        if (isFound)
        {
            if (foundClues.Count >= clueList.Count)
            {

              //  popUP.GetComponentInChildren<UnityEngine.UI.Text>().text = "No more clues";

            }
            else
            {

             //   popUP.GetComponentInChildren<UnityEngine.UI.Text>().text = GetText();

            }
        }
        else
        {

          //  popUP.GetComponentInChildren<UnityEngine.UI.Text>().text = "Clue Damaged";
        }



    }

    private string GetText()
    {
        if (foundClues.Count >= clueList.Count)
            return "No clues";
        //Gets a random clue and returns it if it was not previousl
        int randomClue = Random.Range(0, clueList.Count);
        foreach (string clue in foundClues)
        {
            if (clue == clueList[randomClue])
            {
                return GetText();
            }
        }
        foundClues.Add(clueList[randomClue]);
        return clueList[randomClue];
    }


    public void ChangeLight()
    {
        if (isUVLightOn)
        {
            isUVLightOn = false;
            UVLight.SetActive(false);

        }
        else
        {
            isUVLightOn = true;
            UVLight.SetActive(true);


        }

    }



    // Update is called once per frame
    void Update()
    {
        if (!currentObject)
            return;

        if (!isLocked)
        {
            MoveObject();
        }
        else
        {



            if (lightTarget)
            {
                lightTarget.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (Vector3.Distance(lightTarget.transform.position, this.transform.position) - 0.5f));
                lightDistanceUI.GetComponent<UnityEngine.UI.Text>().text = Vector3.Distance(lightTarget.transform.position, this.transform.position).ToString();
            }
            if (isUVLightOn)
            {
                RaycastHit hitObject;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f, 0f));

                if (Physics.Raycast(ray, out hitObject, 25.0f))
                {

                    if (hitObject.transform.tag == "Selectable")
                    {

                        if (lightTarget != hitObject.transform.gameObject)
                        {
                            // lightTarget.transform.GetComponent<SpriteRenderer>().enabled = false;
                            lightTarget = hitObject.transform.gameObject;

                        }

                        lightTarget.GetComponent<SpriteRenderer>().enabled = true;
                        lightTarget.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (Vector3.Distance(lightTarget.transform.position, this.transform.position) - 0.5f));
                        lightDistanceUI.GetComponent<UnityEngine.UI.Text>().text = Vector3.Distance(lightTarget.transform.position, this.transform.position).ToString();
                    }
                }


            }
            CheckObjectCollisions();


        }

    }
}

