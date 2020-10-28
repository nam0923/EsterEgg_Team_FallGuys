using UnityEngine;

public class Wheel_Rotater : MonoBehaviour
{
    GameObject PutObj;

    private bool isCollChar;

    void Update()
    {
        if (isCollChar)
        {
            OrbitAround();
        }
    }

    private void OrbitAround()
    {
        transform.RotateAround(PutObj.transform.position, Vector3.down, -30f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Wheel")
        {
            PutObj = collision.gameObject;
            isCollChar = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.transform.tag == "Wheel")
        {
            PutObj = collision.gameObject;
            isCollChar = false;
        }
    }

    
}