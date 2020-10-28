using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviourPun , IPunObservable
{
    CharacterCamera chCamera;

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool jump { get; set; }
    public bool SuccesControll { get; set; }

  

    void Start()
    {
        chCamera = GameObject.Find("MainCamera").GetComponent<CharacterCamera>();
    }
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Running")
            {
                if (chCamera.countDown == true)
                {
                    SuccesControll = true;
                }
            }
            else
            {
                SuccesControll = true;
            }
            if (SuccesControll)
            {
                if (GameManager.instance != null && GameManager.instance.isGameover)
                {
                    SuccesControll = false;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    jump = true;
                }
                move = Input.GetAxis("Vertical");
                rotate = Input.GetAxis("Horizontal");
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(move);
            stream.SendNext(rotate);
            stream.SendNext(jump);
            stream.SendNext(SuccesControll);
        }
        else
        {
            move = (float)stream.ReceiveNext();
            rotate = (float)stream.ReceiveNext();
            jump = (bool)stream.ReceiveNext();
            SuccesControll = (bool)stream.ReceiveNext();
        }
    }
}
