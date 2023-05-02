using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool space;
    private bool y;
    public bool Space { get => space; }
    public bool Y { get => y; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        space = Input.GetKey(KeyCode.Space);
        y = Input.GetKeyDown(KeyCode.Y);

        if (y)
        {
            MasterSingleton.Instance.Player.TakeDamage(1);
        }
    }
}
