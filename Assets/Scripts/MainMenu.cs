using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public Button playButton; // Assign in inspector
    public Dropdown levlDropdown; // Assign in inspector

    void Start()
    {
        // Add a listener to the button click event
        playButton.onClick.AddListener(ChangeScene);
    }

    void ChangeScene()
    {
        // Get the currently selected dropdown option
        string selectedOption = levlDropdown.options[levlDropdown.value].text;

        // Load the scene with the same name as the selected option
        SceneManager.LoadScene(selectedOption);
    }
}