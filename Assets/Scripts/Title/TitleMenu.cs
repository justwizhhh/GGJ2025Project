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

    public GameObject MainMenuUI;
    public GameObject OptionsUI;
    public GameObject CreditsUI;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        switch (currentTitleSection)
        {
            case TitleSection.Null: // If nothing has been selected on the titlescreen yet
                menuOption += Input.GetKeyDown(KeyCode.S) ? 1 : Input.GetKeyDown(KeyCode.W) ? -1 : 0;
                menuOption = Mathf.Clamp(menuOption, 0, 2);

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
                            currentTitleSection = TitleSection.Credits;
                            MainMenuUI.SetActive(false);
                            CreditsUI.SetActive(true);
                            break;
                        case 2:
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

            case TitleSection.Credits:

                // Going back to the main menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    currentTitleSection = TitleSection.Null;
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
