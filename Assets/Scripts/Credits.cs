using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

public class Credits : MonoBehaviour
{
    public Text HighScores;
    public int num_scores = 10;

    public void ShowTopScores()
    {
        string path = "Assets/Resources/HighScores.txt";
        string line;
        string[] fields;
        string[] playerNames = new string[num_scores];
        int[] playerScores = new int[num_scores];
        int scores_read = 0;

        HighScores.text = " "; // clear the scores box

        StreamReader reader = new StreamReader(path);
        while (!reader.EndOfStream && scores_read < num_scores)
        {
            line = reader.ReadLine();
            fields = line.Split(',');
            HighScores.text += fields[0] + " : " + fields[1] + "\n";
            scores_read += 1;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        ShowTopScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
            Debug.Log("You have quit the game.");
            Application.Quit();
    }

    public void Clear()
    {
        HighScores.text = " ";
        Debug.Log("output cleared");
    }
}
