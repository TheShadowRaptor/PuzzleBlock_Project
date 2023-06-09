using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BlockCharacter
{
    void Move()
    {
        // Take move commands
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            Vector3 camRightDirection = Camera.main.transform.right;
            camRightDirection.y = 0;
            //WANTS TO MOVE IN THAT DIRECTION'S X
            if (Mathf.Abs(camRightDirection.x) > Mathf.Abs(camRightDirection.z))
            {
                //Ignore Z direction because X is bigger
                camRightDirection.z = 0;
                camRightDirection.Normalize();
                //We now should have a vector direction that should conform to the right of the current camera.
                //So we'll multiply the x by -1 if we're trying to go left. and by 1 if we're trying to go right to keep the vector the same.
                camRightDirection.x *= (Input.GetKey(KeyCode.A) ? -1 : 1);
            }
            else
            {
                //WANTS TO MOVE IN THAT DIRECTION'S Z
                //Ignore X direction because Z is bigger
                camRightDirection.x = 0;
                camRightDirection.Normalize();
                //We now should have a vector direction that should conform to the right of the current camera.
                //So we'll multiply the z by -1 if we're trying to go left. and by 1 if we're trying to go right to keep the vector the same.
                camRightDirection.z *= (Input.GetKey(KeyCode.A) ? -1 : 1);
            }
            DoMove(new Vector3Int((int)camRightDirection.x, 0, (int)camRightDirection.z));
        }

        //Take move commands
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            Vector3 camForwardDirection = Camera.main.transform.forward;
            camForwardDirection.y = 0;
            //WANTS TO MOVE IN THAT DIRECTION'S X
            if (Mathf.Abs(camForwardDirection.x) > Mathf.Abs(camForwardDirection.z))
            {
                //Ignore Z direction because X is bigger
                camForwardDirection.z = 0;
                camForwardDirection.Normalize();
                //We now should have a vector direction that should conform to the Forward of the current camera.
                //So we'll multiply the x by -1 if we're trying to go left. and by 1 if we're trying to go Forward to keep the vector the same.
                camForwardDirection.x *= (Input.GetKey(KeyCode.S) ? -1 : 1);
            }
            else
            {
                //WANTS TO MOVE IN THAT DIRECTION'S Z
                //Ignore X direction because Z is bigger
                camForwardDirection.x = 0;
                camForwardDirection.Normalize();
                //We now should have a vector direction that should conform to the Forward of the current camera.
                //So we'll multiply the z by -1 if we're trying to go left. and by 1 if we're trying to go Forward to keep the vector the same.
                camForwardDirection.z *= (Input.GetKey(KeyCode.S) ? -1 : 1);
            }
            DoMove(new Vector3Int((int)camForwardDirection.x, 0, (int)camForwardDirection.z));
        }
    }

    public void LateUpdate()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 12);
    }

    public override Vector3 GetPosition() { return transform.position; }
}
