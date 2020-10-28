using Photon.Pun;
using System.Collections;
using UnityEngine;

public class RunningNetwork : MonoBehaviourPun
{
    public GameObject floor;
    private GameObject Char;
    private int num;

    public Transform[] spawnPoints = new Transform[3];
        
    void Awake()
    {
        Vector3 spawn;
        if (PhotonNetwork.IsMasterClient)
        {
            spawn = spawnPoints[0].position;
        }
        else
        {
            spawn = spawnPoints[Random.Range(1, 3)].position;
        }

       Char = PhotonNetwork.Instantiate("Player", spawn, Quaternion.identity);
       Camera.main.GetComponent<CharacterCamera>().SetTarget(Char);
    }

        
}
