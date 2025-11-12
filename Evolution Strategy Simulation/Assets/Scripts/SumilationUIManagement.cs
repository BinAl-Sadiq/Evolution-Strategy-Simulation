using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SumilationUIManagement : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Generation;
    public TextMeshProUGUI Fitness;
    public TextMeshProUGUI Father;
    public TextMeshProUGUI FatherFitness;
    public TextMeshProUGUI Count;
    public TextMeshProUGUI Remain;

    public Text Brain_ID;
    public Text Brain_Fitness;
    public GameObject Brain_Panel;
    public GameObject Brain_GraphPanel;
    public Sprite NeuralIcon;
    public Sprite Pixel;
    List<GameObject[][]> connections;
    public TextMeshProUGUI showNeuralNetworkText;
    public GameObject Option_Panel;

    public Text StatsControllerT;
    public Text ScoreControllerT;
    public Text LinesControllerT;
    public Text MusicController;

    public GameObject FilePathPanel;
    public TMP_InputField FilePathInputfield;
    public Button FilePathSaveButton;
    public GameObject ErrorMessagePanel;
    public Text ErrorMessage;

    private bool firstGen = true;

    private void Start()
    {
        drawGraph();
    }

    void Update()
    {
        ChangeGenerationTextValue();
        CountTheRemain();
        Father.text = "Father: " + (FindObjectOfType<NeuralNetworksManagement>().FatherNet != null ? FindObjectOfType<NeuralNetworksManagement>().FatherNet.ID.ToString() : "null");
        FatherFitness.text = "Father Fitness: " + (FindObjectOfType<NeuralNetworksManagement>().FatherNet != null ? FindObjectOfType<NeuralNetworksManagement>().FatherNet.GetFitness().ToString() : "0");

        Fitness.text = "Fitness: " + (float.Parse(Fitness.text.Replace("Fitness: ", "")) + Time.deltaTime).ToString();

        if (Input.GetMouseButtonUp(1) && !Option_Panel.activeSelf)
        {
            RaycastHit2D obj = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (obj && obj.collider.tag == "Brain")
            {
                Brain_Panel.SetActive(true);

                Brain_ID.text = "ID: " + obj.transform.gameObject.GetComponent<Brain>().net.ID.ToString();
                Brain_Fitness.text = "Fitness: " + obj.transform.gameObject.GetComponent<Brain>().net.GetFitness().ToString();
                printWieght(obj.transform.gameObject.GetComponent<Brain>().net);
            }
            else
                Brain_Panel.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
            if (Option_Panel.activeSelf)
            {
                if (FilePathPanel.activeSelf)
                    FilePathPanel.SetActive(false);

                Option_Panel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Option_Panel.SetActive(true);
                Time.timeScale = 0;
            }
    }

    public void ResetTexts()
    {
        if (firstGen)
            firstGen = false;
        else
        {
            Score.text = "0";
            Fitness.text = "Fitness: 0";
        }
    }

    public void IncreaseScoreValue()
    {
        Score.text = (int.Parse(Score.text) + 1).ToString();
    }

    public void ChangeGenerationTextValue()
    {
        Generation.text = "Generation: " + FindObjectOfType<NeuralNetworksManagement>().generationNumber;
    }

    private void CountTheRemain()
    {
        if (FindObjectOfType<NeuralNetworksManagement>().populationSize == 1)
            return;

        Count.text = "Count: " + FindObjectOfType<NeuralNetworksManagement>().populationSize.ToString();
        Remain.text = "Remain: " + Brain.Remain.ToString();
    }

    private void drawGraph()
    {
        int[] layers = FindObjectOfType<NeuralNetworksManagement>().layers;

        Vector2 startPoint = new Vector2(Brain_GraphPanel.GetComponent<RectTransform>().sizeDelta.x / -2f + 37.5f /*offset*/, Brain_GraphPanel.GetComponent<RectTransform>().sizeDelta.y / 2f - 37.5f /*offset*/);
        Vector2 keepADistanceAmount = new Vector2(150f, -75f);

        int mostPop = 0;//value of the layer with the most neurons.
        for (int i = 0; i < layers.Length; i++)
            if (mostPop < layers[i])
                mostPop = layers[i];

        List<GameObject[]> neurons = new List<GameObject[]>();
        connections = new List<GameObject[][]>();

        List<GameObject> inputsNeurons = new List<GameObject>();
        for (int i = 0; i < layers[0]; i++)
        {
            inputsNeurons.Add(new GameObject("inputsNeuron"));
            inputsNeurons[i].transform.parent = Brain_GraphPanel.transform;
            inputsNeurons[i].AddComponent<Image>().sprite = NeuralIcon;
            inputsNeurons[i].GetComponent<RectTransform>().localPosition = new Vector3(startPoint.x, (mostPop > layers[0] ? (mostPop - (float)layers[0]) / 2f * keepADistanceAmount.y : 0f) + startPoint.y + keepADistanceAmount.y * i);
            inputsNeurons[i].GetComponent<RectTransform>().localScale = new Vector3(.75f, .75f, 1f);
        }
        neurons.Add(inputsNeurons.ToArray());

        for (int b = 1; b < layers.Length; b++)
        {
            List<GameObject> HiddenAndOutputsNeurons = new List<GameObject>();
            List<GameObject[]> layersNeuronsConnections = new List<GameObject[]>();

            for (int bb = 0; bb < layers[b]; bb++)
            {
                HiddenAndOutputsNeurons.Add(new GameObject("neuron"));
                HiddenAndOutputsNeurons[bb].transform.parent = Brain_GraphPanel.transform;
                HiddenAndOutputsNeurons[bb].AddComponent<Image>().sprite = NeuralIcon;
                HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localPosition = new Vector3(startPoint.x + keepADistanceAmount.x * b, (mostPop > layers[b] ? (mostPop - (float)layers[b]) / 2f * keepADistanceAmount.y : 0f) + startPoint.y + keepADistanceAmount.y * bb, 0f);
                HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localScale = new Vector3(.75f, .75f, 1f);

                List<GameObject> layerNeuronsConnections = new List<GameObject>();
                for (int i = 0; i < layers[b - 1]; i++)
                {
                    float differentX = neurons[b - 1][i].GetComponent<RectTransform>().localPosition.x - HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localPosition.x;
                    float differentY = neurons[b - 1][i].GetComponent<RectTransform>().localPosition.y - HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localPosition.y;

                    float centerX = (neurons[b - 1][i].GetComponent<RectTransform>().localPosition.x + HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localPosition.x) / 2f;
                    float centerY = (neurons[b - 1][i].GetComponent<RectTransform>().localPosition.y + HiddenAndOutputsNeurons[bb].GetComponent<RectTransform>().localPosition.y) / 2f;

                    float angle = Mathf.Atan2(differentY, differentX) * Mathf.Rad2Deg;

                    layerNeuronsConnections.Add(new GameObject("ConnectionLine"));
                    layerNeuronsConnections[i].transform.parent = Brain_GraphPanel.transform;
                    layerNeuronsConnections[i].AddComponent<Image>().sprite = Pixel;
                    layerNeuronsConnections[i].GetComponent<Image>().color = Color.black;
                    layerNeuronsConnections[i].GetComponent<RectTransform>().localPosition = new Vector3(centerX, centerY, 0f);
                    layerNeuronsConnections[i].GetComponent<RectTransform>().localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    layerNeuronsConnections[i].GetComponent<RectTransform>().localScale = new Vector3(Mathf.Sqrt(Mathf.Pow(differentX, 2) + Mathf.Pow(differentY, 2)), 1f, 1f);
                    layerNeuronsConnections[i].GetComponent<RectTransform>().sizeDelta = new Vector3(1f, 1f, 1f);
                }
                layersNeuronsConnections.Add(layerNeuronsConnections.ToArray());
            }
            connections.Add(layersNeuronsConnections.ToArray());
            neurons.Add(HiddenAndOutputsNeurons.ToArray());
        }
    }

    private void printWieght(NeuralNetwork nt)
    {
        for (int i = 1; i < nt.layers.Length; i++)
        {
            for (int ii = 0; ii < nt.layers[i]; ii++)
            {
                for (int iii = 0; iii < nt.layers[i - 1]; iii++)
                {
                    connections[i - 1][ii][iii].GetComponent<Image>().color = colorSelector(nt.weights[i - 1][ii][iii]);
                }
            }
        }
    }

    private Color colorSelector(float v)
    {
        float r = v > 0f ? 1f - v / .5f * 1f : v < 0f ? v / .5f * 1f : 0f;
        float g = v > 0f ? v / .5f * 1f : v < 0f ? 1f - v / .5f * 1f : 0f;
        float b = v > .5f && v < 1f || v < -.5f && v > -1f ? .5f : v > 1f || v < -1f ? 1f : 0f;

        return new Color(r, g, b);
    }

    public void showNeuralNetwok()
    {
        if (Brain_GraphPanel.activeSelf)
        {
            Brain_GraphPanel.SetActive(false);
            showNeuralNetworkText.text = "Show Neural Network";
        }
        else
        {
            Brain_GraphPanel.SetActive(true);
            showNeuralNetworkText.text = "Hide Neural Network";
        }
    }

    public void StatsController()
    {
         if (!Generation.gameObject.activeSelf)
         {
              StatsControllerT.text = "Hide Stats";

              Generation.gameObject.SetActive(true);
              Fitness.gameObject.SetActive(true);
              Father.gameObject.SetActive(true);
              FatherFitness.gameObject.SetActive(true);
              Count.gameObject.SetActive(true);
              Remain.gameObject.SetActive(true);
         }
         else
         {
              StatsControllerT.text = "Show Stats";

              Generation.gameObject.SetActive(false);
              Fitness.gameObject.SetActive(false);
              Father.gameObject.SetActive(false);
              FatherFitness.gameObject.SetActive(false);
              Count.gameObject.SetActive(false);
              Remain.gameObject.SetActive(false);
         }
    }

    public void ScoreController()
    {
        if (!Score.gameObject.activeSelf)
        {
            ScoreControllerT.text = "Hide Score";

            Score.gameObject.SetActive(true);
        }
        else
        {
            ScoreControllerT.text = "Show Score";

            Score.gameObject.SetActive(false);
        }
    }

    public void LinesController()
    {
        if (Brain.displayLines)
        {
            LinesControllerT.text = "Show Lines";

            Brain.displayLines = false;
        }
        else
        {
            LinesControllerT.text = "Hide Lines";

            Brain.displayLines = true;
        }
    }

    public void SaveNeuralNetwork()
    {
        if (FindObjectOfType<NeuralNetworksManagement>().generationNumber == 0)
        {
            ErrorMessage.text = "You can not save the first generation!";
            ErrorMessagePanel.GetComponent<Animator>().Play("Appear");
        }
        else
        {
            FilePathInputfield.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\Neural Networks.bin";

            FilePathPanel.SetActive(true);
            FilePathSaveButton.onClick.RemoveAllListeners();

            float[][][][] WeightsArray = new float[1][][][];
            WeightsArray[0] = FindObjectOfType<NeuralNetworksManagement>().FatherNet.weights;

            FilePathSaveButton.onClick.AddListener(() => { DataManagement.Save(FilePathInputfield.text, WeightsArray, FindObjectOfType<NeuralNetworksManagement>().generationNumber, FindObjectOfType<NeuralNetworksManagement>().FatherNet.ID, FindObjectOfType<NeuralNetworksManagement>().FatherNet.GetFitness()); FilePathPanel.SetActive(false); });
        }
    }

    public void TurnMusic()
    {
        if (MusicController.text == "Turn Off Music")
        {
            FindObjectOfType<AudioSource>().Pause();
            MusicController.text = "Turn On Music";
        }
        else
        {
            FindObjectOfType<AudioSource>().Play();
            MusicController.text = "Turn Off Music";
        }
    }

    public void SaveTopHalfNeuralNetworks()
    {
        if (FindObjectOfType<NeuralNetworksManagement>().populationSize < 4)
        {
            ErrorMessage.text = "You do not have a lot of neural networks just use the other button!" + (FindObjectOfType<NeuralNetworksManagement>().generationNumber == 0 ? " you can not save the first generation!" : "");
            ErrorMessagePanel.GetComponent<Animator>().Play("Appear");

            return;
        }
        else if (FindObjectOfType<NeuralNetworksManagement>().generationNumber == 0)
        {
            ErrorMessage.text = "You can not save the first generation!";
            ErrorMessagePanel.GetComponent<Animator>().Play("Appear");

            return;
        }

        FilePathInputfield.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\Neural Networks.bin";

        FilePathPanel.SetActive(true);
        FilePathSaveButton.onClick.RemoveAllListeners();

        float[][][][] WeightsArray = new float[FindObjectOfType<NeuralNetworksManagement>().nets.Count][][][];
        
        for (int i = 0; i < WeightsArray.Length; i++)
        {
            WeightsArray[i] = FindObjectOfType<NeuralNetworksManagement>().nets[i].weights;
        }

        FilePathSaveButton.onClick.AddListener(() => { DataManagement.Save(FilePathInputfield.text, WeightsArray, FindObjectOfType<NeuralNetworksManagement>().generationNumber, FindObjectOfType<NeuralNetworksManagement>().FatherNet.ID, FindObjectOfType<NeuralNetworksManagement>().FatherNet.GetFitness()); FilePathPanel.SetActive(false); });
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}