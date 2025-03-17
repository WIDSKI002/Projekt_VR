using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Reload : MonoBehaviour
{
    public GameObject ammo;
    public int ammoIlosc = 7;

    private XRGrabInteractable grabInteractable;
    private bool isAmmoHeldByController = false;

    private void Awake()
    {
        // Pobierz komponent XRGrabInteractable z obiektu `ammo`
        grabInteractable = ammo.GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // Dodaj eventy, aby �ledzi�, kiedy `ammo` jest trzymane przez kontroler
            grabInteractable.selectEntered.AddListener(OnAmmoGrabbed);
            grabInteractable.selectExited.AddListener(OnAmmoReleased);
        }
    }

    private void OnDestroy()
    {
        // Usu� eventy przy usuwaniu obiektu, aby zapobiec b��dom
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnAmmoGrabbed);
            grabInteractable.selectExited.RemoveListener(OnAmmoReleased);
        }
    }

    // Event wywo�ywany, gdy `ammo` jest chwytane przez kontroler
    private void OnAmmoGrabbed(SelectEnterEventArgs args)
    {
        isAmmoHeldByController = true;
    }

    // Event wywo�ywany, gdy `ammo` przestaje by� trzymane
    private void OnAmmoReleased(SelectExitEventArgs args)
    {
        isAmmoHeldByController = false;
    }

    void OnTriggerStay(Collider other)
    {
        // Sprawd�, czy `ammo` jest trzymane przez kontroler i czy obiekt koliduj�cy to "Gun"
        if (isAmmoHeldByController && other.CompareTag("Gun") && gameObject.tag == "ammo")
        {
          

            int dodaj = 7 - strzal.magazynek;
            if (dodaj >= ammoIlosc)
            {
                strzal.magazynek += ammoIlosc;
                ammoIlosc = 0;
                Destroy(ammo); // Zniszcz obiekt amunicji
            }
            else if (dodaj > 0)
            {
                strzal.magazynek += dodaj;
                ammoIlosc -= dodaj;
            }
        }

        if (isAmmoHeldByController && other.CompareTag("Gun") && gameObject.tag == "bigMag")
        {
            strzal.magazynek += 50;
            Destroy(ammo);
        }
    }
}
