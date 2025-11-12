using UnityEngine;

public class Manager : MonoBehaviour
{
    public static float Width, Height;
    public GameObject Up, Down;

    void Awake()
    {
        Width = 1f / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f) / 2f;
        Height = 1f / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f) / 2f;

        Up.transform.position = new Vector3(0f, Height);
        Down.transform.position = new Vector3(0f, -Height);

        Up.transform.localScale = new Vector3(Width * 2f, 2f);
        Down.transform.localScale = new Vector3(Width * 2f, 2f);
    }
}