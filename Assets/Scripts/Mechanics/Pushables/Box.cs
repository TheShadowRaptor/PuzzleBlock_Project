using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IPushable
{
    private bool isMoving = false;
    private Vector3 direction;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            for (int i = 0; i < 8; i++)
            {
                gameObject.transform.Translate(direction);
            }
            isMoving = false;
        }
    }

    void IPushable.Push(Vector3 playerPushDir)
    {
        direction = playerPushDir / 16;
        isMoving = true;
    }
}
