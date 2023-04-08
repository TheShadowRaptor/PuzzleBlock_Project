using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    [SerializeField] private LayerMask unpassableTerrainMasks;
    [SerializeField] private float rollSpeed = 3;
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

        CheckIfCanMove(cameraDirection, cameraDirectionRight);

        Debug.Log(cameraDirection.x);

        if (Input.GetKey(KeyCode.A) && isMoving == false && canMoveLeft) Assemble(-cameraDirectionRight);
        if (Input.GetKey(KeyCode.D) && isMoving == false && canMoveRight) Assemble(cameraDirectionRight);
        if (Input.GetKey(KeyCode.W) && isMoving == false && canMoveForward) Assemble(cameraDirection);
        if (Input.GetKey(KeyCode.S) && isMoving == false && canMoveBack) Assemble(-cameraDirection);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));            
        }

    }

    void CheckIfCanMove(Vector3 cameraDirection, Vector3 cameraDirectionRight)
    {
        RaycastHit hit;
        canMoveForward = true;
        canMoveBack = true;
        canMoveLeft = true;
        canMoveRight = true;

        if (Physics.Raycast(transform.position, -cameraDirectionRight, out hit, 1.2f, unpassableTerrainMasks)) canMoveLeft = false;
        if (Physics.Raycast(transform.position, cameraDirectionRight, out hit, 1.2f, unpassableTerrainMasks)) canMoveRight = false;
        if (Physics.Raycast(transform.position, cameraDirection, out hit, 1.2f, unpassableTerrainMasks)) canMoveForward = false;
        if (Physics.Raycast(transform.position, -cameraDirection, out hit, 1.2f, unpassableTerrainMasks)) canMoveBack = false;
            
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
