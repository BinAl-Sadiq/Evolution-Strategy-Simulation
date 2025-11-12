using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManagement
{
    public static void Save(string DataFilePath, float[][][][] WeightsArray, int GenerationNumber, decimal FatherID, float FatherFitness)
    {
        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream Stream = new FileStream(DataFilePath, FileMode.Create);

        DataGetter DataSetter = new DataGetter();
        DataSetter.WeightsArray = WeightsArray;
        DataSetter.GenerationNumber = GenerationNumber;
        DataSetter.FatherID = FatherID;
        DataSetter.FatherFitness = FatherFitness;
        Formatter.Serialize(Stream, DataSetter);

        Stream.Close();
    }

    public static void Load(string DataFilePath, ref float[][][][] WeightsArray, ref int GenerationNumber, ref decimal FatherID, ref float FatherFitness)
    {
        if (File.Exists(DataFilePath))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(DataFilePath, FileMode.Open);

            DataGetter Data = Formatter.Deserialize(Stream) as DataGetter;
            WeightsArray = Data.WeightsArray;
            GenerationNumber = Data.GenerationNumber;
            FatherID = Data.FatherID;
            FatherFitness = Data.FatherFitness;
            Stream.Close();
        }
        else
            Debug.LogWarning("The Data File \"" + DataFilePath + "\" is not exist!!");
    }
}

[System.Serializable]
public class DataGetter
{
    public float[][][][] WeightsArray;
    public int GenerationNumber;
    public decimal FatherID;
    public float FatherFitness;
}