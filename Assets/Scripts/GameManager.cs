using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public Text playerName;
    [SerializeField]
    public Text livesDisplay;   
    [SerializeField]
    public Text pointsText;
    [SerializeField]
    public GameObject pauseMenu;

    public static int playerpoints = 0;
    public int playerlives;

    public Camera cam;
    public AudioSource AudioSource;
    public Toggle soundToggle;
    public static bool sound;

    private bool isPaused;

    public Text playerTime;
    public float timelimit = MainMenu.timerTime;
    public float timeRemaining;

    public int num_scores = 10;

    public void AddPoints()
    {
        playerpoints += 100;
        pointsText.text =  playerpoints.ToString();
    }

    public void SubtractPoints()
    {
        playerpoints -= 100;
        pointsText.text = playerpoints.ToString();
    }

    public void AddLives()
    {
        playerlives += 1;
        livesDisplay.text = playerlives.ToString();
    }

    public void SubtractLives()
    {
        playerlives -= 1;
        livesDisplay.text = playerlives.ToString();
    }

    public void SoundToggle()
    {
        if (soundToggle.isOn == false)
        {
            cam.GetComponent<AudioSource>().Pause();
        }
        else
        {
            cam.GetComponent<AudioSource>().Play();
        }
        sound = soundToggle.isOn;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
    public void ResumeGame() // called from ESC; also attached to the resume button
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void NewGame()
    {
        playerpoints = 0;
        pointsText.text = playerpoints.ToString();
        playerlives = 0;
        livesDisplay.text = playerlives.ToString();

        ResumeGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        playerName.text = "Currently playing: " +MainMenu.input;
        playerlives = MainMenu.playerLives;
        livesDisplay.text = playerlives.ToString();
        timeRemaining = timelimit;
        playerTime.text = timeRemaining.ToString() + " seconds left";
        playerpoints = 0;
        pointsText.text = playerpoints.ToString();
        soundToggle.isOn = true;
        SoundToggle();
    }

    // Update is called once per frame
    void Update()
    {

        timeRemaining -= Time.deltaTime;
        if (timeRemaining >= 0)
        {
            playerTime.text = timeRemaining.ToString() + " (seconds) left";
        }
        else
        {
            playerTime.text = "GAME OVER";
            Time.timeScale = 0;
        }


        if (Input.GetKeyDown(KeyCode.Escape)) // ESC key pauses AND resumes
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ExitButton()
    {
        EndGame();
        SceneManager.LoadScene(2);
    }

    public void EndGame()
    { 
            string path = "Assets/Resources/HighScores.txt";
            string line;
            string[] fields;
            int scores_written = 0;
            string newName = playerName.text;
            string newScore = playerpoints.ToString();
            bool newScoreWritten = false;
            string[] writeNames = new string[10];
            string[] writeScores = new string[10];

            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                fields = line.Split(',');
                if (!newScoreWritten && scores_written < num_scores) // if new score has not been written yet
                {
                //check if we need to write new higher score first
                if (Convert.ToInt32(newScore) > Convert.ToInt32(fields[1]))
                    {
                        writeNames[scores_written] = newName;
                        writeScores[scores_written] = newScore;
                        newScoreWritten = true;
                        scores_written += 1;
                    }
                }
                if (scores_written < num_scores) // we have not written enough lines yet
                {
                    writeNames[scores_written] = fields[0];
                    writeScores[scores_written] = fields[1];
                    scores_written += 1;
                }
            }
            reader.Close();

            // now we have parallel arrays with names and scores to write
            StreamWriter writer = new StreamWriter(path);

            for (int x = 0; x < scores_written; x++)
            {
                writer.WriteLine(writeNames[x] + ',' + writeScores[x]);
            }
            writer.Close();

            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("HighScores");
    }

    private SaveData CreateSaveData()
    {
        SaveData save = new SaveData();
        save.name = MainMenu.input;
        save.lives = playerlives;
        save.points = playerpoints;
        save.seconds = timeRemaining;

        return save;
    }

    public void SaveButton()
    {
        SaveData save = CreateSaveData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        playerpoints = 0;
        pointsText.text = playerpoints.ToString();
        playerlives = 0;
        livesDisplay.text =playerlives.ToString();
        timeRemaining = 0;
        playerTime.text = timeRemaining + " (seconds) left";

        Debug.Log("Game saved");
    }

    public void LoadButton()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();

            playerName.text = save.name;
            playerlives = save.lives;
            livesDisplay.text = save.lives.ToString();
            playerpoints = save.points;
            pointsText.text = save.points.ToString();
            timeRemaining = save.seconds;
            playerTime.text = save.seconds.ToString()+ " (seconds) left";
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void SaveAsJSON()
    {
        SaveData save = CreateSaveData();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);
    }
}


