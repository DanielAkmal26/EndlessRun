using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvirontmentTrigger : MonoBehaviour
{
    EnvirontmentSpawn terrain;
    // Start is called before the first frame update
    void Start()
    {
        terrain = GameObject.FindObjectOfType<EnvirontmentSpawn>();
        //spawnObstacle();
    }

    private void OnTriggerExit(Collider other)
    {
        terrain.Spawn();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
