using UnityEngine;

public class Brain : MonoBehaviour
{
    [HideInInspector]
    public NeuralNetwork net;
    private Rigidbody2D b;
    private float JumpForce = 350f;
    [HideInInspector]
    public bool IsDead = false;

    public static GameObject Triangle;
    public static float HorizontalPosition;

    private float[] inputs = new float[5];
    public GameObject LinePrefab;
    private GameObject[] Lines;

    private Transform BorderUp, BorderDown;

    public static bool displayLines = true;

    public static int Remain = 0;

    void Start()
    {
        Lines = new GameObject[inputs.Length];

        b = GetComponent<Rigidbody2D>();

        HorizontalPosition = transform.position.x;

        foreach (GameObject O in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (O.name == "Border Up")
            {
                BorderUp = O.transform;
            }
            else if (O.name == "Border Down")
            {
                BorderDown = O.transform;
            }
        }
    }

    void Update()
    {
        if (!IsDead)
        {
            float Angle = Mathf.Atan2(b.velocity.y, x: 10f) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);

            net.AddFitness(Time.deltaTime);

            if (Triangle)
            {
                inputs[0] = Triangle.transform.Find("Up").position.y - transform.position.y;
                inputs[1] = Triangle.transform.Find("Down").position.y - transform.position.y;
                inputs[2] = Triangle.transform.Find("Score").position.x - transform.position.x;

                if (!Lines[0])
                {
                    Lines[0] = Instantiate(LinePrefab, transform);
                    Lines[1] = Instantiate(LinePrefab, transform);
                    Lines[2] = Instantiate(LinePrefab, transform);
                }

                LinePrinter(Lines[0], Triangle.transform.Find("Up"));
                LinePrinter(Lines[1], Triangle.transform.Find("Down"));
                LinePrinter(Lines[2], Triangle.transform.Find("Score"));
            }

            inputs[3] = Manager.Height - transform.Find("Square").position.y;
            inputs[4] = -Manager.Height - transform.Find("Square").position.y;

            float[] outputs = net.FeedForward(inputs);
            Jumping(outputs[0]);

            if (!Lines[3])
            {
                Lines[3] = Instantiate(LinePrefab, transform);
                Lines[4] = Instantiate(LinePrefab, transform);
            }
            
            LinePrinter(Lines[3], BorderUp);
            LinePrinter(Lines[4], BorderDown);

            if (!displayLines && Lines[0]) Lines[0].SetActive(false);
            else if (displayLines && Lines[0]) Lines[0].SetActive(true);
            if (!displayLines && Lines[1]) Lines[1].SetActive(false);
            else if (displayLines && Lines[1]) Lines[1].SetActive(true);
            if (!displayLines && Lines[2]) Lines[2].SetActive(false);
            else if (displayLines && Lines[2]) Lines[2].SetActive(true);
            if (!displayLines && Lines[3]) Lines[3].SetActive(false);
            else if (displayLines && Lines[3]) Lines[3].SetActive(true);
            if (!displayLines && Lines[4]) Lines[4].SetActive(false);
            else if (displayLines && Lines[4]) Lines[4].SetActive(true);
        }
    }

    private void LinePrinter(GameObject Line, Transform Target)
    {
        float Y = Target.position.y - transform.Find("Square").position.y;
        float X = Target.position.x - transform.Find("Square").position.x;

        Line.transform.localScale = new Vector3(Vector2.Distance(Target.position, transform.Find("Square").position) * 2f, .1f);
        Line.transform.position = new Vector3((Target.position.x + transform.position.x) / 2f, (Target.position.y + transform.position.y) / 2f);
        Line.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(Y, X) * Mathf.Rad2Deg, Vector3.forward);
    }

    private void Jumping(float Output)
    {
        b.velocity = Vector2.up * JumpForce * Output * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Triangle" || other.tag == "Border")
        {
            Die();
        }
    }

    public void Init(NeuralNetwork net)
    {
        this.net = net;
    }

    private void Die()
    {
        if (IsDead)
            return;

        Remain--;
        IsDead = true;

        foreach (GameObject Line in Lines)
        {
            Destroy(Line);
        }

        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        Destroy(transform.Find("Idle").gameObject);
        transform.Find("Square").GetComponent<SpriteRenderer>().color = Color.black;
        transform.Find("Square").GetComponent<SpriteRenderer>().sortingOrder = -1;
    }
}