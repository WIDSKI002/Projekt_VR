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
    // Okre�l nazw� sceny, do kt�rej chcesz przenie�� gracza
    [SerializeField] private string sceneName = "MainMenu";

    // Funkcja wywo�ywana przy kolizji
    private void OnTriggerEnter(Collider other)
    {
        // Opcjonalnie: Sprawd�, czy obiekt kolizji ma okre�lon� nazw� lub tag
        if (other.CompareTag("ExitDoor"))
        {
            LoadKillCount();
            sumkills = liczbakill + spawnKaczek.kill;
            // Zapisz liczb� zab�jstw
            SaveKillCount();
            LoadHighscore();
            if (highscore < pkt.PKT)
            {
                highscore = pkt.PKT;
                SaveHighscore();
            }
            pkt.PKT = 0;
            // Przeniesienie do sceny menu g��wnego
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        // Sprawd�, czy scena istnieje i jest poprawnie skonfigurowana w Build Settings
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
        // �cie�ka do pliku z osi�gni�ciami
        string filePath = "Assets/achievements.txt";

        // Sprawd�, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Wczytaj wszystkie linie z pliku
            string[] lines = File.ReadAllLines(filePath);

            // Zaktualizuj lini� z "kills=n"
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("kills="))
                {
                    lines[i] = $"kills={sumkills}"; // Zaktualizuj warto��
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
                        liczbakill = result; // Przypisz odczytan� warto��
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
                        highscore = result; // Przypisz odczytan� warto��
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
        // �cie�ka do pliku z osi�gni�ciami
        string filePath = "Assets/achievements.txt";

        // Sprawd�, czy plik istnieje
        if (File.Exists(filePath))
        {
            // Wczytaj wszystkie linie z pliku
            string[] lines = File.ReadAllLines(filePath);

            // Zaktualizuj lini� z "kills=n"
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("highScore="))
                {
                    lines[i] = $"highScore={highscore}"; // Zaktualizuj warto��
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