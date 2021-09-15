using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PMovement : MonoBehaviour
{
    Rigidbody rb;

    private GameManager GMScript;

    public float timer = 0.0f;
    //private bool increment = true;
    bool alive = true;

    private const float LANE_DISTANCE = 3.2f;
    //private const float TURN_SPEED = 0.05f;

    //Swipe Variabel
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = true;
    public float SWIPE_THRESHOLD = 20f;

    public Text debugtext;

    private CharacterController controller;
    private float jumpForce = 20.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    public float speed = 50.0f;
    public int turnSpeed;
    private int desiredLane = 1; //Posisi Laning (0 = Left, 1 = Mid, 2 = Right)

    public int ChancePlay = 1;

    [Header("Debuff Applier")]
    public GameObject diz;
    // Start is called before the first frame update

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        GMScript = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float seconds = timer % 60;

        if (timer > 30)
        {
            speed += 2;
            timer = 0;
        }

        if (!alive)
        {
            return;
        }
        else
        {
            //Keyboard Control
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //MoveLeft
                MoveLane(false);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //MoveRight
                MoveLane(true);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Jump");
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            //Swipe Control
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

            //Calculate Position Change
            Vector3 targetPosition = transform.position.z * Vector3.forward;

            if (desiredLane == 0)
            {
                targetPosition += Vector3.left * LANE_DISTANCE;
            }
            else if (desiredLane == 2)
            {
                targetPosition += Vector3.right * LANE_DISTANCE;
            }

            //Calculate Delta Movement
            Vector3 moveVector = Vector3.zero;
            moveVector.x = (targetPosition - transform.position).normalized.x * speed;

            //Calculate Y (Jump)
            if (controller.isGrounded) //if Ground
            {
                //GroundedYes();
                verticalVelocity = -0.1f;
                if (Input.GetKeyDown(KeyCode.UpArrow) || debugtext.text == "Swipe Up")
                {
                    //Jump
                    verticalVelocity = jumpForce;
                    debugtext.text = "";
                }
            }
            else
            {
                //GroundedNo();
                verticalVelocity -= (gravity * 4 * Time.deltaTime);

                //Fast Falling Mechanic
                if (Input.GetKeyDown(KeyCode.DownArrow) || debugtext.text == "Swipe Down")
                {
                    verticalVelocity = -jumpForce;
                    debugtext.text = "";
                }
            }

            moveVector.x = (targetPosition - transform.position).x * turnSpeed;
            moveVector.y = verticalVelocity;
            moveVector.z = speed;

            //Move Character Forward
            controller.Move(moveVector * Time.deltaTime);

            //Rotate Character Where he is going
            //Vector3 dir = controller.velocity;
            //if (dir != Vector3.zero)
            //{
            //    dir.y = 0;
            //    transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
            //}
        }
    }

    //public void GroundedYes()
    //{
        
    //}

    //public void GroundedNo()
    //{
        
    //}

    public void Die()
    {
        alive = false;
        //Restart
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
        debugtext.text = "Swipe Up";
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
        debugtext.text = "Swipe Down";
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        debugtext.text = "Swipe Left";
        MoveLane(false);
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        debugtext.text = "Swipe Right";
        MoveLane(true);
    }

    private bool isGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            Time.timeScale = 0;
            GMScript.GameOver();
        }
        if (ChancePlay == 1)
        {
            if (col.gameObject.tag == "ObstacleLeft")
            {
                MoveLane(false);
                ChancePlay -= 1;
                diz.SetActive(true);
                StartCoroutine(Dizzy());
            }
            if (col.gameObject.tag == "ObstacleRight")
            {
                MoveLane(true);
                ChancePlay -= 1;
                diz.SetActive(true);
                StartCoroutine(Dizzy());
            }
        }
        else
        {
            if (col.gameObject.tag == "ObstacleLeft")
            {
                Time.timeScale = 0;
                GMScript.GameOver();
            }
            if (col.gameObject.tag == "ObstacleRight")
            {
                Time.timeScale = 0;
                GMScript.GameOver();
            }
        }
        
    }
    public IEnumerator Dizzy()
    {
        yield return new WaitForSeconds(3f);
        diz.SetActive(false);
        ChancePlay = 1;
    }
}
