using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFramer : MonoBehaviour
{
    private Camera mainCam;

    [HideInInspector]
    public bool doFollowShoe = false;
    [HideInInspector]
    public Transform shoe;

    private float yFloor; //lowest the camera's border will go

    private Vector3 resetPos;
    private float resetSize;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        float yFloor = mainCam.transform.position.y - mainCam.orthographicSize;

        resetPos = mainCam.transform.position;
        resetSize = mainCam.orthographicSize;
    }

    void Update()
    {
        if (doFollowShoe)
        {
            //calculate the camera's edges
            float vExtent = mainCam.orthographicSize;
            float hExtent = vExtent * Screen.width / Screen.height;

            float minX = mainCam.transform.position.x - hExtent;
            float maxX = mainCam.transform.position.x + hExtent;
            float minY = mainCam.transform.position.y - vExtent;
            float maxY = mainCam.transform.position.y + vExtent;

            //grab the difference between the shoe's position and the camera's bounds
            float xDif = 0f, yDif = 0f;
            if (shoe.position.x > maxX)
                xDif = shoe.position.x - maxX; //will be positive
            
            if (shoe.position.y > maxY)
                yDif = shoe.position.y - maxY;

            float increaseAmount = Mathf.Max(xDif, yDif) / 2f;

            //increase the size of the camera, and move it so the bottom left stays in the same spot
            mainCam.orthographicSize += increaseAmount;
            mainCam.transform.position += new Vector3(increaseAmount * Screen.width / Screen.height, increaseAmount, 0f);
        }
    }

    public void Reset()
    {
        doFollowShoe = false;
        mainCam.transform.position = resetPos;
        mainCam.orthographicSize = resetSize;
    }
}
