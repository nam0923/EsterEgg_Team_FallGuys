using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;

public enum PLAYER_STATE
{
    //STRETCHING,
    MOVE,
    JUMP,
    FALL,
    VICTORY,
    END
}

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    WaitForSeconds Wait2Sec = new WaitForSeconds(2f);
    WaitForSeconds Wait8Sec = new WaitForSeconds(8f);
    
    public float moveSpeed = 10f;
    public float rotateSpeed = 180f;
    public float jumpPower = 10f;

    public PlayerControll playerControll;
    public Animator playerAnimator;

    private Rigidbody playerRigidbody;

    private int RevState;
    private float BMove;
    private bool BJump;

    private bool IsCollBar;
    private bool Airstate;
    private bool isGround;

    private Vector3 otherPos;
    private Quaternion otherRot;

    [SerializeField]
    PLAYER_STATE state = PLAYER_STATE.MOVE;

    void Start()
    {
        playerControll = GetComponent<PlayerControll>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        
    }
    void FixedUpdate()
    {
        if (state != PLAYER_STATE.JUMP)
        {
            moveSpeed = 5f;
        }
        else
        {
            moveSpeed = 10f;
        }
     
        Gravity();
        Move();
        JumpUp();
        Rotate();

        playerAnimator.SetFloat("move", playerControll.move);

    }
    
    void Gravity()
    {
        Physics.gravity = new Vector3(0, -10.0f, 0);
    }
    void Move()
    {
        Vector3 moveDistance = playerControll.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }
    void Rotate()
    {
        float turn = playerControll.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
    void JumpUp()
    {
        if (!playerControll.jump)
            return;
        
        if (!Airstate)
        {
            moveSpeed = 5f;
            playerRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            playerAnimator.SetBool("isJump", true);
            Airstate = true;
        }
    }

    #region 패킷전송
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(state);
            
            ChangeState(state);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            ChangeState((PLAYER_STATE)stream.ReceiveNext());
        }
    }
    #endregion

    #region 충돌처리
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bar")
        {
            IsCollBar = true;
        }
        if (collision.transform.tag == "Ground")
        {
            playerAnimator.SetBool("isJump", false);
            playerControll.jump = false;
            isGround = true;
            Airstate = false;
        }
        if (collision.transform.tag == "Wheel")
        {
            playerAnimator.SetBool("isJump", false);
            playerControll.jump = false;
            Airstate = false;

        }
        if (collision.transform.tag == "Win")
        {
            ChangeState(PLAYER_STATE.VICTORY);
            StartCoroutine(CoroutineChangeVictory());
            playerControll.SuccesControll = false;
        }
       
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fall")
        {
            ChangeState(PLAYER_STATE.FALL);
        }
        if (other.tag == "StartPos")
        {
            ChangeState(PLAYER_STATE.MOVE);
        }
        if (other.tag == "SavePos")
        {
            ChangeState(PLAYER_STATE.MOVE);
        }
    }
    #endregion

    void ChangeState(PLAYER_STATE nextState)
    {
       if (state == nextState)
           return;

        state = nextState;
        BMove = 0f;
        BJump = false;

        playerAnimator.SetBool("isJump", BJump);
        //playerAnimator.SetBool("isStretch", false);
        playerAnimator.SetBool("isFall", false);
        playerAnimator.SetBool("isVictory", false);
        playerAnimator.SetFloat("move", BMove);

        StopAllCoroutines();

        switch (state)
        {
            //case PLAYER_STATE.STRETCHING:
            //    StartCoroutine(CoroutineStretch());
            //    break;
            case PLAYER_STATE.MOVE:
                BMove = 1f;
                break;
            case PLAYER_STATE.JUMP:
                BJump = true;
                break;
            case PLAYER_STATE.FALL:
                StartCoroutine(CoroutineFall());
                break;
            case PLAYER_STATE.VICTORY:
                StartCoroutine(CoroutineVictory());
                break;
        }
    }

    IEnumerator CoroutineChangeVictory()
    {
        while (true)
        {
            yield return Wait8Sec;
            SceneManager.LoadScene("Victory");
            yield break;
        }
    }

    IEnumerator CoroutineFall()
    {
        playerAnimator.SetBool("isFall", true);
        yield break;
    }
    IEnumerator CoroutineVictory()
    {
        playerAnimator.SetBool("isVictory", true);
        yield break;
    }
}
