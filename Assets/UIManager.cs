using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        mainmenu,
        controls,
        gameplay,
        win
    }

    private UIState state;
    public UIState State { get => state; }
    // Start is called before the first frame update
    void Start()
    {
        state = UIState.mainmenu;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case UIState.mainmenu:
                break;

            case UIState.controls:
                break;

            case UIState.gameplay:
                break;

            case UIState.win:
                break;
        }
    }
}
