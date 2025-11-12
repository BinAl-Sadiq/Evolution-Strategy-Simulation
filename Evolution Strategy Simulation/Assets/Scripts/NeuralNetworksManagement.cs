using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworksManagement : MonoBehaviour
{
    public GameObject Prefab;

    private bool isTraining = false;
    public int populationSize = 1;
    [HideInInspector]
    public int generationNumber = -1;
    public int[] layers = new int[] { 5, 10, 10, 1 };
    [HideInInspector] public List<NeuralNetwork> nets;
    [HideInInspector] public NeuralNetwork FatherNet; //The best neuron network.
    private List<Brain> BrainsList = null;

    void Start()
    {
        populationSize = FindObjectOfType<SumilationSettings>().NeuralNetworkCount;
    }

    void Update()
    {
        if (isTraining == false)
        {
            if (generationNumber == -1)
            {
                InitPrefabsNeuralNetworks(ref FindObjectOfType<SumilationSettings>().NeuralWeight);
            }
            else
            {
                if (populationSize > 1)
                {
                    nets.Sort();

                    if (FatherNet == null || FatherNet.GetFitness() < nets[nets.Count - 1].GetFitness())
                    {
                        FatherNet = new NeuralNetwork(nets[nets.Count - 1], nets[nets.Count - 1].ID);
                        FatherNet.SetFitness(nets[nets.Count - 1].GetFitness());
                    }

                    for (int i = 0; i < populationSize / 2; i++)
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i].Mutate();

                        nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    }
                }
                else
                {
                    if (FatherNet == null && nets[0].GetFitness() < 10f)
                    {
                        nets[0] = new NeuralNetwork(layers);
                    }
                    else if (FatherNet != null)
                    {
                        if (FatherNet.GetFitness() >= nets[0].GetFitness())
                        {
                            nets[0] = new NeuralNetwork(FatherNet);
                            nets[0].Mutate();
                        }
                        else if (FatherNet.GetFitness() < nets[0].GetFitness())
                        {
                            FatherNet = new NeuralNetwork(nets[0], nets[0].ID);
                            FatherNet.Mutate();

                            nets[0].Mutate();

                            FatherNet.SetFitness(nets[0].GetFitness());
                        }
                    }
                    else
                    {
                        FatherNet = new NeuralNetwork(nets[0], nets[0].ID);
                        FatherNet.Mutate();

                        nets[0].Mutate();

                        FatherNet.SetFitness(nets[0].GetFitness());
                    }
                }
            }

            foreach (NeuralNetwork net in nets)
            {
                net.SetFitness(0f);
            }
            FindObjectOfType<TriSpawner>().Reset_();
            FindObjectOfType<SumilationUIManagement>().ResetTexts();
            generationNumber++;

            isTraining = true;
            CreatePrefabs();

            Brain.Remain = populationSize;
        }

        foreach (Brain b in BrainsList.ToArray())
        {
            if (b.IsDead)
            {
                isTraining = false;
            }
            else
            {
                isTraining = true;
                break;
            }
        }
    }

    private void CreatePrefabs()
    {
        if (BrainsList != null)
        {
            for (int i = 0; i < BrainsList.Count; i++)
            {
                Destroy(BrainsList[i].gameObject);
            }
        }

        BrainsList = new List<Brain>();

        for (int i = 0; i < populationSize; i++)
        {
            Brain brain = Instantiate(Prefab, new Vector3(), Quaternion.identity).GetComponent<Brain>();
            brain.Init(nets[i]);
            BrainsList.Add(brain);
        }
    }

    void InitPrefabsNeuralNetworks(ref float[][][][] Weights)
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize > 1 && populationSize % 2 != 0)
        {
            populationSize++;
        }

        nets = new List<NeuralNetwork>();

        if (Weights == null)
        {
            for (int i = 0; i < populationSize; i++)
            {
                NeuralNetwork net = new NeuralNetwork(layers);
                nets.Add(net);
            }
        }
        else
        {
            for (int i = 0; i < FindObjectOfType<SumilationSettings>().NeuralWeight.Length; i++)
            {
                NeuralNetwork net = new NeuralNetwork(layers, FindObjectOfType<SumilationSettings>().NeuralWeight[i]);
                nets.Add(net);
            }

            FatherNet = new NeuralNetwork(layers, FindObjectOfType<SumilationSettings>().NeuralWeight[FindObjectOfType<SumilationSettings>().NeuralWeight.Length - 1], FindObjectOfType<SumilationSettings>().FatherID);
            FatherNet.SetFitness(FindObjectOfType<SumilationSettings>().FatherFitness);

            populationSize = FindObjectOfType<SumilationSettings>().NeuralWeight.Length;
            generationNumber = FindObjectOfType<SumilationSettings>().GenerationNumber - 1;
        }
    }
}