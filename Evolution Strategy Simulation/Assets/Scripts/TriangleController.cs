using UnityEngine;

public class TriangleController : MonoBehaviour
{
    public float Speed = 200f;
    Rigidbody2D b;

    void Start()
    {
        b = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10f);
        b.velocity = Vector2.left * Speed;
    }

    private void Update()
    {
        if (!Brain.Triangle && Brain.HorizontalPosition < transform.position.x)
        {
            Brain.Triangle = gameObject;
        }
        else if (Brain.HorizontalPosition < transform.position.x)
        {
            float Distance_T_B = transform.position.x - Brain.HorizontalPosition;
            float Distance_T_BT = Brain.Triangle.transform.position.x - Brain.HorizontalPosition;

            if (Distance_T_B < Distance_T_BT)
                Brain.Triangle = gameObject;
        }
        else if (Brain.Triangle == gameObject)
        {
            Brain.Triangle = null;

            FindObjectOfType<SumilationUIManagement>().IncreaseScoreValue();
        }

        if (Brain.Triangle != gameObject)
        {
            transform.Find("Triangle").GetComponent<SpriteRenderer>().color = new Color(ColorsChanger.r, ColorsChanger.g, ColorsChanger.b);
            transform.Find("Triangle_").GetComponent<SpriteRenderer>().color = new Color(ColorsChanger.r, ColorsChanger.g, ColorsChanger.b);
        }
    }
}