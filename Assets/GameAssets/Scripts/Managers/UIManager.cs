using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {
    
    public UIDocument document;
    private VisualElement root,mainMenuContainer,buttonsContainer,gameplayContainer,builderTileContainer,builderTopContainer;
    private Label headerLabel;
    public static bool IsInMenu = false;

    //Editor
    [HideInInspector] public string chosenTile;

    void Start()
    {
        chosenTile = "GrassBlock";
        root = document.rootVisualElement;
        mainMenuContainer = root.Query("Container");
        buttonsContainer = root.Query("ButtonsContainer");
        gameplayContainer = root.Query("GameplayContainer");
        builderTileContainer = root.Query("BuilderTileContainer");
        builderTopContainer = root.Query("BuilderTopContainer");
        headerLabel = root.Query<Label>("HeaderLabel");
        ShowMainmenu();
    }

    public void ShowMenu(string menuTitle) {
        buttonsContainer.Clear();
        headerLabel.text = menuTitle;
        mainMenuContainer.SetDisplayBasedOnBool(true);
        IsInMenu = true;
    }

    public void ShowGameplayHud()
    {
        gameplayContainer.SetDisplayBasedOnBool(true);
    }

    public void ShowBuilderMenu(string menu)
    {
        //builderTileContainer.SetDisplayBasedOnBool(true);
        if (menu == "Top")
        {
            builderTopContainer.Clear();
            builderTopContainer.SetDisplayBasedOnBool(true);
            builderTileContainer.SetDisplayBasedOnBool(false);
        }
        else if (menu == "Tiles")
        {
            builderTileContainer.Clear();
            builderTileContainer.SetDisplayBasedOnBool(true);
        }
    }

    public void HideGameplayMenu()
    {
        gameplayContainer.SetDisplayBasedOnBool(false);
    }

    public void HideMainmenu() {
        mainMenuContainer.SetDisplayBasedOnBool(false);
        IsInMenu = false;
    }

    public void HideBuilderMenu()
    {
        builderTopContainer.SetDisplayBasedOnBool(false);
        builderTileContainer.SetDisplayBasedOnBool(false);
    }

    public void ShowMainmenu()
    {
        ShowMenu("PuzzleBlocks_Project");

        //buttonsContainer.Add(CreateButton("W", () => {
        //    MasterSingleton.Instance.Player.W = true;
        //}));

        //buttonsContainer.Add(CreateButton("S", () => {
        //    MasterSingleton.Instance.Player.S = true;
        //}));

        //buttonsContainer.Add(CreateButton("A", () => {
        //    MasterSingleton.Instance.Player.A = true;
        //}));

        //buttonsContainer.Add(CreateButton("D", () => {
        //    MasterSingleton.Instance.Player.D = true;
        //}));

        buttonsContainer.Add(CreateButton("Play game", ShowControlsMenu));
        buttonsContainer.Add(CreateButton("Build Level", SwitchToBuilder));
        buttonsContainer.Add(CreateButton("Settings", ShowSettingsMenu));
        buttonsContainer.Add(CreateButton("Quit Game", () => {
            Application.Quit();
        }));

        mainMenuContainer.style.width = new StyleLength(Length.Percent(50));
    }

    public void ShowSettingsMenu()
    {
        ShowMenu("Settings");
        buttonsContainer.Add(CreateLabel("Work In Progress"));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Back to main menu", ShowMainmenu));
    }

    public void ShowControlsMenu()
    {
        ShowMenu("Controls");
        buttonsContainer.Add(CreateLabel("WASD = Movement"));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(CreateLabel("Q/E = Camera Rotate"));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(CreateLabel("R = Reset"));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(CreateLabel("ESC = Controls"));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Next", ShowColourCodeMenu));
    }

    public void ShowColourCodeMenu()
    {
        Color brown = new Color(75, 25, 0);
        Color lightBlue = new Color(0, 171, 240);
        ShowMenu("Shadow realm Colour Code");
        buttonsContainer.Add(SquareGraphic(50, 50, Color.red, "= Buttons/Switches",50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, Color.green, "= Events", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, Color.grey, "= Pushables", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, brown , "= Springs", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, lightBlue, "= Light", 50));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Next", SwitchToGameplay));
    }

    private void ShowBuilderTileMenu()
    {
        ShowBuilderMenu("Tiles");
        builderTileContainer.Add(CreateTileButton("GrassBlock", () => chosenTile = "GrassBlock"));
        builderTileContainer.Add(CreateSpacer());
        builderTileContainer.Add(CreateTileButton("DirtBlock", () => chosenTile = "DirtBlock"));
        builderTileContainer.Add(CreateSpacer());
    }

    private void ShowBuilderTopMenu()
    {
        ShowBuilderMenu("Top");
        builderTopContainer.Add(CreateTopButton("Tiles", ShowBuilderTileMenu));
        builderTopContainer.Add(CreateTopButton("Play", ShowBuilderTileMenu));
        builderTopContainer.Add(CreateTopButton("Exit", SwitchToMainmenu));
    }

    public void ShowLevelCompleteMenu()
    {
        ShowMenu("End Of Demo");
        buttonsContainer.Add(CreateLabel("Thank you for playing!"));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Exit", () => Application.Quit()));
    }

    public void SwitchToGameplay()
    {
        HideMainmenu();
        HideBuilderMenu();
        MasterSingleton.Instance.LevelManager.SwitchScene("TestScene2");
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
        ShowGameplayHud();
    }

    public void SwitchToMainmenu()
    {
        //MasterSingleton.Instance.LevelManager.SwitchScene("Mainmenu");
        HideGameplayMenu();
        HideBuilderMenu();
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.mainmenu);
        ShowMainmenu();
    }
    public void SwitchToBuilder()
    {
        HideMainmenu();
        HideGameplayMenu();
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.edit);
        ShowBuilderTopMenu();
    }


    public const string ButtonClass = "Button", BuilderTopButtonClass = "BuilderTopButton", BuilderTileButtonClass = "BuilderTileButton", MenuLabelClass = "MenuLabel";

    public Button CreateButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(ButtonClass);
        return newbutton;
    }

    public Button CreateTopButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(BuilderTopButtonClass);
        return newbutton;
    }

    public Button CreateTileButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(BuilderTileButtonClass);
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

    public VisualElement SquareGraphic(float widthSize, float heightSize, Color color, string label, float labelLeftPosition)
    {
        VisualElement newSquareVisual = new VisualElement();
        newSquareVisual.style.minHeight = heightSize;
        newSquareVisual.style.maxWidth = widthSize;
        newSquareVisual.style.backgroundColor = color;

        Label newSquareLabel = new Label(label);
        newSquareLabel.AddToClassList(MenuLabelClass);
        newSquareLabel.style.left = new Length(labelLeftPosition, LengthUnit.Pixel);

        newSquareVisual.Add(newSquareLabel);
        return newSquareVisual;
    }
}
