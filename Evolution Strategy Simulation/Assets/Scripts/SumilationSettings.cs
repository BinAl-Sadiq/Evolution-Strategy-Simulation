using UnityEngine;

public class SumilationSettings : MonoBehaviour
{
    private static SumilationSettings Instance;
    [HideInInspector] public int NeuralNetworkCount = 0;
    [HideInInspector] public float[][][][] NeuralWeight;
    [HideInInspector] public int GenerationNumber;
    [HideInInspector] public decimal FatherID;
    [HideInInspector] public float FatherFitness;

    void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
}