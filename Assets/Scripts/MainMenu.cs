using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputField nameInput;
    public static string input;
    public Text playerName;

    public Dropdown lives;
    public static float livesTolive;
    public static int playerLives;

    public Slider timer;
    public static float timerTime = 30.0f;

    public void PlayerName()
    {
        input = nameInput.text;
        playerName.text = input;
    }

    public void DisplayName()
    {
        playerName.text = input;
    }

    public void SetLives()
    {
        switch (lives.value)
        {
            case 2:
                livesTolive = 1.0f;
                playerLives = lives.value;
                break;
            case 3:
                livesTolive = 2.0f;
                playerLives = lives.value;
                break;
            case 4:
                livesTolive = 3.0f;
                playerLives = lives.value;
                break;
            case 5:
                livesTolive = 4.0f;
                playerLives = lives.value;
                break;
            case 6:
                livesTolive = 5.0f;
                playerLives = lives.value;
                break;
            case 7:
                livesTolive = 6.0f;
                playerLives = lives.value;
                break;
            case 8:
                livesTolive = 7.0f;
                playerLives = lives.value;
                break;
            case 9:
                livesTolive = 8.0f;
                playerLives = lives.value;
                break;
            case 10:
                livesTolive = 9.0f;
                playerLives = lives.value;
                break;
        }
    }
    public void SetTimer()
    {
        timerTime = timer.value;
    }
   
    public void StartButton()
    {
        SceneManager.LoadScene(1);

    } // Start is called before the first frame update
    void Start()
    {
        timerTime = 0;
        SetTimer();
        lives.value = 0;
        SetLives();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
