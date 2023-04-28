using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightpillar : EventObject, ICellOccupier
{
    public bool IsSolid { get => true; set { } }
    [SerializeField] float maxLightRange;

    private Light light;
    private GameObject lightSize;
    private float speed = 2;

    bool playEvent = false;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        lightSize = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, light.range);

        foreach (Collider collider in colliders)
        {
            // Check if the collider is not part of the point light's own GameObject
            if (collider.gameObject == PlayerControllerGrid.playerControllerGrid.gameObject)
            {
                Debug.Log("Point light is touching " + collider.gameObject.name);
            }
        }

        if (playEvent) 
        {
            Vector3 scale = lightSize.transform.localScale;
            if (light.range < maxLightRange)
            {
                light.range += speed * Time.deltaTime;
                scale.x += speed * Time.deltaTime;
                scale.y += speed * Time.deltaTime;
                scale.z += speed * Time.deltaTime;
                lightSize.transform.localScale = scale;
            }
        }

        else if (!playEvent)
        {

        }
    }


    public override void PlayEvent()
    {
        playEvent = true;
    }

    public override void CancelEvent()
    {
        playEvent = false;
    }

    public virtual Vector3 GetPosition() { return transform.position; }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
       
    }

    public void BlockExitHere(BlockCharacter exited)
    {

    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {
        Debug.Log("Attempted");
        PlayEvent();
    }
}
