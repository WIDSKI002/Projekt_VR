using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovementSpeedController : MonoBehaviour
{
    public ContinuousMoveProviderBase moveProvider; // referencja do ContinuousMoveProvider
    public float StartSpeed = 1.0f; // normalna prêdkoœæ
    private float normalSpeed = 1.0f;
    public float slowSpeed = 0.2f; // spowolniona prêdkoœæ
    private bool isSlowedDown = false;
    private bool isSpeedUp = false;
    public float countdownTime = 5f; // Ustalony czas odliczania w sekundach
    void Start()
    {
        normalSpeed = StartSpeed;
        if (moveProvider == null)
        {
            moveProvider = GetComponent<ContinuousMoveProviderBase>();
        }
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed;
        }
    }
  

    void Update()
    {
        if (isSpeedUp && countdownTime > 0)
        {
            countdownTime -= Time.deltaTime; // Odliczanie czasu

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                isSpeedUp = false; // Zatrzymaj odliczanie
                normalSpeed = StartSpeed;
                moveProvider.moveSpeed = normalSpeed;
            }
        }
    }
    // Funkcja do spowalniania gracza
    public void SlowDown()
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed - slowSpeed;
            isSlowedDown = true;
        }
    }
    public void SpeedUp()
    {
        if (moveProvider != null && !isSpeedUp)
        {
            normalSpeed *= 3;
            moveProvider.moveSpeed = normalSpeed ;
            isSpeedUp = true;
        }
    }
   
    // Funkcja do przywrócenia normalnej prêdkoœci
    public void ResetSpeed()
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = normalSpeed;
            isSlowedDown = false;
        }
    }

    // Sprawdza, czy gracz jest spowolniony
    public bool IsSlowedDown()
    {
        return isSlowedDown;
    }
}
