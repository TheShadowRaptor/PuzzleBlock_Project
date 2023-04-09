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


    private bool isMoving;
    private bool canMoveForward;
    private bool canMoveBack;
    private bool canMoveLeft;
    private bool canMoveRight;

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
        Move();
    }

    void Move()
    {
        if (isMoving) return;

        Vector3 cameraDirection = CameraController.cameraController.CameraPoints[CameraController.cameraController.CurrentCameraPoint].transform.forward;
        cameraDirection.y = 0f;


        Vector3 cameraDirectionRight = CameraController.cameraController.CameraPoints[CameraController.cameraController.CurrentCameraPoint].transform.right;
        cameraDirectionRight.y = 0f;

        // Rotate cameraDirection by 45 degrees around the y-axis
        cameraDirection = Quaternion.AngleAxis(45f, Vector3.up) * cameraDirection;
        cameraDirectionRight = Quaternion.AngleAxis(45f, Vector3.up) * cameraDirectionRight;

        cameraDirection.Normalize();
        cameraDirectionRight.Normalize();

        CheckSurroundingsWithRaycasts(cameraDirection, cameraDirectionRight);

        Debug.Log(cameraDirection.x);

        // Input
        if (Input.GetKey(KeyCode.A) && isMoving == false && canMoveLeft && IsGrounded()) Assemble(-cameraDirectionRight);
        if (Input.GetKey(KeyCode.D) && isMoving == false && canMoveRight && IsGrounded()) Assemble(cameraDirectionRight);
        if (Input.GetKey(KeyCode.W) && isMoving == false && canMoveForward && IsGrounded()) Assemble(cameraDirection);
        if (Input.GetKey(KeyCode.S) && isMoving == false && canMoveBack && IsGrounded()) Assemble(-cameraDirection);

        if (!IsGrounded()) EnableGravity();

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }
    }

    void CheckSurroundingsWithRaycasts(Vector3 cameraDirection, Vector3 cameraDirectionRight)
    {
        RaycastHit hit;
        canMoveForward = true;
        canMoveBack = true;
        canMoveLeft = true;
        canMoveRight = true;

        // Grid
        if (Physics.Raycast(transform.position, -cameraDirectionRight, out hit, 1.2f, unpassableTerrainMasks)) canMoveLeft = false;
        if (Physics.Raycast(transform.position, cameraDirectionRight, out hit, 1.2f, unpassableTerrainMasks)) canMoveRight = false;
        if (Physics.Raycast(transform.position, cameraDirection, out hit, 1.2f, unpassableTerrainMasks)) canMoveForward = false;
        if (Physics.Raycast(transform.position, -cameraDirection, out hit, 1.2f, unpassableTerrainMasks)) canMoveBack = false;
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, unpassableTerrainMasks)) return true;
        else return false;
    }

    void EnableGravity()
    {
        this.gameObject.transform.Translate(Vector3.down * gravityPower, Space.World);
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

}
