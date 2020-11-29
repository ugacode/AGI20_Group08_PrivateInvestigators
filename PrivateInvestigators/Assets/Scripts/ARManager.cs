using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class ARManager : MonoBehaviour
{
    public GameObject currentObject;


    public enum toolList
    {
        glove,
        hammer,
    }

    private enum arState
    {
        placeItem,
        interact,
        move,
    }


    private arState currentState = arState.placeItem;
    public toolList currentTool = toolList.glove;
    private GameObject spawmedObject;
    private ARRaycastManager m_arCastManager;
    private Vector2 touchPosition;
    private ARSessionOrigin arOrigin;
    private ARPlaneManager planeManager;

    private GameObject selectedObject = null;


    public List<string> clueList;
    private List<string> foundClues = new List<string>();

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isUVLightOn = false;
    public GameObject UVLight;

    public GameObject placeUI;
    public GameObject interactUI;
    public GameObject moveUI;

    public Shader outlineShader;
    public Shader standardShader;


    private void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }


    private void Awake()
    {
        m_arCastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

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

    private void changeState(arState newState)
    {

    }


    public void SetObject(GameObject p_object)
    {
        Destroy(spawmedObject);

        currentObject = p_object;
        // MoveObject();
    }


    private void SetAllPlanesActive(bool value){

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }

        }

    private void PlaceObject()
    {

        if (m_arCastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = hits[0].pose;

            if (spawmedObject == null)
            {
                spawmedObject = Instantiate(currentObject.gameObject, hitPos.position, hitPos.rotation);
                // spawmedObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f,1.0f,1.0f,0.5f);
            
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

    public void placeObject()
    {
        //spawmedObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        if (spawmedObject != null)
        {
            currentState = arState.interact;
            planeManager.enabled = !planeManager.enabled;
            SetAllPlanesActive(false);
            placeUI.SetActive(false);
            interactUI.SetActive(true);
        }
        

    }
 
    private void CheckObjectSelection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;
            bool isOverUI = IsPointerOverUIObject(touchPosition);



            if (!isOverUI)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    if (Physics.Raycast(ray, out hitObject))
                    {
                        if (selectedObject != null)
                        {

                        }
                        selectedObject = hitObject.transform.gameObject;
                        // selectedObject.GetComponent<Renderer>().material.shader = outlineShader;
                        for (int i = 0; i < spawmedObject.transform.childCount; i++)
                        {
                            
                                if (selectedObject.transform.GetChild(i).tag != "Selectable")
                            {
                                selectedObject.transform.GetChild(i).GetComponent<Renderer>().material.shader = outlineShader;
                            }
                           
                        }

                        //Renderer[] children;
                        //children = GetComponentsInChildren<Renderer>();
                        //foreach (Renderer rend in children)
                        //{
                        //    var mats = new Material[rend.materials.Length];
                        //    for (var j = 0; j < rend.materials.Length; j++)
                        //    {
                        //        mats[j].shader = outlineShader;
                        //    }
                        //    rend.materials = mats;
                        //}

                        currentState = arState.move;
                        interactUI.SetActive(false);
                        moveUI.SetActive(true);
                        isUVLightOn = false;
                        UVLight.SetActive(false);

                    }

                }

            }
        }


    }

  



        private void MoveObject()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            var currentTouch = touch.position;

            bool isOverUI = IsPointerOverUIObject(currentTouch);



            if (!isOverUI)
            {
                if (m_arCastManager.Raycast(currentTouch, hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPos = hits[0].pose;


                    selectedObject.transform.position = hitPos.position;


                }
            }
        }
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            float pinchAmount = deltaMagnitudeDiff * 0.002f * Time.deltaTime;
            selectedObject.transform.localScale -= new Vector3(pinchAmount, pinchAmount, pinchAmount);
        }
    }

    private bool IsPointerOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        PointerEventData evenPosition = new PointerEventData(EventSystem.current);
        evenPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(evenPosition, results);

        return results.Count > 0;

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
    //locks and removes the selected object
    public void Lock()
    {
        moveUI.SetActive(false);
        interactUI.SetActive(true);
        currentState = arState.interact;
        for (int i = 0; i < spawmedObject.transform.childCount; i++)
        {
            if (selectedObject.transform.GetChild(i).tag != "Selectable")
            {
                selectedObject.transform.GetChild(i).GetComponent<Renderer>().material.shader = standardShader;
            }
        }
        selectedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentObject)
            return;

        if (currentState == arState.placeItem)
        {
            PlaceObject();
        }
        else if (currentState == arState.interact)
        {
            CheckObjectSelection();
        }
        else if (currentState == arState.move)
        {
            MoveObject();
        }

    }
}

