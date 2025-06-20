using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleMenu : MonoBehaviour
{
    // A basic title-screen menu, where you can access the options
    enum TitleSection
    {
        Null,
        StartGame,
        Options,
        Credits,
        Exit
    }
    TitleSection currentTitleSection;

    int menuOption;

    // Positions for the camera to move towards when selecting different menu options
    public Vector2 MainCamPosition;
    public Vector2 OptionsCamPosition;
    public Vector2 CreditsCamPosition;

    [Space(10)]

    public GameObject MenuCursor;
    TextMeshProUGUI currentCursorPos;

    [Space(10)]

    public GameObject LogoUI;
    public GameObject MainMenuUI;
    public GameObject OptionsUI;
    public GameObject CreditsUI;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        AudioManager.instance.PlayMusic("GGJ2025ProjectMusic OLD", 1);
    }

    private void UpdateMenuCursor(bool isVisible)
    {
        MenuCursor.transform.GetChild(0).GetComponent<RawImage>().enabled = isVisible;
        MenuCursor.transform.GetChild(1).GetComponent<RawImage>().enabled = isVisible;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(AudioManager.instance.PlaySound("game_start", 1)); 
        }
            
        switch (currentTitleSection)
        {
            case TitleSection.Null: // If nothing has been selected on the titlescreen yet
                menuOption += Input.GetKeyDown(KeyCode.S) ? 1 : Input.GetKeyDown(KeyCode.W) ? -1 : 0;
                menuOption = Mathf.Clamp(menuOption, 0, 3);

                // Selecting each menu options one-by-one
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switch (menuOption)
                    {
                        case 0:
                            currentTitleSection = TitleSection.StartGame;
                            SceneManager.LoadScene(2);
                            break;
                        case 1:
                            currentTitleSection = TitleSection.Options;
                            MainMenuUI.SetActive(false);
                            OptionsUI.SetActive(true);
                            break;
                        case 2:
                            currentTitleSection = TitleSection.Credits;
                            UpdateMenuCursor(false);
                            LogoUI.SetActive(false);
                            MainMenuUI.SetActive(false);
                            CreditsUI.SetActive(true);
                            break;
                        case 3:
                            Application.Quit();
                            break;
                    }
                }

                cam.transform.position = MainCamPosition;
                currentCursorPos = MainMenuUI.transform.GetChild(menuOption).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.StartGame:
                // Starts the game

                break;

            case TitleSection.Options: // Changing music and sound volumes
                menuOption += Input.GetKeyDown(KeyCode.S) ? 1 : Input.GetKeyDown(KeyCode.W) ? -1 : 0;
                menuOption = Mathf.Clamp(menuOption, 0, 1);

                // Choosing between music/sound volume toggles
                if (menuOption == 0)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        AudioManager.instance.UpdateMusicVolume(0.1f);
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        AudioManager.instance.UpdateMusicVolume(-0.1f);
                    }

                    OptionsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                        "Music Volume - " + (Mathf.RoundToInt(AudioManager.instance.MusicVolume * 100)).ToString() + "%";
                }
                else
                {
                    AudioManager.instance.SoundVolume = Mathf.Clamp(AudioManager.instance.SoundVolume, 0, 1);
                    AudioManager.instance.SoundVolume +=
                        Input.GetKeyDown(KeyCode.D) ? 0.1f :
                        Input.GetKeyDown(KeyCode.A) ? -0.1f : 0;

                    OptionsUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                        "Sound Volume - " + (Mathf.RoundToInt(AudioManager.instance.SoundVolume * 100)).ToString() + "%";
                }

                // Going back to the main menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentTitleSection = TitleSection.Null;
                    OptionsUI.SetActive(false);
                    MainMenuUI.SetActive(true);
                }

                cam.transform.position = OptionsCamPosition;
                currentCursorPos = OptionsUI.transform.GetChild(menuOption).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.Credits:

                // Going back to the main menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentTitleSection = TitleSection.Null;
                    UpdateMenuCursor(true);
                    LogoUI.SetActive(true);
                    CreditsUI.SetActive(false);
                    MainMenuUI.SetActive(true);
                }

                cam.transform.position = CreditsCamPosition;
                currentCursorPos = CreditsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.Exit:
                Application.Quit();
                break;
        }

        // Moving the cursor object around, depending on what part of the menu is being selected currently
        if (currentCursorPos != null) 
        {
            MenuCursor.transform.position = currentCursorPos.transform.position;
            MenuCursor.GetComponent<RectTransform>().sizeDelta = new Vector2(currentCursorPos.rectTransform.rect.width + 40, 0);
        }
    }
}
