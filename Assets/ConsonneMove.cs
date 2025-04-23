using UnityEngine;

public class ConsonneMove : MonoBehaviour
{
    public float speed = 2f;
    public float leftBound = -10f;
    public float rightSpawn = 10f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > rightSpawn)
        {
            Vector3 pos = transform.position;
            pos.x = leftBound;
            transform.position = pos;
        }
    }
}
