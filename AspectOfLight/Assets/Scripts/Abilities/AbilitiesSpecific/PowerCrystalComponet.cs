using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCrystalComponet : MonoBehaviour
{

    public GameObject crystalZone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("I'm in the zone!");
    }
     void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("I exited the zone!");
        Destroy(this.crystalZone);
    }
}
