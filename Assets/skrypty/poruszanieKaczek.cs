using System.Collections;
using UnityEngine;

public class SmoothAutoMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxRuch = 10f;
    public float checkDistance = 0.5f;
    public float rotationSpeed = 5f;
    public LayerMask obstacleLayer;

    private Vector3 moveDirection;
    private bool isMoving = false;
    private Coroutine currentRotationCoroutine; // Zmienna do przechowywania korutyny rotacji

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            // Wybierz losowy kierunek i obróæ obiekt
            ChooseRandomDirection();
            RotateToDirection();

            float elapsedTime = 0f;
            isMoving = true;
            float moveTime = Random.Range(0.1f, maxRuch);
            while (elapsedTime < moveTime && CanMove(moveDirection))
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            isMoving = false;
            yield return new WaitForSeconds(1f);
        }
    }

    private void ChooseRandomDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
        moveDirection = directions[Random.Range(0, directions.Length)];
    }

    private void RotateToDirection()
    {
        // Przerwij poprzedni¹ korutynê rotacji, jeœli istnieje
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        // Oblicz docelow¹ rotacjê na podstawie kierunku ruchu i rozpocznij now¹ korutynê
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        currentRotationCoroutine = StartCoroutine(SmoothRotate(targetRotation));
    }

    private IEnumerator SmoothRotate(Quaternion targetRotation)
    {
        // Wykonuj p³ynn¹ rotacjê do docelowego kierunku
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        RaycastHit hit;
        bool isHit = Physics.BoxCast(
            transform.position,
            GetComponent<BoxCollider>().bounds.extents,
            direction,
            out hit,
            Quaternion.identity,
            checkDistance,
            obstacleLayer
        );

        return !isHit;
    }
}
