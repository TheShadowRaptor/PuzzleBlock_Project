using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    [SerializeField] private GameObject cameraPointHolder;
    [SerializeField] private List<GameObject> cameraPoints = new List<GameObject>();

    [SerializeField] private int currentCameraPoint = 0;
    [SerializeField] private float cameraPanSpeed = 0.1f;

    public List<GameObject> CameraPoints { get =>  cameraPoints; }
    public int CurrentCameraPoint { get => currentCameraPoint; }

    private void Awake()
    {
        if (cameraController != null && cameraController != this)
        {
            Destroy(this);
            return;
        }
        cameraController = this;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraPosition();
        FollowPlayer();
    }
    
    void ChangeCameraPosition()
    {
        // Change currentCameraPoint
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentCameraPoint == 0) currentCameraPoint = cameraPoints.Count - 1;
            else currentCameraPoint -= 1;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCameraPoint == cameraPoints.Count - 1) currentCameraPoint = 0;
            else currentCameraPoint += 1;
        }

        // Camera lerps to currentCameraPoint untill it reaches it
        Vector3 cameraPos = this.gameObject.transform.position;
        Quaternion cameraRot = this.gameObject.transform.rotation;

        if (cameraPos != cameraPoints[currentCameraPoint].transform.position)
        {
            cameraPos = Vector3.Lerp(cameraPos, cameraPoints[currentCameraPoint].transform.position, cameraPanSpeed);   
            this.gameObject.transform.position = cameraPos;
        }

        if (cameraRot != cameraPoints[currentCameraPoint].transform.rotation)
        {
            cameraRot = Quaternion.Lerp(cameraRot, cameraPoints[currentCameraPoint].transform.rotation, cameraPanSpeed);   
            this.gameObject.transform.rotation = cameraRot;
        }
    }

    void FollowPlayer()
    {
        //if (!PlayerController.playerController.IsMoving) return;
        Vector3 pointPos = cameraPointHolder.transform.position;
        Vector3 playerPos = PlayerController.playerController.gameObject.transform.position;
        pointPos.x = playerPos.x;
        pointPos.z = playerPos.z;
        cameraPointHolder.transform.position = pointPos;
    }
}
