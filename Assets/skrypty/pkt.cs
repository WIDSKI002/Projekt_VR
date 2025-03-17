using UnityEngine;

public class pkt : MonoBehaviour
{
    public static int PKT; // Zmienna globalna

    public TextMesh textMesh; // Referencja do komponentu TextMesh

    void Start()
    {
        UpdateTextMesh();
    }

    void Update()
    {
        // Możesz zaktualizować globalInt w dowolnym miejscu w kodzie
        // Przykład: globalInt++;
        UpdateTextMesh();
    }

    void UpdateTextMesh()
    {
        textMesh.text = "PKT:" + PKT.ToString(); // Wyświetlanie zmiennej w TextMesh
    }
}


