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
        // Mo¿esz zaktualizowaæ globalInt w dowolnym miejscu w kodzie
        // Przyk³ad: globalInt++;
        UpdateTextMesh();
    }

    void UpdateTextMesh()
    {
        textMesh.text = "PKT:" + PKT.ToString(); // Wyœwietlanie zmiennej w TextMesh
    }
}


