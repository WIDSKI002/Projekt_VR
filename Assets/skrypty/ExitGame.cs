using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Funkcja do wyjœcia z gry
    public void QuitGame()
    {
        // Zakoñczenie aplikacji
        Application.Quit();

        // W trakcie testowania w edytorze Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}