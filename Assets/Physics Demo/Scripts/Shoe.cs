using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Shoe : MonoBehaviour
{
    //an enum which we use to easily create our own way of tracking state
    public enum ShoeState
    {
        Wearing,
        Airborn,
        Landed
    }
    [HideInInspector]
    public ShoeState currentShoeState;

    //just references to rigidbodies and starting info
    private Rigidbody2D rb;
    private Rigidbody2D parentRb;
    private Transform startParent;
    private Quaternion startRotation;

    //score and ground references
    private Text score;
    private float startingX;

    //camera
    private CameraFramer camFramer;



    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        camFramer = Camera.main.GetComponent<CameraFramer>();
        camFramer.shoe = this.transform;

        //save the parent to use when getting the shoe back
        startParent = this.transform.parent;
        parentRb = startParent.GetComponent<Rigidbody2D>();
        startRotation = this.transform.localRotation;
        score = GameObject.FindGameObjectWithTag("score").GetComponent<Text>();
    }

    private void Update()
    {
        //When the mouse is pressed, handle the input based on the shoe's state
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentShoeState)
            {
                case ShoeState.Wearing:
                    //make the rigidbody fall again, and unchild it
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    this.transform.parent = null;

                    //give it the velocity of the parent it just had
                    rb.AddForce(parentRb.velocity, ForceMode2D.Impulse);

                    //start the camera follow
                    camFramer.doFollowShoe = true;

                    startingX = this.transform.position.x;

                    currentShoeState = ShoeState.Airborn;
                    break;
                case ShoeState.Airborn:
                    //do nothing when we are leaving the airborn state
                    break;
                case ShoeState.Landed:
                    ResetShoe();
                    break;
            }
        }
    }

    public void ResetShoe()
    {
        //reset its position and rotation, make it a child again
        this.transform.position = startParent.transform.position;
        this.transform.SetParent(startParent);
        this.transform.localRotation = startRotation;

        //Rigidbody is now kinematic because we are controlling it
        rb.bodyType = RigidbodyType2D.Kinematic;

        //reset velocities
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;

        //change shoe state to wearing
        currentShoeState = ShoeState.Wearing;

        camFramer.Reset();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //If this shoe is colliding and not moving, set the state to landed if not there already
        if (currentShoeState != ShoeState.Landed && rb.velocity.magnitude < 0.05f)
        {
            score.text = (this.transform.position.x - startingX).ToString("F2");
            currentShoeState = ShoeState.Landed;
        }
    }
}
