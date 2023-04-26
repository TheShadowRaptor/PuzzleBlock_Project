using Cyan;
using System.Collections;
using UnityEngine;

public class PlayerController : BlockCharacter
{
    public static PlayerController playerController;

    [Header("UI")]
    [SerializeField] private Blit blit;
    [SerializeField] private Material shadowVignette;
    [SerializeField] private Material lightVignette;

    // Input Bools
    bool leftInput;
    bool rightInput;
    bool upInput;
    bool downInput;

    // Spring Varibles
    private bool hitSpring = false;
    private bool springingUp = false;
    private bool springingDown = false;
    private bool elevationSpring = false;
    private int blocksTraveled;
    private int maxBlocksToTravel;
    private Vector3 springDir;

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
        CheckInput();
        CheckSurroundingsWithRaycasts();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isMoving) return;

        if (canMoveLeft)
        {
            Assemble(-cameraDirectionRight);
        }
        else if (canMoveRight)
        {
            Assemble(cameraDirectionRight);
        }
        else if (canMoveForward)
        {
            Assemble(cameraDirection);
        }
        else if (canMoveBack)
        {
            Assemble(-cameraDirection);
        }

        canMoveLeft = false;
        canMoveRight = false;
        canMoveForward = false;
        canMoveBack = false;

        if (!IsGrounded() && !hitSpring) EnableGravity();
        if (hitSpring) BounceFromSpring();

        CalculateCameraVectors();
    }

    void Assemble(Vector3 dir)
    {
        var anchor = transform.position + (Vector3.down + dir) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    void CheckInput()
    {
        if (!IsGrounded()) return;
        if (hitSpring) return;
        if (isMoving) return;

        leftInput = false;
        rightInput = false;
        upInput = false;
        downInput = false;

        leftInput = Input.GetKey(KeyCode.A);
        rightInput = Input.GetKey(KeyCode.D);
        upInput = Input.GetKey(KeyCode.W);
        downInput = Input.GetKey(KeyCode.S);

        // Input
        if (leftInput)
        {
            lastDirInput = -cameraDirectionRight;
            canMoveLeft = true;
        }

        else if (rightInput)
        {
            lastDirInput = cameraDirectionRight;
            canMoveRight = true;
        }

        else if (upInput)
        {
            lastDirInput = cameraDirection;
            canMoveForward = true;
        }

        else if (downInput)
        {
            lastDirInput = -cameraDirection;
            canMoveBack = true;            
        }
    } 

    void BounceFromSpring()
    {
        float speed = 0.5f / 2;
        // Moves Player Upwards
        if (elevationSpring)
        {
            blocksTraveled++;
            if (blocksTraveled <= maxBlocksToTravel)
            {
                this.gameObject.transform.Translate(Vector3.up * speed, Space.World);
            }
            else
            {
                Assemble(springDir);
                hitSpring = false;
                springingDown = false;
                blocksTraveled = 0;
            }
        }

        // Move player Fowards
        else
        {
            Vector3 travelUp = Vector3.up / 4; // These values are mor for visual reasons. Stops the player from spring really high.
            Vector3 travelDown = Vector3.down / 4; // These values are mor for visual reasons. Stops the player from spring really high.


            blocksTraveled++;
            if (blocksTraveled <= maxBlocksToTravel)
            {
                if (springingUp)
                {
                    this.gameObject.transform.Translate(travelUp + springDir * speed, Space.World);
                    if (blocksTraveled == maxBlocksToTravel / 2) // If half
                    {
                        springingUp = false;
                        springingDown = true;
                    }
                }
                else if (springingDown)
                {
                    this.gameObject.transform.Translate(travelDown + springDir * speed, Space.World);
                }
            }
            else
            {
                hitSpring = false;
                springingDown = false;
                blocksTraveled = 0;
            }
        }

    }

    public void LandedOnSpring(Vector3 springDirection, int blocksToTravel, bool isElevationSpring)
    {
        elevationSpring = isElevationSpring;
        springDir = springDirection;
        maxBlocksToTravel = blocksToTravel * 4; // Player moves quarter a block. So this value is raised to make a whole block movement
        hitSpring = true;
        springingUp = true;
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

            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("Pushable") && leftInput && !isMoving) objectHit.GetComponent<IPushable>().Push(lastDirInput);
        }
        if (Physics.Raycast(transform.position, cameraDirectionRight, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveRight = false;

            rayCastHitting = true;

            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("Pushable") && rightInput && !isMoving) objectHit.GetComponent<IPushable>().Push(lastDirInput);
        }
        if (Physics.Raycast(transform.position, cameraDirection, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveForward = false;

            rayCastHitting = true;

            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("Pushable") && upInput && !isMoving) objectHit.GetComponent<IPushable>().Push(lastDirInput);
        }
        if (Physics.Raycast(transform.position, -cameraDirection, out hit, surroundingRayLength, detectTerrainMasks))
        {
            canMoveBack = false;

            rayCastHitting = true;

            GameObject objectHit = hit.collider.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer("Pushable") && downInput && !isMoving) objectHit.GetComponent<IPushable>().Push(lastDirInput);
        }
        // --------------------------------------------------------------------------------------------------------------------------

        IsGrounded();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Light")) blit.blitPass.blitMaterial = lightVignette;
        else blit.blitPass.blitMaterial = shadowVignette;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Light")) blit.blitPass.blitMaterial = shadowVignette;
        else blit.blitPass.blitMaterial = lightVignette;
    }
}
