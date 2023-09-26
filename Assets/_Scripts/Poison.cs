using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Poison : MonoBehaviour
{
    [SerializeField] private Bounds bounds;
    // Start is called before the first frame update
    private bool end = false;
    private int w = 0;
    void Start()
    {
        RandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            enabled = false;
            StartCoroutine(TestRoutine(1f));
        }
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
        if (other.CompareTag("Snake"))
        {
            // Destroy the current object (this gameObject)
            Debug.Log("P2 Win!");
            end = true;
            Time.timeScale = 0;
            w = 2;
        }
        else if(other.CompareTag("SnakeP2"))
        {
            Debug.Log("P1 Win!");
            end = true;
            Time.timeScale = 0;
            w = 1;
        }
    }
    IEnumerator TestRoutine(float duration)
    {
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSecondsRealtime(duration); ;
        Debug.Log($"Ended at {Time.time}");
        enabled = true;
        Time.timeScale = 1;
        if (w == 1)
        {
            SceneManager.LoadScene(3);
        }
        else if (w == 2)
        {
            SceneManager.LoadScene(4);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, bounds.size);
    }
}
