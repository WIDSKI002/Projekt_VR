using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firefly : MonoBehaviour
{
    [Header("Scaling Settings")]
    public Transform targetObject;                 // Obiekt, którego rozmiar bêdzie zmieniany
    public Vector3 minScale = Vector3.one * 0.8f;  // Minimalna wielkoœæ
    public Vector3 maxScale = Vector3.one * 1.2f;  // Maksymalna wielkoœæ
    public float scaleInterval = 2f;               // Interwa³ czasu w sekundach miêdzy zmianami wielkoœci
    public float scaleTransitionSpeed = 1f;        // Szybkoœæ p³ynnej zmiany wielkoœci

    [Header("Movement Settings")]
    public float movementRadius = 0.2f;            // Promieñ, w którym obiekt bêdzie siê przemieszcza³
    public float movementSpeed = 0.5f;             // Szybkoœæ poruszania siê wokó³ oryginalnej pozycji

    private Vector3 originalPosition;
    private Vector3 targetScale;
    private Vector3 randomDirection;
    private float scaleLerpTime = 0f;
    private float randomStartOffset;
    private bool scalingUp = true;

    void Start()
    {
        if (targetObject == null)
        {
            targetObject = transform;
        }

        originalPosition = targetObject.position;
        targetScale = minScale;

        // Ustawienie losowego kierunku ruchu i losowego opóŸnienia
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        randomStartOffset = Random.Range(0f, scaleInterval);
        InvokeRepeating("SwitchScaleTarget", randomStartOffset, scaleInterval);
    }

    void Update()
    {
        // P³ynna zmiana wielkoœci obiektu
        scaleLerpTime += Time.deltaTime * scaleTransitionSpeed;
        targetObject.localScale = Vector3.Lerp(targetObject.localScale, targetScale, scaleLerpTime);

        // P³ynne poruszanie obiektu wokó³ jego oryginalnej pozycji z losowym kierunkiem
        float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRadius * randomDirection.x;
        float offsetY = Mathf.Cos(Time.time * movementSpeed) * movementRadius * randomDirection.y;
        targetObject.position = originalPosition + new Vector3(offsetX, offsetY, 0f);
    }

    void SwitchScaleTarget()
    {
        // Prze³¹cz miêdzy minimalnym a maksymalnym rozmiarem
        targetScale = scalingUp ? maxScale : minScale;
        scalingUp = !scalingUp;
        scaleLerpTime = 0f;  // Resetuje czas interpolacjifhfhfhf
    }
}
