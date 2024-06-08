using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Title : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private SceneReference gameScene;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene(gameScene);
    }

    private void OnExitClick()
    {
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Application.Quit();
        }
    }
}
