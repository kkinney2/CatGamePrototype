using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Camera myCamera;
    public GameObject target;

    private Transform rotationTarget;

    private bool invertPitch = false;
    public float sensitivity_X;
    //10
    public float sensitivity_Y;
    //10
    private float horizontalInput;
    private float verticalInput;

    public Vector2 pitchMinMax = new Vector2(-30, 85);
    public float acceleration = .12f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public bool lockPosition = false;

    float yaw;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        if (myCamera == null)
        {
            Debug.LogWarning("No assigned camera found. Defaulting to MAIN");
            myCamera = Camera.main;
        }

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        currentRotation = transform.eulerAngles;
    }

   
    

    private void FixedUpdate()
    {
        if (!lockPosition)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (!lockPosition)
        {
            transform.position = target.transform.position;
        }

        // Seems like over correction to me
        // JK THIS IS HIGHLY NEEDED
        rotationTarget.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

    }

    //private void GetInput()
    //{
    //    horizontalInput = Input.GetAxisRaw(gameController.camXInput);
    //    verticalInput = Input.GetAxisRaw(gameController.camYInput);
    //}

    private void Move()
    {
        // <<-----------------------------------------------------------------------------**
        // Get input data and clamp 
        // **---------------------------------------------**

        //yaw += Input.GetAxis(gameController.camXInput) * sensitivity_X;

        if (invertPitch)
        {
            //pitch += Input.GetAxis(gameController.camYInput) * sensitivity_Y;
        }

        else
        {
            //pitch += Input.GetAxis(gameController.camYInput) * -sensitivity_Y;
        }

        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // **----------------------------------------------------------------------------->>


        // <<-----------------------------------------------------------------------------**
        // Calculate rotation and apply to camera
        // **---------------------------------------------**

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, acceleration * Time.deltaTime);

        transform.eulerAngles = currentRotation;

        // **----------------------------------------------------------------------------->>
    }

    public void SetCameraTarget(Transform target)
    {
        rotationTarget = target;
    }

   
}
