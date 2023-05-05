using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    [SerializeField] private GameObject cameraPointHolder;
    [SerializeField] private List<GameObject> cameraPoints = new List<GameObject>();

    [SerializeField] private int currentCameraPoint = 0;
    [SerializeField] private float cameraPanSpeed = 0.1f;
    [SerializeField] private float cameraPanSpeedDuringRotation = 0.1f;
    [SerializeField] private float cameraRotationDuration = 0.25f;
    [SerializeField] private float cameraHeight = 10f;

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
            isRotating = true;
            transform.DOKill();
            transform.DORotate(cameraPoints[currentCameraPoint].transform.rotation.eulerAngles, cameraRotationDuration)
                .SetEase(Ease.Linear).OnComplete(
                    () => { isRotating = false; });
        }

        // Camera lerps to currentCameraPoint untill it reaches it
        Vector3 cameraPos = this.gameObject.transform.position;

        if (cameraPos != cameraPoints[currentCameraPoint].transform.position)
        {
            cameraPos = Vector3.Lerp(cameraPos, cameraPoints[currentCameraPoint].transform.position, isRotating ? cameraPanSpeedDuringRotation : cameraPanSpeed);   
            this.gameObject.transform.position = cameraPos;
        }
    }

    public static bool isRotating = false;
    void FollowPlayer()
    {
        Vector3 pointPos = cameraPointHolder.transform.position;
        Vector3 playerPos = MasterSingleton.Instance.Player.gameObject.transform.position;
        //if (!PlayerController.playerController.IsMoving) return;
        pointPos.x = playerPos.x;
        pointPos.z = playerPos.z;
        pointPos.y = playerPos.y + cameraHeight;
        cameraPointHolder.transform.position = pointPos;
    }
}
