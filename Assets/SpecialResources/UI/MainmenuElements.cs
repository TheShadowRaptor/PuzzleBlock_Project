using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainmenuElements : MonoBehaviour
{
    VisualElement root;

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonStart = root.Q<Button>("ButtonStart");
        Button buttonExit = root.Q<Button>("ButtonExit");

        buttonStart.clicked += () => Debug.Log("Something");
        buttonExit.clicked += () => Debug.Log("Something");
    }
}
