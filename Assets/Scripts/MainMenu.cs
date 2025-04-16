using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //Main Menu Buttons
    public GameObject mmbttns;
    public GameObject newGameButton;
    public GameObject settingsButton;
    public GameObject exitButton;

    //Settings Buttons
    public GameObject sttngsbttns;
    public GameObject returnButton;
    void Start()
    {
        //Main Menu Buttons
        newGameButton.GetComponent<Button>().onClick.AddListener(NewGame);
        settingsButton.GetComponent<Button>().onClick.AddListener(Settings);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitTheGame);

        //Settings Buttons
        returnButton.GetComponent<Button>().onClick.AddListener(ReturnToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void ExitTheGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    void Settings()
    {
        mmbttns.SetActive(false);
        sttngsbttns.SetActive(true);
    }

    void ReturnToMainMenu()
    {
        mmbttns.SetActive(true);
        sttngsbttns.SetActive(false);
    }
}
