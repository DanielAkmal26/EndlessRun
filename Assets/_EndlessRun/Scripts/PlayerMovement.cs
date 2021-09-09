using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;
    public int speed;
    public int turnSpeed;
    private int desiredLane = 1; //Posisi Laning (0 = Left, 1 = Mid, 2 = Right)
    void Start()
    {

    }

    void Update()
    {
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

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }


        //transform.Translate(Vector3.forward / speedForce);
        Vector3 moveVector = Vector3.zero;
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
}
