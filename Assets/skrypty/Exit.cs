using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Exit : MonoBehaviour
{
    private int liczbakill = 0;
    private int sumkills = 0;
    private int highscore = 0;
    // Okreœl nazwê sceny, do której chcesz przenieœæ gracza
    [SerializeField] private string sceneName = "MainMenu";

    // Funkcja wywo³ywana przy kolizji
    private void OnTriggerEnter(Collider other)
    {
        // Opcjonalnie: SprawdŸ, czy obiekt kolizji ma okreœlon¹ nazwê lub tag
        if (other.CompareTag("ExitDoor"))
        {
            LoadKillCount();
            sumkills = liczbakill + spawnKaczek.kill;
            // Zapisz liczbê zabójstw
            SaveKillCount();
            LoadHighscore();
            if (highscore < pkt.PKT)
            {
                highscore = pkt.PKT;
                SaveHighscore();
            }
            pkt.PKT = 0;
            // Przeniesienie do sceny menu g³ównego
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        // SprawdŸ, czy scena istnieje i jest poprawnie skonfigurowana w Build Settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scena '{sceneName}' nie istnieje w ustawieniach Build Settings!");
        }
    }

    private void SaveKillCount()
    {
        // Œcie¿ka do pliku z osi¹gniêciami
        string filePath = "Assets/achievements.txt";

        // SprawdŸ, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Wczytaj wszystkie linie z pliku
            string[] lines = File.ReadAllLines(filePath);

            // Zaktualizuj liniê z "kills=n"
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("kills="))
                {
                    lines[i] = $"kills={sumkills}"; // Zaktualizuj wartoœæ
                    break;
                }
            }

            // Zapisz zmienione linie do pliku
            File.WriteAllLines(filePath, lines);
        }
        else
        {
            Debug.LogError("Plik achievements.txt nie istnieje!");
        }
    }

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
                        liczbakill = result; // Przypisz odczytan¹ wartoœæ
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

    private void SaveHighscore()
    {
        // Œcie¿ka do pliku z osi¹gniêciami
        string filePath = "Assets/achievements.txt";

        // SprawdŸ, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Wczytaj wszystkie linie z pliku
            string[] lines = File.ReadAllLines(filePath);

            // Zaktualizuj liniê z "kills=n"
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("highScore="))
                {
                    lines[i] = $"highScore={highscore}"; // Zaktualizuj wartoœæ
                    break;
                }
            }

            // Zapisz zmienione linie do pliku
            File.WriteAllLines(filePath, lines);
        }
        else
        {
            Debug.LogError("Plik achievements.txt nie istnieje!");
        }
    }
}