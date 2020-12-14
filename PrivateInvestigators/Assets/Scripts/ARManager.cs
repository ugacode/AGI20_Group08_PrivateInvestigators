using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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


    public List<GameObject> spawnedObjectList = new List<GameObject>();


    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isUVLightOn = false;
    public GameObject UVLight;

    public Text DistanceTextUI;
    public GameObject placeUI;
    public GameObject interactUI;
    public GameObject moveUI;

    public Shader outlineShader;
    public Shader standardShader;


    const float pinchTurnRatio = Mathf.PI / 2;
    const float minTurnAngle = 0;
    const float pinchRatio = 1;

    // <summary> // The delta of the angle between two touch points // </summary> 
    private float turnAngleDelta;

    // <summary> // The angle between two touch points // </summary> 
    private float turnAngle;


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
                spawnedObjectList.Add(spawmedObject);
            
            }
            else
            {
                spawmedObject.transform.position = hitPos.position;
            }
     

        }



    }

    public void Place()
    {
       

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
                 
                        for (int i = 0; i < spawmedObject.transform.childCount; i++)
                        {
                            
                                if (selectedObject.transform.GetChild(i).tag != "Selectable")
                            {
                                selectedObject.transform.GetChild(i).GetComponent<Renderer>().material.shader = outlineShader;
                            }
                           
                        }

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


          
            Quaternion desiredRotation = transform.rotation;

            turnAngle = Angle(touchZero.position, touchOne.position);
            float prevTurn = Angle(touchZero.position - touchZero.deltaPosition, touchOne.position - touchOne.deltaPosition);
            turnAngleDelta = Mathf.DeltaAngle(prevTurn, turnAngle);


            // ... if it's greater than a minimum threshold, it's a turn! 
            if (Mathf.Abs(turnAngleDelta) > minTurnAngle)
            {
                turnAngleDelta *= pinchTurnRatio;
            }
            else
            {
                turnAngle = turnAngleDelta = 0;
            }

            if (Mathf.Abs(turnAngleDelta) > 0)
            {
                Vector3 rotationDeg = Vector3.zero;
                rotationDeg.y = -turnAngleDelta;
                desiredRotation *= Quaternion.Euler(rotationDeg);
            }

            selectedObject.transform.Rotate(desiredRotation.x, desiredRotation.y*30, desiredRotation.z, Space.Self);
        }

        //remove
        //DistanceTextUI.text = Vector3.Distance(selectedObject.transform.position, transform.position).ToString();



    }


    public void ItemToSpawn(GameObject newItem) {

        if(newItem== null)
        {
            return;
        }
        spawmedObject = null;
        currentObject = newItem;
        currentState = arState.placeItem;
        planeManager.enabled = !planeManager.enabled;
        placeUI.SetActive(true);
        interactUI.SetActive(false);
        SetAllPlanesActive(true);
        isUVLightOn = false;
        UVLight.SetActive(false);


    }


    private float Angle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 from = pos2 - pos1;
        Vector2 to = new Vector2(1, 0);
        float result = Vector2.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        if (cross.z > 0)
        {
            result = 360f - result;
        }
        return result;
    }

    public void DeleteObject()
    {
        moveUI.SetActive(false);
        interactUI.SetActive(true);
        currentState = arState.interact;
        Destroy(selectedObject);
        selectedObject = null;
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

