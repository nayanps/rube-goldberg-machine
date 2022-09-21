using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Transform connectedTransform;

    private Camera mainCam;

    private void Start()
    {
        //hide the mouse
        Cursor.visible = false;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        SetTransformToMousePos(this.transform);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }

    private void SetTransformToMousePos(Transform t)
    {
        //Grab the mouse position
        Vector3 mousePos = Input.mousePosition;

        //convert it to world coordinates
        mousePos = mainCam.ScreenToWorldPoint(mousePos);

        float vExtent = mainCam.orthographicSize;
        float hExtent = vExtent * Screen.width / Screen.height;

        //calculate the camera's edges
        float minX = mainCam.transform.position.x - hExtent;
        float maxX = mainCam.transform.position.x + hExtent;
        float minY = mainCam.transform.position.y - vExtent;
        float maxY = mainCam.transform.position.y + vExtent;

        //clamp the position to the boundary box sprite
        mousePos.x = Mathf.Clamp(mousePos.x, minX, maxX);
        mousePos.y = Mathf.Clamp(mousePos.y, minY, maxY);

        //set the position
        t.transform.position = mousePos;
    }
}
