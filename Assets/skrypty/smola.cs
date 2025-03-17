using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BoxCollider))] // wymaga dodania BoxCollidera
public class SlowDownZone : MonoBehaviour
{
    private bool open = false;
    private void Start()
    {
        // Ustawienie BoxCollidera jako trigger
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

   
    private void OnTriggerStay(Collider other)
    {
        PlayerMovementSpeedController playerController = other.GetComponent<PlayerMovementSpeedController>();
        if (playerController != null && !playerController.IsSlowedDown() && gameObject.tag == "smola")
        {
            playerController.SlowDown(); // Spowalnia gracza
        }

        if (playerController != null && gameObject.tag == "speed")
        {
            XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                XRBaseInteractor interactor = grabInteractable.selectingInteractor;
                XRController controller = interactor?.GetComponent<XRController>();
       
               if (controller != null && controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed){
              // if(Input.GetButton("Fire1")){

                    if (other.CompareTag("usta"))
                    {
                        playerController.SpeedUp();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sprawdza, czy obiekt opuszczający strefę to gracz
        PlayerMovementSpeedController playerController = other.GetComponent<PlayerMovementSpeedController>();
        if (playerController != null && playerController.IsSlowedDown() && gameObject.tag == "smola")
        {
            playerController.ResetSpeed(); // Przywraca normalną prędkość gracza
        }
    }
}
