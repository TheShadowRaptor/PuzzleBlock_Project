using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {
    
    public UIDocument document;
    private VisualElement root,mainMenuContainer,buttonsContainer;
    private Label headerLabel;
    public static bool IsInMenu = false;
    void Start()
    {
        root = document.rootVisualElement;
        mainMenuContainer = root.Query("Container");
        buttonsContainer = root.Query("ButtonsContainer");
        headerLabel = root.Query<Label>("HeaderLabel");
        ShowMainMenu();
    }

    public void ShowMenu(string menuTitle) {
        buttonsContainer.Clear();
        headerLabel.text = menuTitle;
        mainMenuContainer.SetDisplayBasedOnBool(true);
        IsInMenu = true;
    }

    public void HideMenu() {
        mainMenuContainer.SetDisplayBasedOnBool(false);
        IsInMenu = false;
    }

    public void ShowMainMenu()
    {
        ShowMenu("PuzzleBlocks Yay");
        buttonsContainer.Add(CreateButton("Play game", () =>
        {
            Debug.Log("Go to play game");
        }));
        buttonsContainer.Add(CreateButton("Settings", ShowSettingsMenu));
        buttonsContainer.Add(CreateButton("Quit Game", () => {
            Application.Quit();
        }));
    }

    public void ShowSettingsMenu()
    {
        ShowMenu("Settings");
        buttonsContainer.Add(CreateLabel("New label! This is a label! Isn't that cool?"));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(CreateLabel("Another label!"));
        buttonsContainer.Add(CreateButton("A button!", ShowMainMenu));
        buttonsContainer.Add(CreateButton("Another button!", ShowMainMenu));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Back to main menu", ShowMainMenu));
    }

    public const string ButtonClass = "Button", MenuLabelClass = "MenuLabel";

    public Button CreateButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(ButtonClass);
        return newbutton;
    }
    
    public Label CreateLabel(string buttonText)
    {
        Label newLabel = new Label(buttonText);
        newLabel.AddToClassList(MenuLabelClass);
        return newLabel;
    }
    
    public VisualElement CreateSpacer(float space = 10)
    {
        VisualElement spacer = new VisualElement();
        spacer.style.minHeight = space;
        return spacer;
    }
}
