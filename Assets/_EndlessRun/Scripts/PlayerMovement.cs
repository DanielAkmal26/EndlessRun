using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    public int speed;
    public int turnSpeed;
    private int desiredLane = 1; //Posisi Laning (0 = Left, 1 = Mid, 2 = Right)

    public bool isGrounded;
    public float jumpForce = 20.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    Rigidbody rb;

    //Swipe Variabel
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Detect Input Arrow and Touch Swipe
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //MoveLeft
            MoveLane(false);
            Debug.Log("Left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //MoveRight
            MoveLane(true);
            Debug.Log("Right");
        }
        //====================================================================BUG====================================================================

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //Go Up
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

        //Calculate Jump

        //if (isGrounded) //if Ground
        //{
        //    verticalVelocity = -0.1f;
        //    if (Input.GetKeyDown(KeyCode.UpArrow))
        //    {
        //        //Jump
        //        verticalVelocity = jumpForce;
        //        isGrounded = false;
        //    }
        //}
        //else
        //{
        //    verticalVelocity -= (gravity * 3 * Time.deltaTime);

        //    //Fast Falling Mechanic
        //    //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    //{
        //    //    verticalVelocity = -jumpForce;
        //    //}
        //}
        //====================================================================BUG====================================================================

        //Detect Swipe
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }


        //Calculate Position
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }


        //Integrate Player Position
        //transform.Translate(Vector3.forward / speedForce);
        Vector3 moveVector = Vector3.zero;
        moveVector.y = verticalVelocity;
        moveVector.x = (targetPosition - transform.position).x * turnSpeed;

        moveVector.z = speed;

        transform.Translate(moveVector * Time.deltaTime);
    }

    private void MoveLane(bool goRight)
    {
        desiredLane += (goRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);

        //left
        //if (!goRight)
        //{
        //    desiredLane--;
        //    if (desiredLane == -1)
        //    {
        //        desiredLane = 0;
        //    }
        //}else
        //{
        //    desiredLane++;
        //    if (desiredLane == 3)
        //    {
        //        desiredLane = 2;
        //    }
        //}
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        MoveLane(false);
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        MoveLane(true);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (col.gameObject.tag == "Obstacle")
        {
            SceneManager.LoadScene("Main");
        }
    }
}
