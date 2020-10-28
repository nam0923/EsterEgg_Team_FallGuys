using UnityEngine;

public class propeller : MonoBehaviour
{
    private float m_propeller;
    void Start()
    {
        m_propeller = Random.Range(360f, 720f);
    }


    void Update()
    {
        transform.Rotate(0, m_propeller * Time.deltaTime, 0);
    }
}
