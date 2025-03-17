using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class katana : MonoBehaviour
{
    private spawnKaczek spawnkaczek;

    void Start()
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
       
    }



}

