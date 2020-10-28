using UnityEngine;
using System.Collections;

public class DropFloorMgr : MonoBehaviour
{
    public float FloorColorRand { get; set; }   // 바닥 색 관련 변수
    public float RandWait { get; set; }         // 코르틴 wait 기다릴값 담아주는 변수
    private Renderer ColorRend;
    WaitForSeconds NewWait = new WaitForSeconds(2f);
    GameObject ChildObject = null;

    private void Awake()
    {
        Puntest();
    }

    public void Puntest()
    {
        FloorColorRand = 150f;
        RandWait = 11f;
    }
   
    private void Start()
    {
        ColorRend = GetComponent<Renderer>();
    }
    
    public void OnEnableFunc()
    {
        StartCoroutine(DelayFloor());
    }

    IEnumerator DelayFloor()
    {
        ChildObject = gameObject.transform.GetChild(0).gameObject;
        while (ChildObject)
        {
            if (ChildObject.activeSelf)
            {
                yield return new WaitForSeconds(RandWait);
                ChildObject.GetComponentInChildren<Renderer>().material.color =
                    new Color(240 / 255, 255 / 255, 255 / 255);
                yield return NewWait;
                ChildObject.SetActive(false);
            }
            else if (!ChildObject.activeSelf)
            {
                yield return NewWait;
                ChildObject.SetActive(true);
                ChildObject.gameObject.GetComponentInChildren<Renderer>().material.color =
                    new Color(FloorColorRand / 255, 0 / 255, 0 / 255);
            }
        }
    }

}