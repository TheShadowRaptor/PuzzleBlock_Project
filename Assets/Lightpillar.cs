using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightpillar : EventObject
{
    [SerializeField] float maxLightRange;

    private Light light;
    private GameObject lightSize;
    private float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        lightSize = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            PlayEvent();
        }
    }

    protected override void PlayEvent()
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

    
}
