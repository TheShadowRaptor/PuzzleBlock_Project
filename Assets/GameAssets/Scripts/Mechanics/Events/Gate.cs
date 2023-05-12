using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : EventObject
{
    Animator animator;
    public bool exitGate = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        animator.SetBool("false", true);
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
}
