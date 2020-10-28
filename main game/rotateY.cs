using UnityEngine;

public class rotateY : MonoBehaviour
{
    public float Speed { get; private set; }

    private void Start()
    {
        Speed = -70f;
    }
    private void Update()
    {
        Orbit_Rotation();
    }

    void Orbit_Rotation()
    {
        transform.Rotate(Vector3.down * Speed * Time.deltaTime);
    }

}
