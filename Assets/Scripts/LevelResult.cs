using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelResult : MonoBehaviour
{
    public Button menuButton; // Assign in inspector

    // Start is called before the first frame update
    void Start()
    {
        // Add a listener to the button click event
        menuButton.onClick.AddListener(ChangeScene);
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MenuScene");

    }
}