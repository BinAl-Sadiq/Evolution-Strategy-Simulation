using UnityEngine;
using System.Collections.Generic;

public class TriSpawner : MonoBehaviour
{
    public GameObject TriAnglePrefab;
    public float Time = 4f;
    private float Timer;
    List<GameObject> Triangles;

    public void Reset_()
    {
        Timer = Time;

        foreach (GameObject o in Triangles)
        {
            Destroy(o);
        }
        Triangles = new List<GameObject>();
    }

    void Start()
    {
        transform.position = new Vector3(Manager.Width + 4f/*Offset*/, 0f);

        Timer = Time;

        Triangles = new List<GameObject>();
    }

    void Update()
    {
        if (Timer <= 0f)
        {
            Timer = Time;

            Triangles.Add(Instantiate(TriAnglePrefab, new Vector3(transform.position.x, Random.Range(-Manager.Width + 10f, Manager.Width - 10f)), Quaternion.identity));
        }
        else
            Timer -= UnityEngine.Time.deltaTime;
    }
}
