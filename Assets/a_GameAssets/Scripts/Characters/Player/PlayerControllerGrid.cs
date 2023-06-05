using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerControllerGrid : BlockCharacter
{
    protected void Start()
    {
        startTimeTillNextInteract = timeUntilNextInteract;
        startHealth = health;
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = cellPos;
        _currentCell = GridCell.GetCell(cellPos);
    }

    public void Update()
    {
        if (MasterSingleton.Instance.InputManager.Esc && MasterSingleton.Instance.GameManager.State == GameManager.GameState.gameplay && MasterSingleton.Instance.LevelManager.CurrentScene != "LevelBuilder")
        {
            MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.paused);
            MasterSingleton.Instance.UIManager.HideGameplayMenu();
            MasterSingleton.Instance.UIManager.ShowPauseMenu();
            return;
        }
        else if ((MasterSingleton.Instance.InputManager.Esc && MasterSingleton.Instance.GameManager.State == GameManager.GameState.gameplay && MasterSingleton.Instance.LevelManager.CurrentScene == "LevelBuilder"))
        {
            //LevelArea.Instance.ReloadTiles();
            LevelArea.Instance.ReloadTiles();
            MasterSingleton.Instance.LevelManager.SpawnPlayer();
            MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.edit);
            MasterSingleton.Instance.UIManager.SwitchToBuilder();
        }

        else if (MasterSingleton.Instance.InputManager.Esc && MasterSingleton.Instance.GameManager.State != GameManager.GameState.gameplay && MasterSingleton.Instance.GameManager.State != GameManager.GameState.mainmenu)
        {
            MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
            MasterSingleton.Instance.UIManager.SwitchToGameplay(true, false);
            return;
        }

        if (MasterSingleton.Instance.GameManager.State != GameManager.GameState.gameplay) return;

        //if (LightEmitter.IsInLightRange(transform.position)) MasterSingleton.Instance.AudioManager.SwapMusic(AudioManager.Sound.SoundName.gameplayLight);
        //else MasterSingleton.Instance.AudioManager.SwapMusic(AudioManager.Sound.SoundName.gameplayDark);

        if (MasterSingleton.Instance.InputManager.R) TakeDamage(int.MaxValue);

        if (IsAlive == false)
        {
            if (FinishedDying() == true)
            {
                LevelArea.Instance.ReloadTiles();
                MasterSingleton.Instance.LevelManager.SpawnPlayer();

            }
        }
        CanInteract();
        DetectChangeInLight();
        Move();
        
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.A)) A = true;
        else if (Input.GetKey(KeyCode.W)) W = true;
        else if (Input.GetKey(KeyCode.S)) S = true;
        else if (Input.GetKey(KeyCode.D)) D = true;

        if ((D || A || W || S) && !CameraController.isRotating)
        {
             Camera mainCamera = Camera.main;
            // Vector3 cameraRight = mainCamera.transform.right; Vector3 cameraUp = mainCamera.transform.up;
            //
            // actualForward = Vector3.Lerp(cameraUp, cameraRight, 0.5f);
            // actualForward.y = 0;
            // actualForward = actualForward.normalized;
            //
            
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0; // Project onto XZ plane
            cameraForward.Normalize();
            
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            cameraForward = rotation * cameraForward;

            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0; // Project onto XZ plane
            cameraRight = rotation * cameraRight;
            cameraRight.Normalize();
            int vertical = 0;
            if (W || S) vertical = W ? 1 : -1;
            int horizontal = 0;
            if (D || A) horizontal = D ? 1 : -1;
            
            Vector3 moveDirection = ((cameraForward * vertical) + (cameraRight * horizontal)).normalized;
            Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3Int movementRounded = Vector3Int.RoundToInt(movement);
            if (movementRounded.x == movementRounded.z) movementRounded.x = 0;
            DoMove(movementRounded);
        }

        W = false;
        S = false;
        D = false;
        A = false;
    }

    public bool W, A, S, D;
    public void LateUpdate()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 12);
    }

    public override Vector3 GetPosition() { return transform.position; }

}
