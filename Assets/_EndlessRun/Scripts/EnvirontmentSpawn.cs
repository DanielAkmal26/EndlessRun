using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvirontmentSpawn : MonoBehaviour
{
    public GameObject ground;
    Vector3 nextSpawnPoint;
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        GameObject tmp = Instantiate(ground, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;
    }
}
