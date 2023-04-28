using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : EventObject
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("false", true);
    }

    public override void PlayEvent()
    {
        // Open Gate
        animator.SetBool("Open", true);     
    }

    public override void CancelEvent()
    {
        animator.SetBool("Open", false);
    }
}
