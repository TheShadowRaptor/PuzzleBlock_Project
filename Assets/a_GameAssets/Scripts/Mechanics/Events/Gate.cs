using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defective.JSON;

public class Gate : EventObject
{
    Animator animator;
    public bool exitGate = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        animator.SetBool("false", true);
        base.Start();
    }

    private void Update()
    {
        if (exitGate)
        {
            if (LightEmitter.IsInLightRange(transform.position)) PlayEvent();
            else CancelEvent();
        }
        
    }

    public override void PlayEvent()
    {
        // Open Gate
        if (exitGate)
        {
            animator.SetBool("OpenExit", true);
        }
        else animator.SetBool("Open", true);
    }

    public override void CancelEvent()
    {
        if (exitGate)
        {
            animator.SetBool("OpenExit", false);
        }
        else animator.SetBool("Open", false);
    }

    public override bool MiddleMouseEvent()
    {
        gameObject.transform.Rotate(0,90,0);
        return true;
    }

    public override void Serialize(JSONObject jsonObject)
    {
        base.Serialize(jsonObject);

        // Save Event info
        if (this.gameObject != null)
        {
            // Save to eventInformation
            jsonObject.SetField("yRotation", gameObject.transform.eulerAngles.y);
        }
    }

    public override void DeSerialize(JSONObject jsonObject)
    {
        base.DeSerialize(jsonObject);

        // Load Event info
        jsonObject.GetField(out float y, "yRotation", 0);
        gameObject.transform.rotation = Quaternion.Euler(0, y,0);
    }
}
