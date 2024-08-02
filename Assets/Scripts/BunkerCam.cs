using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BunkerCam : MonoBehaviour
{
    private Vector3 scrollInput;

    private Vector3 mouseInput;

    public float moveSpeed = 8f;

    public float smoothing = 4f;

    public float zoomSpeed = 4f;

    private bool autoPanCamera = true;

    public Vector3[] panningWaypoints;

    public float minChangeWaypointDistance = 10f;

    public float panningSpeed = 2f;

    private int currentWaypointIndex = 0;

    private Vector3 panningVelocity;

    private void OnEnable()
    {
        scrollInput = Vector3.one * Input.GetAxis("Mouse ScrollWheel");
        mouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoPanCamera = !autoPanCamera;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseInput = Vector3.zero;
        }

        scrollInput = Vector3.Lerp(scrollInput, new Vector3(0, 0, 1000) * Input.GetAxis("Mouse ScrollWheel"), 8 * Time.deltaTime);

        //transform.localScale += scrollInput * zoomSpeed;
        //transform.localPosition -= scrollInput * zoomSpeed;

        if (!autoPanCamera)
        {
            if (PointerOnScreen())
                mouseInput = Vector3.Lerp(mouseInput, new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0), smoothing * Time.deltaTime);

            Vector3 mouseDragAmount = Vector3.zero;
            if (Input.GetMouseButton(0)) {
                mouseDragAmount = (mouseInput * moveSpeed) + scrollInput;
            }

            transform.localPosition += (mouseDragAmount - scrollInput);
        }
        else
        {
            AutoCam();

            transform.localPosition -= scrollInput;
        }

       
    }

    private void AutoCam()
    {
        //transform.localPosition = Vector3.Lerp(transform.localPosition, panningWaypoints[currentWaypointIndex], panningSpeed * Time.deltaTime);

        Vector3 finalWaypoint = panningWaypoints[currentWaypointIndex];
        finalWaypoint.z = transform.localPosition.z;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalWaypoint, ref panningVelocity, panningSpeed * Time.deltaTime);

        


        if ((finalWaypoint - transform.localPosition).magnitude < minChangeWaypointDistance)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= panningWaypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    private bool PointerOnScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (!screenRect.Contains(Input.mousePosition))
            return false;
        else
            return true;
    }
}
