using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool space;
    private bool esc;
    private bool r;
    private bool w;
    private bool a;
    private bool s;
    private bool d;

    public bool Space { get => space; }
    public bool Esc { get => esc; }
    public bool R { get => r; }
    public bool W { get => w; }
    public bool A { get => a; }
    public bool S { get => s; }
    public bool D { get => d; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        space = Input.GetKey(KeyCode.Space);
        esc = Input.GetKeyDown(KeyCode.Escape);
        r = Input.GetKey(KeyCode.R);
        w = Input.GetKey(KeyCode.W);
        a = Input.GetKey(KeyCode.A);
        s = Input.GetKey(KeyCode.S);
        d = Input.GetKey(KeyCode.D);
    }
}
