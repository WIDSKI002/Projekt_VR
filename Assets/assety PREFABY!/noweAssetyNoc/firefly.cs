using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firefly : MonoBehaviour
{
    [Header("Scaling Settings")]
    public Transform targetObject;                 // Obiekt, kt�rego rozmiar b�dzie zmieniany
    public Vector3 minScale = Vector3.one * 0.8f;  // Minimalna wielko��
    public Vector3 maxScale = Vector3.one * 1.2f;  // Maksymalna wielko��
    public float scaleInterval = 2f;               // Interwa� czasu w sekundach mi�dzy zmianami wielko�ci
    public float scaleTransitionSpeed = 1f;        // Szybko�� p�ynnej zmiany wielko�ci

    [Header("Movement Settings")]
    public float movementRadius = 0.2f;            // Promie�, w kt�rym obiekt b�dzie si� przemieszcza�
    public float movementSpeed = 0.5f;             // Szybko�� poruszania si� wok� oryginalnej pozycji

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

        // Ustawienie losowego kierunku ruchu i losowego op�nienia
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        randomStartOffset = Random.Range(0f, scaleInterval);
        InvokeRepeating("SwitchScaleTarget", randomStartOffset, scaleInterval);
    }

    void Update()
    {
        // P�ynna zmiana wielko�ci obiektu
        scaleLerpTime += Time.deltaTime * scaleTransitionSpeed;
        targetObject.localScale = Vector3.Lerp(targetObject.localScale, targetScale, scaleLerpTime);

        // P�ynne poruszanie obiektu wok� jego oryginalnej pozycji z losowym kierunkiem
        float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRadius * randomDirection.x;
        float offsetY = Mathf.Cos(Time.time * movementSpeed) * movementRadius * randomDirection.y;
        targetObject.position = originalPosition + new Vector3(offsetX, offsetY, 0f);
    }

    void SwitchScaleTarget()
    {
        // Prze��cz mi�dzy minimalnym a maksymalnym rozmiarem
        targetScale = scalingUp ? maxScale : minScale;
        scalingUp = !scalingUp;
        scaleLerpTime = 0f;  // Resetuje czas interpolacjifhfhfhf
    }
}
