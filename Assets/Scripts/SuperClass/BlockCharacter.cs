using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public abstract class BlockCharacter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float gravityPower = 3;

    [Header("Rays")]
    [SerializeField] protected LayerMask detectTerrainMasks;
    [SerializeField] protected float surroundingRayLength = 1.2f;
    [SerializeField] protected float groundRayLength = 1.01f;

    public bool rayCastHitting;

    // Movement bools
    protected bool canMoveForward;
    protected bool canMoveBack;
    protected bool canMoveLeft;
    protected bool canMoveRight;

    protected Vector3 lastDirInput;

    // Camera Vectors
    protected Vector3 cameraDirection;
    protected Vector3 cameraDirectionRight;
    protected bool isMoving;

    public bool IsMoving { get => isMoving; }
    public Vector3 CameraDirection { get => cameraDirection; }
    public Vector3 CameraDirectionRight { get => cameraDirectionRight; }

    protected void CalculateCameraVectors()
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

    protected virtual void CheckSurroundingsWithRaycasts()
    {
        RaycastHit hit;

        rayCastHitting = false;

        // Grid --------------------------------------------------------------------------------------------------------------------
        if (Physics.Raycast(transform.position, -cameraDirectionRight, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveLeft = false;

            rayCastHitting = true;
        }
        if (Physics.Raycast(transform.position, cameraDirectionRight, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveRight = false;

            rayCastHitting = true;
        }
        if (Physics.Raycast(transform.position, cameraDirection, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveForward = false;

            rayCastHitting = true;
        }
        if (Physics.Raycast(transform.position, -cameraDirection, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveBack = false;

            rayCastHitting = true;
        }
        // --------------------------------------------------------------------------------------------------------------------------

        IsGrounded();
    }

    protected void EnableGravity()
    {
        this.gameObject.transform.Translate(Vector3.down * gravityPower, Space.World);
    }

    protected bool IsGrounded()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, groundRayLength);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundRayLength, detectTerrainMasks)) return true;
        else return false;
    }

    protected IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;

        for (int i = 0; i < (90 / speed); i++)
        {
            transform.RotateAround(anchor, axis, speed);
            yield return new WaitForSeconds(0.01f);
        }

        isMoving = false;
    }
}
