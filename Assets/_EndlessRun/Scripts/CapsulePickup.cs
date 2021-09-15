using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulePickup : MonoBehaviour
{
    GameManager gmScript;
    // Start is called before the first frame update
    void Start()
    {
        gmScript = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
       if (col.gameObject.tag == "Player")
       {
           Destroy(this.gameObject);
           gmScript.CapsulePick();
       }
    }
}
