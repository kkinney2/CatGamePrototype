using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public CamControl camControl;
    public Rigidbody Rb { get; private set; }


    [Range(1, 10)]
    public float moveSpeed = 6f;
    [Range(1, 15)]
    public float runSpeed = 8f;
    private float currentSetSpeed = 6f;
    private bool isRunning = false;
    private Vector3 moveDirection = Vector3.zero;
    public float floorOffsetY;
    public float rotateSpeed = 10f;
    public float jumpStrength = 5f;
    public float inputGravity = 1f;
    private float inputGravityStep = 0f;

    float inputAmount;
    Vector3 raycastFloorPos;
    Vector3 floorMovement;
    Vector3 gravity;
    Vector3 CombinedRaycast;

    public Transform groundChecker;
    public Transform collisionChecker;

    private int playerLayerMask;

    public InputMaster controls;
    private Vector2 playerInput;

    private void Awake()
    {
        if (!GetComponent<Rigidbody>())
            Debug.LogWarning("Rigidbody missing on " + gameObject.name);
        else
            Rb = GetComponent<Rigidbody>();

        if (camControl == null)
            Debug.LogWarning("Camera Controller Missing!!");

        if (animator != null)
            animator = GetComponent<Animator>();

        //rotationTarget = new GameObject().transform;
        //rotationTarget.transform.parent = gameObject.transform;
        //rotationTarget.name = "RotationTarget$$";
        //camControl.SetCameraTarget(rotationTarget);

        currentSetSpeed = moveSpeed;

        controls = new InputMaster(); // Needs to use the same InputMaster (One Instance)
        camControl.controls = controls;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Movement.performed += ctx => playerInput = ctx.ReadValue<Vector2>();
        controls.Player.Run.performed += ctx => Run();
        //controls.Player.Jump.performed += ctx => Jump();

        playerLayerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        playerLayerMask = ~playerLayerMask;
    }

    private void Update()
    {
        // reset movement
        moveDirection = Vector3.zero;

        // get vertical and horizontal movement input (controller and WASD/ Arrow Keys)
        //Vector2 playerInput = controls.Player.Movement.ReadValue<Vector2>();

        // base movement on camera and normalize
        moveDirection = CorrectMoveDirection(playerInput);

        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(playerInput.x) + Mathf.Abs(playerInput.y);
        inputAmount = Mathf.Clamp01(inputMagnitude);

        // rotate player to movement direction
        Rotate(moveDirection);
    }


    private void FixedUpdate()
    {
        // if not grounded , increase down force
        if (FloorRaycasts(0, 0, 0.6f) == Vector3.zero)
        {
            gravity += Vector3.up * Physics.gravity.y * Time.fixedDeltaTime;
        }

        // actual movement of the rigidbody + extra down force
        Rb.velocity = (moveDirection * currentSetSpeed * inputAmount) + gravity;

        // find the Y position via raycasts
        floorMovement = new Vector3(Rb.position.x, FindFloor().y + floorOffsetY, Rb.position.z);

        // only stick to floor when grounded
        if (FloorRaycasts(0, 0, 0.6f) != Vector3.zero && floorMovement != Rb.position)
        {
            // move the rigidbody to the floor
            Rb.MovePosition(floorMovement);
            gravity.y = 0;
        }
    }

    #region Floor Raycasting

    Vector3 FindFloor()
    {
        // width of raycasts around the centre of your character
        float raycastWidth = 0.25f;
        // check floor on 5 raycasts   , get the average when not Vector3.zero  
        int floorAverage = 1;

        CombinedRaycast = FloorRaycasts(0, 0, 1.6f);
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

        return CombinedRaycast / floorAverage;
    }

    // only add to average floor position if its not Vector3.zero
    int getFloorAverage(float offsetx, float offsetz)
    {

        if (FloorRaycasts(offsetx, offsetz, 1.6f) != Vector3.zero)
        {
            CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f);
            return 1;
        }
        else { return 0; }
    }


    Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
    {
        RaycastHit hit;
        // move raycast
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength))
        {
            return hit.point;
        }
        else return Vector3.zero;
    }

    #endregion

    private Vector3 CorrectMoveDirection(Vector2 direction)
    {
        Vector3 correctedVertical = direction.y * Camera.main.transform.forward;
        Vector3 correctedHorizontal = direction.x * Camera.main.transform.right;
        Vector3 correctedInput = correctedHorizontal + correctedVertical;
        correctedInput.Normalize();
        return new Vector3(correctedInput.x, 0, correctedInput.z);
    }

    private void Rotate(Vector3 moveDirection)
    {
        Quaternion rot = Quaternion.LookRotation(moveDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
        transform.rotation = targetRotation;
    }

    private void Run()
    {
        isRunning = !isRunning;

        if (isRunning)
            currentSetSpeed = runSpeed;
        else
            currentSetSpeed = moveSpeed;
    }
}


