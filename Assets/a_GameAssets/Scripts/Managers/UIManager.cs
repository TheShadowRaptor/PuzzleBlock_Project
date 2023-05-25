using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{

    public UIDocument document;
    private VisualElement root, mainMenuContainer, buttonsContainer, gameplayContainer, builderTileContainer, builderEventContainer, builderTopContainer;
    private Label headerLabel;
    public static bool IsInMenu = false;

    public TextField levelName;
    public bool isFocused;

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
        builderEventContainer = root.Query("BuilderEventContainer");
        builderTopContainer = root.Query("BuilderTopContainer");
        headerLabel = root.Query<Label>("HeaderLabel");
        ShowMainmenu();
    }

    private void Update()
    {
        isFocused = levelName != null && levelName.panel != null && levelName.panel.focusController != null && levelName.panel.focusController.focusedElement == levelName;
    }

    public void ShowMenu(string menuTitle)
    {
        buttonsContainer.Clear();
        headerLabel.text = menuTitle;
        mainMenuContainer.SetDisplayBasedOnBool(true);
        IsInMenu = true;
    }

    public void ShowGameplayMenu()
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
            builderEventContainer.SetDisplayBasedOnBool(false);
        }
        else if (menu == "Tiles")
        {
            builderTileContainer.Clear();
            builderTileContainer.SetDisplayBasedOnBool(true);
        }

        else if (menu == "Events")
        {
            builderEventContainer.Clear();
            builderEventContainer.SetDisplayBasedOnBool(true);
        }
    }

    public void HideGameplayMenu()
    {
        gameplayContainer.SetDisplayBasedOnBool(false);
    }

    public void HideMainmenu()
    {
        mainMenuContainer.SetDisplayBasedOnBool(false);
        IsInMenu = false;
    }

    public void HideBuilderMenu()
    {
        builderTopContainer.SetDisplayBasedOnBool(false);
        builderTileContainer.SetDisplayBasedOnBool(false);
        builderEventContainer.SetDisplayBasedOnBool(false);
    }

    public void ShowMainmenu()
    {
        ShowMenu("Lumina\nRise of Darkness");

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
        buttonsContainer.Add(CreateButton("Level Select", ShowLevelsMenu));
        buttonsContainer.Add(CreateButton("Build Level", SwitchToBuilder));
        buttonsContainer.Add(CreateButton("Settings", ShowSettingsMenu));
        buttonsContainer.Add(CreateButton("Quit Game", () =>
        {
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
        buttonsContainer.Add(CreateButton("Start", SwitchToGameplay));
        buttonsContainer.Add(CreateButton("MainMenu", SwitchToMainmenu));
    }

    private void SwitchToGameplay()
    {
        SwitchToGameplay(false);
    }

    public void ShowLevelsMenu()
    {
        ShowMenu("Levels");
        string levelsPath = Application.streamingAssetsPath;
        //Find all files with extension .txt on the levelsPath folder and iterate through them on a for loop
        // Find all files with the .txt extension in the levelsPath folder
        string[] txtFiles = Directory.GetFiles(levelsPath, "*.txt");
        bool addedAnyButtons = false;
        // Iterate through the files using a foreach loop
        foreach (string txtFile in txtFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(txtFile);
            if (PlayerPrefs.GetInt(fileName, 0) == 1) {
                buttonsContainer.Add(CreateButton(fileName, () =>
                {
                    LevelSaveSystem.LoadFromPath(txtFile);
                    SwitchToGameplay(true);
                }));
                addedAnyButtons = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    var newButton = CreateButton($"{fileName} LOCKED", () => { });
                    newButton.style.backgroundColor = new Color(0.75f, 0.6f, 0.36f,0.4f);  
                    buttonsContainer.Add(newButton);
                }
            }
        }

        if (!addedAnyButtons) {
            buttonsContainer.Add(CreateLabel("You haven't beaten any levels yet"));
        }
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("MainMenu", SwitchToMainmenu));
    }

    public void ShowColourCodeMenu()
    {
        Color brown = new Color(75, 25, 0);
        Color lightBlue = new Color(0, 171, 240);
        ShowMenu("Shadow realm Colour Code");
        buttonsContainer.Add(SquareGraphic(50, 50, Color.red, "= Buttons/Switches", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, Color.green, "= Events", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, Color.grey, "= Pushables", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, brown, "= Springs", 50));
        buttonsContainer.Add(CreateSpacer());
        buttonsContainer.Add(SquareGraphic(50, 50, lightBlue, "= Light", 50));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("Next", () => {
            MasterSingleton.Instance.LevelManager.SwitchScene("Gameplay");
            SwitchToGameplay();
            }));
    }

    bool inTileMenu = false;
    private void ShowBuilderTileMenu()
    {
        ShowBuilderMenu("Tiles");
        builderTileContainer.Add(CreateTileButton("GrassBlock", () =>
        {
            chosenTile = "GrassBlock";
            builderTileContainer.SetDisplayBasedOnBool(false);
            inTileMenu = false;
        }));
        builderTileContainer.Add(CreateTileButton("DirtBlock", () =>
        {
            chosenTile = "DirtBlock";
            builderTileContainer.SetDisplayBasedOnBool(false);
            inTileMenu = false;
        }));
        builderTileContainer.Add(CreateTileButton("SandBlock", () =>
        {
            chosenTile = "SandBlock";
            builderTileContainer.SetDisplayBasedOnBool(false);
            inTileMenu = false;
        }));
        builderTileContainer.Add(CreateTileButton("LightBlock", () =>
        {
            chosenTile = "LightBlock";
            builderTileContainer.SetDisplayBasedOnBool(false);
            inTileMenu = false;
        }));
        builderTileContainer.Add(CreateTileButton("ShadowBlock", () => { chosenTile = "ShadowBlock"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("Box", () => { chosenTile = "Box"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("Gate", () => { chosenTile = "Gate"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("Exit", () => { chosenTile = "Exit"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("LightPillar", () => { chosenTile = "LightPillar"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("Button", () => { chosenTile = "Button"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("Spring", () => { chosenTile = "Spring"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
        builderTileContainer.Add(CreateTileButton("PlayerSpawner", () => { chosenTile = "PlayerSpawner"; builderTileContainer.SetDisplayBasedOnBool(false); inTileMenu = false; }));
    }

    private void ShowBuilderEventMenu()
    {
        builderEventContainer.SetDisplayBasedOnBool(true);
        ShowBuilderMenu("Events");
    }

    private void ShowBuilderTopMenu()
    {
        ShowBuilderMenu("Top");

        builderTopContainer.Add(CreateTopButton("ToggleLight", () =>
        {
            LevelArea.Instance.ToggleEditorLight();

        }));
        builderTopContainer.Add(CreateTopButton("Tiles", () =>
        {
            if (inTileMenu == true) 
            {
                inTileMenu = false;
                builderTileContainer.SetDisplayBasedOnBool(false);
            }

            else if (inTileMenu == false)
            {
                inTileMenu = true;
                builderTileContainer.SetDisplayBasedOnBool(true);
                ShowBuilderTileMenu();
            }
        }));
        builderTopContainer.Add(levelName = CreateTextField("SaveLevel"));
        builderTopContainer.Add(CreateTopButton("Save", () =>
        {
            var saveText = LevelSaveSystem.Serialize();
            string filePath = $"{Application.streamingAssetsPath}/{levelName.text}.txt";
            File.WriteAllText(filePath, saveText);
        }
        ));
        builderTopContainer.Add(CreateTopButton("Load", () =>
        {
            string filePath = $"{Application.streamingAssetsPath}/{levelName.text}.txt";
            if (File.Exists(filePath))
            {
                var readText = File.ReadAllText(filePath);
                LevelSaveSystem.DeSerialize(readText);
            }
        }
        ));
        builderTopContainer.Add(CreateTopButton("Play", () =>
        {
            SwitchToPlaytest();
            MasterSingleton.Instance.Player.ResetStats();

        }));
        builderTopContainer.Add(CreateTopButton("Exit", SwitchToMainmenu));
    }

    public void ShowLevelCompleteMenu()
    {
        HideGameplayMenu();
        ShowMenu("LevelComplete");
        //buttonsContainer.Add(CreateLabel("Thank you for playing!"));
        buttonsContainer.Add(CreateSpacer(50));
        buttonsContainer.Add(CreateButton("NextLevel", () =>
        {
            ShowGameplayMenu();
            SwitchToGameplay();       
        }));
    }

    public void SwitchToGameplay( bool skipFindNextLevel)
    {
        if(!skipFindNextLevel)
            LevelSaveSystem.FindNextLevel();
        
        CameraController.cameraController.ReAlignGameCamera();
        HideMainmenu();
        HideBuilderMenu();
        MasterSingleton.Instance.Player.ResetStats();
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
        LevelArea.Instance.levelEditorLight.enabled = false;
        ShowGameplayMenu();
    }
    public void SwitchToPlaytest()
    {
        HideMainmenu();
        HideBuilderMenu();
        LevelArea.Instance.ReloadTiles();
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
        MasterSingleton.Instance.LevelManager.SpawnPlayer();
        CameraController.cameraController.ReAlignGameCamera();
        ShowGameplayMenu();
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
        MasterSingleton.Instance.LevelManager.SwitchScene("LevelBuilder");
        ShowBuilderTopMenu();
    }

    bool inEventMenu = false;

    public void ShowLevelEditorConnectButtonMenu(EventObjButton lbb) {
        if (inEventMenu == false) 
        {
            inEventMenu = true;
            builderEventContainer.Clear();
            ShowBuilderEventMenu();
            for (int i = 0; i < EventObject.eventObjects.Count; i++) {
                EventObject objReference = EventObject.eventObjects[i];
                builderEventContainer.Add(CreateEventButton($"{objReference.gameObject.name} {Vector3Int.RoundToInt(objReference.transform.position)}",
                    () =>
                    {
                        lbb.eventInformation = Vector3Int.RoundToInt(objReference.transform.position);
                        builderEventContainer.Clear();
                        ShowBuilderEventMenu();
                    }));
            }        
        }
        else
        {
            inEventMenu = false;
            builderEventContainer.Clear();
            builderEventContainer.SetDisplayBasedOnBool(false);
        }
    }

    public const string ButtonClass = "Button", BuilderTopButtonClass = "BuilderTopButton", BuilderTileButtonClass = "BuilderTileButton", BuilderEventButtonClass = "BuilderEventButton", MenuLabelClass = "MenuLabel", TextFieldClass = "SaveTextField";

    public Button CreateButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(ButtonClass);
        return newbutton;
    }

    public TextField CreateTextField(string labelText)
    {
        TextField textField = new TextField();
        textField.label = labelText;
        textField.AddToClassList(TextFieldClass);
        return textField;
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

    public Button CreateEventButton(string buttonText, Action buttonAction)
    {
        Button newbutton = new Button(buttonAction);
        newbutton.text = buttonText;
        newbutton.AddToClassList(BuilderEventButtonClass);
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
