using UnityEngine;

public class SquareController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.J))
        {
            horizontal -= 1f;
        }
        if (Input.GetKey(KeyCode.L))
        {
            horizontal += 1f;
        }
        if (Input.GetKey(KeyCode.I))
        {
            vertical += 1f;
        }
        if (Input.GetKey(KeyCode.K))
        {
            vertical -= 1f;
        }

        transform.position += new Vector3(horizontal, vertical, 0f) * speed * Time.deltaTime;
    }
}

