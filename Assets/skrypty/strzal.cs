using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Collections;

public class strzal : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawn; // Miejsce, z kt�rego pocisk b�dzie wystrzeliwany
    public float bulletSpeed = 1000f; // Pr�dko�� pocisku
    public float bulletLifetime = 2f; // Czas �ycia pocisku
    public static int magazynek = 7;
    private bool trigger = false;
    private bool isHolding = false; // Czy obiekt jest trzymany
    private int holdingCount = 0; // Liczba interaktor�w trzymaj�cych obiekt
    private string firstControllerTag; // Tag kontrolera, kt�ry pierwszy chwyci� obiekt
    private XRGrabInteractable grabInteractable;
    private InputDevice firstControllerDevice;

    private int firstInteractorID = -1;
    private XRBaseInteractor currentInteractor;
    public float recoilReturnSpeed = 2f;
    private Quaternion originalRotation;
    private Quaternion originalGunRotation;


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.onSelectEntered.AddListener(OnGrab);
            grabInteractable.onSelectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogError("XRGrabInteractable nie zosta� znaleziony na obiekcie " + gameObject.name);
        }
        
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Sprawdzanie przycisku strza�u tylko, gdy obiekt jest trzymany przez oba kontrolery
        if (isHolding && holdingCount == 2 || holdingCount == 1 && firstControllerDevice.isValid)
        {
          
            if (firstControllerDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerPressed) && isTriggerPressed)
            {
                if(!trigger) { 
                    Shoot(); }
                
            }
            trigger = isTriggerPressed;
        }
        
    }

    private void OnGrab(XRBaseInteractor interactor)
    {
        holdingCount++;
        if (holdingCount == 1)
        {
           
            firstInteractorID = interactor.transform.GetInstanceID();//czy to jest potrzebne????
            firstControllerTag = interactor.gameObject.tag;//czy to jest potrzebne????
            currentInteractor = interactor;
            originalGunRotation = transform.rotation;
            if (firstControllerTag != "lewy" && firstControllerTag != "prawy")
            {
                Debug.LogError("Nie znaleziono poprawnego tagu dla kontrolera! Ustaw tagi na lewy lub prawy.");
                return;
            }

            // Ustawienie urz�dzenia dla kontrolera
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(firstControllerTag == "lewy" ? XRNode.LeftHand : XRNode.RightHand, devices);
            if (devices.Count > 0)
            {
                firstControllerDevice = devices[0];
            }
            else
            {
                Debug.LogError("Nie znaleziono kontrolera " + firstControllerTag);
            }
        }
        isHolding = true;
    }

    


    private void OnRelease(XRBaseInteractor interactor)
    {
        holdingCount--;
        if (holdingCount <= 0)
        {
            isHolding = false;
            firstControllerTag = null;
            firstInteractorID = -1;
            firstControllerDevice = default;
               currentInteractor = null;
        }
    }

  

    void Shoot()
    {
        if (magazynek > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bulletSpawn.forward * bulletSpeed); // Dodaje si�� do pocisku w kierunku, w kt�rym jest skierowany

            Destroy(bullet, bulletLifetime);

            bullet.AddComponent<CollisionHandler>().SetGun(this);
            magazynek--;
             if (currentInteractor != null && holdingCount == 1)
            {
                StartCoroutine(SmoothRecoil());
            }
        }
    }

    IEnumerator SmoothRecoil()
    {
        // Zapamiętaj aktualną rotację broni i kontrolera
        Quaternion currentGunRotation = transform.rotation;
        Quaternion controllerOriginalRotation = currentInteractor.transform.rotation;
        
        // Oblicz różnicę między rotacją broni a kontrolera
        Quaternion rotationDifference = Quaternion.Inverse(controllerOriginalRotation) * currentGunRotation;
        
        // Oblicz docelową rotację (90 stopni w górę względem aktualnej rotacji broni)
        Quaternion targetGunRotation = currentGunRotation * Quaternion.Euler(90f, 0f, 0f);
        // Przelicz docelową rotację kontrolera
        Quaternion targetControllerRotation = targetGunRotation * Quaternion.Inverse(rotationDifference);
        
        // Płynne przejście do rotacji odrzutu
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * recoilReturnSpeed;
            currentInteractor.transform.rotation = Quaternion.Lerp(controllerOriginalRotation, targetControllerRotation, elapsedTime);
            yield return null;
        }

        // Płynny powrót do pierwotnej rotacji
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * recoilReturnSpeed;
            currentInteractor.transform.rotation = Quaternion.Lerp(targetControllerRotation, controllerOriginalRotation, elapsedTime);
            yield return null;
        }

        // Upewnij się, że kontroler wrócił do pierwotnej rotacji
        currentInteractor.transform.rotation = controllerOriginalRotation;
    }

}


public class CollisionHandler : MonoBehaviour
{
    private strzal gun;
    public spawnKaczek spawnkaczek;

    public void SetGun(strzal gun)
    {
        this.gun = gun;
    }

    private void Start()
    {
        spawnkaczek = FindObjectOfType<spawnKaczek>();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible") && spawnkaczek != null)
        {
            spawnkaczek.DestroyModel(collision.gameObject);
            pkt.PKT += spawnkaczek.GetPointsForObject(gameObject);
        }
        Destroy(gameObject);
    }
}
