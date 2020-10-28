using Photon.Pun; //유니티 포톤
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    private string gameVersion = "1";
    private bool ClickBtn;              // 버튼의 클릭 유무
    public Text connectionInfoText;     // 서버 상태 텍스트
    public Text connectionUser;         // 서버 연결된 유저수
    private bool CountMaster;

    WaitForSeconds Wait2Sec = new WaitForSeconds(5f);

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        connectionInfoText.text = "게임 시작 준비 중...";
    }

   
    
    // 마스터 서버에 접속
    public override void OnConnectedToMaster()
    {
        connectionInfoText.text = "마스터 서버 접속 중...";
        
        StartCoroutine(CoroutineDelay());
    }

    // 네트워크 연결이 끊겼을 때
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionInfoText.text = "오프라인 : 게임 접속 재시도 중...";
        PhotonNetwork.ConnectUsingSettings();
    }
    
    // 연결 되었을 때
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "빈 방 찾는 중...";
            // 랜덤 방에 참여한다.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 접속 재시도중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    // 빈 방이 없을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "새로운 방 생성 중...";
        // 방을 만든다.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 3 });
    }
    

    // 방에 참여 할때
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "더 많은 플레이어 찾는중...";
        connectionUser.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            connectionUser.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            connectionUser.text = PhotonNetwork.CurrentRoom.MaxPlayers + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
            PhotonNetwork.LoadLevel("Running");
        }
    }

    IEnumerator CoroutineDelay()
    {
        yield return new WaitForSeconds(3f);
        Connect();
        yield break;
    }
    
}
