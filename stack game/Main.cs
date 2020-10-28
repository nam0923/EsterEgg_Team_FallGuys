using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject CurrentCube;
    public GameObject LastCube;
    public Text Text;
    public int Level;
    public bool Done;

    void Start()
    {
        newBlock();    
    }

    private void newBlock()
    {
        if(LastCube != null)
        {
            CurrentCube.transform.position = new Vector3(Mathf.Round(CurrentCube.transform.position.x), CurrentCube.transform.position.y, Mathf.Round(CurrentCube.transform.position.z));
            CurrentCube.transform.localScale = new Vector3(LastCube.transform.localScale.x - Mathf.Abs(CurrentCube.transform.position.x - LastCube.transform.position.x),
                                                          LastCube.transform.localScale.y,
                                                          LastCube.transform.localScale.z - Mathf.Abs(CurrentCube.transform.position.z - LastCube.transform.position.z));
            CurrentCube.transform.position = Vector3.Lerp(CurrentCube.transform.position, LastCube.transform.position, 0.5f) + Vector3.up * 5f;
            if(CurrentCube.transform.localScale.x <=0f || CurrentCube.transform.localScale.z <=0f)
            {
                Done = true;
                Text.gameObject.SetActive(true);
                Text.text = "Your Score : " + Level;
                if (Level >= 20)
                {
                    Text.text = "드디어!! 다음게임으로 넘어갈수 있어!! 달려보자구! " ;
                    StartCoroutine(x());
                }
                if(Level<20)
                {
                    Text.text = "이게임만 할꺼야? 좀더 분발하라구!! 다시 기회를 줄께. ";
                    StartCoroutine(y());
                }
                return;
            }
        }
        LastCube = CurrentCube;
        CurrentCube = Instantiate(LastCube);
        CurrentCube.name = Level + "";
        Text.gameObject.SetActive(true);
        Text.text = "미션: 20점을 넘겨라! " +" Your Score : " + Level;
        CurrentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((Level / 100f) % 1f, 1f, 1f));
        Level++;
        Camera.main.transform.position = CurrentCube.transform.position + new Vector3(100, 100, 100);
        Camera.main.transform.LookAt(CurrentCube.transform.position);
    }
  
    void Update()
    {
        if (Done)
            return;

        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);

        var pos1 = LastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((Level % 2 == 0) ? Vector3.left : Vector3.forward) * 120;

        if(Level %2 ==0)
        {
            CurrentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        }
        else
        {
            CurrentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        }

        if (Input.GetMouseButtonDown(0))
            newBlock();
    }
    IEnumerator y()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Puzzle");
    }
    IEnumerator x()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Running");
  
    }
}
