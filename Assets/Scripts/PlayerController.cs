using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 3;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) return;

        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.x = 45;
        cameraDirection.y = 0f;
        cameraDirection.Normalize();

        Vector3 cameraDirectionRight = Camera.main.transform.right;
        cameraDirectionRight.y = 0f;
        cameraDirectionRight.x = 0;
        cameraDirectionRight.Normalize();


        if (Input.GetKey(KeyCode.A) && isMoving == false) Assemble(-cameraDirectionRight);
        if (Input.GetKey(KeyCode.D) && isMoving == false) Assemble(cameraDirectionRight);
        if (Input.GetKey(KeyCode.W) && isMoving == false) Assemble(cameraDirection);
        if (Input.GetKey(KeyCode.S) && isMoving == false) Assemble(-cameraDirection);

        void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));            
        }
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
