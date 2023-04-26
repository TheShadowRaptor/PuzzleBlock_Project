using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BlockCharacter, IPushable
{
    private bool isBeingPushed = false;
    private Vector3 direction;

    // Update is called once per frame
    private void Update()
    {
        CheckSurroundingsWithRaycasts();
    }

    void FixedUpdate()
    {
        Debug.Log(transform.position);
        if (isBeingPushed && CanMoveThisDir())
        {
            direction /= 16;
            for (int i = 0; i < 8; i++)
            {                
                gameObject.transform.Translate(direction.x, -direction.z, 0);
            }
            isBeingPushed = false;
        }
    }

    void IPushable.Push(Vector3 playerPushDir)
    {
        direction = playerPushDir;
        isBeingPushed = true;
    }

    bool CanMoveThisDir()
    {
        PlayerController playerController = PlayerController.playerController;
        if (direction == -playerController.transform.right && canMoveLeft)
        {
            return true;

        }
        else if (direction == playerController.transform.right && canMoveRight)
        {
            return true;
        }
        else if (direction == playerController.transform.forward && canMoveForward)
        {
            return true;
        }
        else if (direction == -playerController.transform.forward && canMoveBack)
        {
            return true;
        }
        else return false;
    }

    protected override void CheckSurroundingsWithRaycasts()
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
}
