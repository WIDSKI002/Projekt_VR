using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class AchievementSystem : MonoBehaviour
{
    public TMP_Text firstKillText; // Odwo³anie do komponentu TextMeshPro
    public TMP_Text highscoreText;
    private int liczba = 0;       // Zmienna przechowuj¹ca liczbê zabójstw
    private int highscore = 0;

    void Start()
    {
        // Sprawdzenie, czy przypisano komponent TextMeshPro
        if (firstKillText == null)
        {
            Debug.LogError("Brak przypisanego komponentu TextMeshPro dla pierwszego zabójstwa!");
        }
        if (highscoreText == null)
        {
            Debug.LogError("Brak przypisanego komponentu TextMeshPro dla highscore!");
        }

        LoadKillCount(); // Wczytaj liczbê zabójstw
        LoadHighscore(); // Wczytaj highscore

        // Sprawdzenie i aktywacja osiagniecia 1
        if (liczba >= 1)
        {
            GameObject.Find("achiev1").SetActive(true);
        }
        else
        {
            GameObject.Find("achiev1").SetActive(false);
        }

        // Sprawdzenie i aktywacja osiagniecia 2
        if (highscore >= 100)
        {
            GameObject.Find("achiev2").SetActive(true);
        }
        else
        {
            GameObject.Find("achiev2").SetActive(false);
        }

        UpdateText(); // Aktualizacja tekstu przy starcie gry
    }

    // Funkcja do aktualizacji tekstu
    public void UpdateText()
    {
        if (firstKillText != null)
        {
            firstKillText.text = liczba + "/1";
        }
        if (highscoreText != null)
        {
            highscoreText.text = highscore + "/100";
        }
    }

    // Funkcja do wczytywania liczby z pliku tekstowego
    private void LoadKillCount()
    {
        string path = "Assets/achievements.txt";

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (line.StartsWith("kills="))
                {
                    string value = line.Substring(6); // Pobierz tekst po "kills="
                    if (int.TryParse(value, out int result))
                    {
                        liczba = result; // Przypisz odczytan¹ wartoœæ
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Plik achievements.txt nie istnieje!");
        }
    }

    private void LoadHighscore()
    {
        string path = "Assets/achievements.txt";

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (line.StartsWith("highScore="))
                {
                    string value = line.Substring(10); // Pobierz tekst po "highScore="
                    if (int.TryParse(value, out int result))
                    {
                        highscore = result; // Przypisz odczytan¹ wartoœæ
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Plik achievements.txt nie istnieje!");
        }
    }
}
