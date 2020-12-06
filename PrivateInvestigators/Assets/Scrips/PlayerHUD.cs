using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public CityRatPlayer player;
    public Text speedText;
    public Text cluesText;

    private float minFov = 50.0f;
    private float maxFov = 82.0f;
    private float targetFov;
    private float zoomDamping = 9f;
    private float zoomVelocity = 0f;
    private float zoomSensitivityMouse = 10.0f;
    private float zoomSensitivityTouch = 0.06f;
    private Camera cam;
    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void Start()
    {
        cam = Camera.main;
        targetFov = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            speedText.text = $"Player speed - {player.speed:0.#} m\\s";
            cluesText.text = $"Clues - {player.collectedClues}/10";

            if (Input.touchSupported)
            {
                HandleTouch();
            }
            else
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                ZoomCamera(scroll, zoomSensitivityMouse);
            }

            if (targetFov != cam.fieldOfView)
            {
                //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * zoomDamping);
                cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetFov, ref zoomVelocity, Time.deltaTime * zoomDamping);
            }
        }
    }

    void HandleTouch() {
        if (Input.touchCount == 2)
        {
            Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
            if (!wasZoomingLastFrame) {
                lastZoomPositions = newPositions;
                wasZoomingLastFrame = true;
            } else {
                float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                float offset = newDistance - oldDistance;
    
                ZoomCamera(offset, zoomSensitivityTouch);
    
                lastZoomPositions = newPositions;
            }
        }
        else
        {
            wasZoomingLastFrame = false;
        }
    }
    

    void ZoomCamera(float offset, float speed) {
        if (offset == 0) {
            return;
        }
    
        targetFov = Mathf.Clamp(cam.fieldOfView - (offset * speed), minFov, maxFov);
        zoomVelocity = 0f;
    }
}
