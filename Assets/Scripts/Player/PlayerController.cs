using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    [Header("Settings")]
    [SerializeField] private LayerMask unpassableTerrainMasks;
    [SerializeField] private float rollSpeed = 3;
    [SerializeField] private float gravityPower = 3;

    [Header("Rays")]
    [SerializeField] private float surroundingRayLength = 1.2f;
    [SerializeField] private float groundRayLength = 1.01f;

    // Movement bools
    private bool isMoving;
    private bool canMoveForward;
    private bool canMoveBack;
    private bool canMoveLeft;
    private bool canMoveRight;

    Vector3 lastDirMoved;

    private bool hitSpring = false;
    private float springPower;
    private float springDuration;

    // Camera Vectors
    private Vector3 cameraDirection;
    private Vector3 cameraDirectionRight;

    public bool IsMoving { get => isMoving; }

    private void Awake()
    {
        if (playerController != null && playerController != this)
        {
            Destroy(this);
            return;
        }
        playerController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurroundingsWithRaycasts(cameraDirection, cameraDirectionRight);
        CheckInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isMoving) return;

        if (!IsGrounded() && !hitSpring) EnableGravity();
        if (hitSpring) BounceFromSpring();

        CalculateCameraVectors();
    }

    void Assemble(Vector3 dir)
    {
        var anchor = transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    void CheckInput()
    {
        if (!IsGrounded()) return;
        if (isMoving) return;
        if (hitSpring) return;

        // Input
        if (Input.GetKey(KeyCode.A) && canMoveLeft)
        {
            Assemble(-cameraDirectionRight); 
            lastDirMoved = -cameraDirectionRight;
        }

        else if (Input.GetKey(KeyCode.D) && canMoveRight)
        {
            Assemble(cameraDirectionRight); 
            lastDirMoved = cameraDirectionRight;
        }

        else if (Input.GetKey(KeyCode.W) && canMoveForward)
        {
            Assemble(cameraDirection); lastDirMoved = cameraDirection;
        }

        else if (Input.GetKey(KeyCode.S) && canMoveBack)
        {
            Assemble(-cameraDirection); lastDirMoved = -cameraDirection;
        }
    }

    void CalculateCameraVectors()
    {
        cameraDirection = CameraController.cameraController.CameraPoints[CameraController.cameraController.CurrentCameraPoint].transform.forward;
        cameraDirection.y = 0f;

        cameraDirectionRight = CameraController.cameraController.CameraPoints[CameraController.cameraController.CurrentCameraPoint].transform.right;
        cameraDirectionRight.y = 0f;

        // Rotate cameraDirection by 45 degrees around the y-axis
        cameraDirection = Quaternion.AngleAxis(45f, Vector3.up) * cameraDirection;
        cameraDirectionRight = Quaternion.AngleAxis(45f, Vector3.up) * cameraDirectionRight;

        cameraDirection.Normalize();
        cameraDirectionRight.Normalize();
    }

    void CheckSurroundingsWithRaycasts(Vector3 cameraDirection, Vector3 cameraDirectionRight)
    {
        RaycastHit hit;
        canMoveForward = true;
        canMoveBack = true;
        canMoveLeft = true;
        canMoveRight = true;

        // Grid
        if (Physics.Raycast(transform.position, -cameraDirectionRight, out hit, surroundingRayLength, unpassableTerrainMasks)) canMoveLeft = false;
        if (Physics.Raycast(transform.position, cameraDirectionRight, out hit, surroundingRayLength, unpassableTerrainMasks)) canMoveRight = false;
        if (Physics.Raycast(transform.position, cameraDirection, out hit, surroundingRayLength, unpassableTerrainMasks)) canMoveForward = false;
        if (Physics.Raycast(transform.position, -cameraDirection, out hit, surroundingRayLength, unpassableTerrainMasks)) canMoveBack = false;
    }

    void EnableGravity()
    {
        this.gameObject.transform.Translate(Vector3.down * gravityPower, Space.World);
    }

    void BounceFromSpring()
    {
        springDuration -= Time.deltaTime;

        if (springDuration > 0) this.gameObject.transform.Translate(Vector3.up * springPower, Space.World);
        else
        {
            springDuration = 0;
            Assemble(lastDirMoved);
            hitSpring = false;
        }
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, groundRayLength);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundRayLength, unpassableTerrainMasks)) return true;
        else return false;
    }

    IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;

        for (int i = 0; i < (90 / rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        isMoving = false;
    }

    public void LandedOnSpring(float power, float duration)
    {
        springPower = power;
        springDuration = duration;
        hitSpring = true;
    }
}
