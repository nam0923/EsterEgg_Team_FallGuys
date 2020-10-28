using Photon.Pun;
using UnityEngine;

public class rotate : MonoBehaviourPun, IPunObservable
{
    private float bar_z;

    void Start()
    {
        bar_z = Random.Range(57f, 75f);
    }
   

    void Update()
    {
        transform.Rotate(0, 0, bar_z * Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(bar_z);
        }
        else
        {
            bar_z = (float)stream.ReceiveNext();
        }
    }
}
