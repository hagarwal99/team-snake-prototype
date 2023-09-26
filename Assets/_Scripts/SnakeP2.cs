using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeP2 : MonoBehaviour
{
    private Vector3 dir = Vector3.left;
    private float timer;
    [SerializeField] private GameObject bodyPrefab;

    private float hungryTimer;
    private bool canMove;

    public static List<GameObject> bodyList;

    private DirType typed;

    private bool end = false;
    private int w = 0;
    // Start is called before the first frame update
    void Awake()
    {
        bodyList = new List<GameObject>();
    }
    void Start()
    {
        bodyList.Add(this.gameObject);
        Vector3 initialPosition = transform.position; // Get the initial position of the snake's head

        for (int i = 0; i < 6; i++)
        {
            GameObject body = Instantiate(bodyPrefab);

            // Calculate the position of the new body part to be one unit to the left of the previous body part
            initialPosition.x += 1.0f; // Adjust the '1.0f' as needed for the spacing you want
            body.transform.position = initialPosition;

            bodyList.Add(body);
        }
        typed = DirType.NONE;
        canMove = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (end)
        {
            enabled = false;
            StartCoroutine(TestRoutine(1f));
        }
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && typed != DirType.DOWN)
            {
                dir = Vector3.up;
                typed = DirType.UP;
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && typed != DirType.UP)
            {
                dir = Vector3.down;
                typed = DirType.DOWN;
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && typed != DirType.RIGHT)
            {
                dir = Vector3.left;
                typed = DirType.LEFT;
                canMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && typed != DirType.LEFT)
            {
                dir = Vector3.right;
                typed = DirType.RIGHT;
                canMove = false;
            }

        }
    }

    private void GrowBody()
    {
        GameObject body = Instantiate(bodyPrefab);
        body.transform.position = bodyList[bodyList.Count - 1].transform.position;
        bodyList.Add(body);
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
        else if (w == 3)
        {
            SceneManager.LoadScene(5);
        }
    }
    private void ShrinkBody()
    {
        if (bodyList.Count > 1)
        {
            GameObject lastBody = bodyList[bodyList.Count - 1];
            bodyList.Remove(lastBody);
            Destroy(lastBody);
        }
        else
        {
            Debug.Log("P2 Win!");
            end = true;
            Time.timeScale = 0;
            w = 2;
        }
    }

    private void FixedUpdate()
    {
        // Calculate the new speed based on the number of body parts
        float speed = 0.2f - (bodyList.Count - 1) * 0.01f;

        // Ensure that the speed doesn't go below the minimum value (0.1f)
        speed = Mathf.Max(speed, 0.1f);

        // Move the snake when it can move
        timer += Time.deltaTime;
        hungryTimer += Time.deltaTime;

        if (hungryTimer > 10)
        {
            Debug.Log("P1 win!");
            end = true;
            Time.timeScale = 0;
            w = 1;
        }

        if (timer > speed)
        {
            for (int i = bodyList.Count - 1; i > 0; i--)
            {
                bodyList[i].transform.position = bodyList[i - 1].transform.position;
            }
            canMove = true;
            transform.position += dir;
            timer = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Add"))
        {
            hungryTimer = 0;
            GrowBody();
        }
        else if (other.CompareTag("Remove"))
        {
            hungryTimer = 0;
            ShrinkBody();
        }
        else if (other.CompareTag("body") || other.CompareTag("Block"))
        {
            Debug.Log("P1 Win!");
            end = true;
            Time.timeScale = 0;
            w = 1;
        }
        else if (other.CompareTag("Snake"))
        {
            Debug.Log("Tied!");
            end = true;
            Time.timeScale = 0;
            w = 3;
        }
    }
}
