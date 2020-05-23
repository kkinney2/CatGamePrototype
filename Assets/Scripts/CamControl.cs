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
    public float sensitivity_Y;

    public Vector2 pitchMinMax = new Vector2(-30, 85);
    public float acceleration = .12f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public InputMaster controls;
    private Vector2 playerInput;

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

        //controls = new InputMaster(); // Needs to use the same InputMaster (One Instance)
        controls.Player.Camera.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void LateUpdate()
    {
        Move();

        // Seems like over correction to me
        // JK THIS IS HIGHLY NEEDED
        //rotationTarget.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

    }

    private void Move()
    {
        transform.position = target.transform.position;
    }

    private void Rotate()
    {
        // <<-----------------------------------------------------------------------------**
        // Get input data and clamp 
        // **---------------------------------------------**

        yaw += playerInput.x * sensitivity_X;

        if (invertPitch)
        {
            pitch += playerInput.y * sensitivity_Y;
        }

        else
        {
            pitch += playerInput.y * -sensitivity_Y;
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
