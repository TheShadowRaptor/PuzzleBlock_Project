using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : EventObject
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("false", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) PlayEvent();
    }

    protected override void PlayEvent()
    {
        // Open Gate
        animator.SetBool("Open", true);     
    }
}
