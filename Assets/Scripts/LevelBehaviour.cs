using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelBehaviour : MonoBehaviour
{
    public Button homeButton; // Assign in inspector

    // Start is called before the first frame update
    void Start()
    {
        homeButton.onClick.AddListener(ChangeScene);
    }

    void ChangeScene()
    {
        // Load the scene with the same name as the selected option
        SceneManager.LoadScene("MenuScene");
    }
}