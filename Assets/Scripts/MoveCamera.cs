using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to move the camera when the cursor nears the edge of the game window
/// src 1: https://answers.unity.com/questions/275436/move-the-camera-then-the-cursor-is-at-the-screen-e.html]
/// src 2: https://www.youtube.com/watch?v=fQ2Dvj5-pfc
/// </summary>
public class MoveCamera : MonoBehaviour
{

    public int Boundary = 50; // distance from edge scrolling starts
    public int speed = 5;

    public BoxCollider2D boundBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private int theScreenWidth;
    private int theScreenHeight;

    private Camera theCamera;
    private float halfHeight;
    private float halfWidth;
    public GameManager manager;
    
        
    private void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;

        minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;

        theCamera = GetComponent<Camera>();
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * theScreenWidth / theScreenHeight;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && manager.isRunning())
        {
            Debug.Log("shift down");
            if (Input.mousePosition.x > theScreenWidth - Boundary)
            {
                //Debug.Log("yeet1");
                transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0); // move on +X axis
            }
            if (Input.mousePosition.x < 0 + Boundary)
            {
                //Debug.Log("yeet2");
                transform.position = transform.position - new Vector3(speed * Time.deltaTime, 0, 0); // move on -X axis
            }
            if (Input.mousePosition.y > theScreenHeight - Boundary)
            {
                //Debug.Log("yeet3");
                transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0); // move on +y axis
            }
            if (Input.mousePosition.y < 0 + Boundary)
            {
                //Debug.Log("yeet4");
                transform.position = transform.position - new Vector3(0, speed * Time.deltaTime, 0); // move on -y axis
            }

            float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            //Debug.Log("Clamped X: " + clampedX + ", Clamped Y: " + clampedY);


            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
    /*
    private void OnGUI()
    {
        GUI.Box(new Rect((Screen.width / 2) - 140, 5, 280, 25), "Mouse Position = " + Input.mousePosition);
        GUI.Box(new Rect((Screen.width / 2) - 70, Screen.height - 30, 140, 25), "Mouse X = " + Input.mousePosition.x);
        GUI.Box(new Rect(5, (Screen.height / 2) - 12, 140, 25), "Mouse Y = " + Input.mousePosition.y);
    }
    */
}
