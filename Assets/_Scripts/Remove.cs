using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour
{
    [SerializeField] private Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        RandomPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RandomPosition()
    {
        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        for (int i = 0; i < Snake.bodyList.Count; i++)
        {
            if (Snake.bodyList[i].transform.position.x == x && Snake.bodyList[i].transform.position.y == y)
            {
                RandomPosition();
            }
        }
        for (int i = 0; i < SnakeP2.bodyList.Count; i++)
        {
            if (SnakeP2.bodyList[i].transform.position.x == x && SnakeP2.bodyList[i].transform.position.y == y)
            {
                RandomPosition();
            }
        }
        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Snake") || other.CompareTag("SnakeP2"))
        {
            // Destroy the current object (this gameObject)
            RandomPosition();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, bounds.size);
    }
}
