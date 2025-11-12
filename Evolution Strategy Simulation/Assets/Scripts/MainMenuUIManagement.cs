using UnityEngine;
using TMPro;

public class MainMenuUIManagement : MonoBehaviour
{
    public TMP_InputField CountOfNeuralNettworks;

    public TMP_InputField PathToLoad;

    void Start()
    {
        PathToLoad.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }

    public void StartNewSimulation()
    {
        FindObjectOfType<SumilationSettings>().NeuralNetworkCount =  int.Parse(CountOfNeuralNettworks.text);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ContinueLoadedSimulation()
    {
        DataManagement.Load(PathToLoad.text, ref FindObjectOfType<SumilationSettings>().NeuralWeight, ref FindObjectOfType<SumilationSettings>().GenerationNumber, ref FindObjectOfType<SumilationSettings>().FatherID, ref FindObjectOfType<SumilationSettings>().FatherFitness);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void CloseTheSimulator()
    {
        Application.Quit();
    }
}