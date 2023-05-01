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
