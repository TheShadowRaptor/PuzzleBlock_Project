using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using Unity.VisualScripting;
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
    [SerializeField] private float cameraMoveDelay = 1f;
    [SerializeField] private float startCameraMoveDelay = 1f;

    [Header("TitleCameraSettings")]
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private Vector3 offset;

    [Header("EditorCameraSettings")]
    [SerializeField] private float editorCameraMoveSpeed = 1;

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

    private void Start()
    {
        startCameraMoveDelay = cameraMoveDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (MasterSingleton.Instance.UIManager.isFocused) return;

        if (MasterSingleton.Instance.GameManager.State == GameManager.GameState.edit)
        {
            ControlEditorCamera();
        }

        if (MasterSingleton.Instance.GameManager.State == GameManager.GameState.gameplay)
        {
            ChangeCameraPosition();
            FollowPlayer();
            CameraCanMove();
        }

        if (MasterSingleton.Instance.GameManager.State == GameManager.GameState.mainmenu)
        {
            OrbitCamera();
        }
    }
    
    void ChangeCameraPosition()
    {
        // Change currentCameraPoint
        if (Input.GetKeyDown(KeyCode.Q) && CameraCanMove())
        {
            cameraMoveDelay = startCameraMoveDelay;
            if (currentCameraPoint == 0) currentCameraPoint = cameraPoints.Count - 1;
            else currentCameraPoint -= 1;
            isRotating = true;
            transform.DOKill();
            transform.DORotate(cameraPoints[currentCameraPoint].transform.rotation.eulerAngles, cameraRotationDuration)
                .SetEase(Ease.Linear).OnComplete(
                    () => { isRotating = false; });
        }

        if (Input.GetKeyDown(KeyCode.E) && CameraCanMove())
        {
            cameraMoveDelay = startCameraMoveDelay;
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

    public void ReAlignGameCamera()
    {
        // Camera will not be in right spot on playtest. This fixes that.
        if (this.gameObject.transform.rotation != cameraPoints[currentCameraPoint].transform.rotation)
        {
            cameraMoveDelay = startCameraMoveDelay;
            isRotating = true;
            transform.DOKill();
            transform.DORotate(cameraPoints[currentCameraPoint].transform.rotation.eulerAngles, cameraRotationDuration)
                .SetEase(Ease.Linear).OnComplete(
                    () => { isRotating = false; });
        }
    }

    public bool CameraCanMove()
    {
        cameraMoveDelay -= Time.deltaTime;

        if (cameraMoveDelay <= 0)
        {
            cameraMoveDelay = 0;
            return true;
        }
        return false;
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

    void OrbitCamera()
    {
        Vector3 orbitCamera = transform.position;
        orbitCamera.y = 8;
        transform.position = orbitCamera;

        //transform.position = target.position + offset;
        if (target != null)
        {
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
            //transform.LookAt(target.position);
        }
        else
        {
            transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
            try
            {
                target = GameObject.Find("Target").transform;
            }
            catch { }
        }
    }

    void ControlEditorCamera()
    {
        InputManager input = MasterSingleton.Instance.InputManager;
        // -----------------------------------
        Vector3 cameraForward = transform.forward;
        cameraForward.y = 0; // Project onto XZ plane
        cameraForward.Normalize();

        Quaternion cameraRotation = transform.rotation;
        Vector3 cameraEulerAngles = cameraRotation.eulerAngles; // Get the current Euler angles of the camera

        if (Input.GetKey(KeyCode.E))
        {
            float rotateSpeed = 50.0f; 
            cameraEulerAngles.y += rotateSpeed * Time.deltaTime; // Increment the Y angle to rotate the camera around the Y-axis
        }
        if (Input.GetKey(KeyCode.Q))
        {
            float rotateSpeed = 50.0f;
            cameraEulerAngles.y -= rotateSpeed * Time.deltaTime; // Decrement the Y angle to rotate the camera around the Y-axis
        }

        cameraRotation = Quaternion.Euler(cameraEulerAngles);

        //cameraForward = rotation * cameraForward;

        Vector3 cameraRight = transform.right;
        cameraRight.y = 0; // Project onto XZ plane
        //cameraRight = rotation * cameraRight;
        cameraRight.Normalize();

        int vertical = 0;
        if (input.W || input.S) vertical = input.W ? 1 : -1;
        int horizontal = 0;
        if (input.D || input.A) horizontal = input.D ? 1 : -1;

        Vector3 moveDirection = ((cameraForward * vertical) + (cameraRight * horizontal)).normalized;
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.z);

        transform.position += movement * Time.deltaTime * editorCameraMoveSpeed;
        transform.rotation = cameraRotation;

        
        // -----------------------------------
    }
}
