using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    [SerializeField] Text bestScoreText;
    [SerializeField] InputField nameField;

    public string name = "";

    public string bestName;
    public int bestScore;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
    }

    void Start()
    {
        if (bestName != "")
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = name;
            SaveGame();
            MainManager.Instance.BestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
        Debug.Log("Score: " + score + "  Player: " + name);
    }

    public void StartGame()
    {
        if (nameField.text != "")
        {
            name = nameField.text;
            SceneManager.LoadScene("MainLevel");
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
        }
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    [System.Serializable] class SaveData
    {
        public string name;
        public int bestScore;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.name = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestName = data.name;
            bestScore = data.bestScore;
        }
    }
}