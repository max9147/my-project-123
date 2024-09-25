using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
}

[System.Serializable]
public struct RecordData
{
    public Difficulty Difficulty;

    public int Score;

    public RecordData(int _setScore, Difficulty _setDifficulty)
    {
        Score = _setScore;
        Difficulty = _setDifficulty;
    }
}

[System.Serializable]
public struct DataStruct
{
    public List<RecordData> RecordDatas;

    public Difficulty SelectedDifficulty;

    public bool SoundIsOn;
}

public class DataContainer : MonoBehaviour
{
    public static DataContainer Instance;

    public List<RecordData> CurrentRecords => _dataStruct.RecordDatas;

    public Difficulty CurrentDifficulty => _dataStruct.SelectedDifficulty;

    public bool CurrentSoundIsOn => _dataStruct.SoundIsOn;

    [SerializeField] private DataStruct _dataStruct;

    private string _dataPath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _dataPath = Path.Combine(Application.persistentDataPath, "Data.txt");

        if (File.Exists(_dataPath))
            _dataStruct = JsonUtility.FromJson<DataStruct>(File.ReadAllText(_dataPath));
        else
        {
            _dataStruct.RecordDatas = new List<RecordData>();
            _dataStruct.SelectedDifficulty = Difficulty.Normal;
            _dataStruct.SoundIsOn = true;

            File.WriteAllText(_dataPath, JsonUtility.ToJson(_dataStruct, true));
        }
    }

    public void ClearData()
    {
        _dataStruct.RecordDatas.Clear();

        SaveData();
    }

    public void ChangeDifficulty(Difficulty _setDifficulty)
    {
        _dataStruct.SelectedDifficulty = _setDifficulty;

        SaveData();
    }

    public void SubmitRecord(int _setScore)
    {
        _dataStruct.RecordDatas.Add(new RecordData(_setScore, CurrentDifficulty));

        SortRecords();
        SaveData();
    }

    public bool ToggleSound()
    {
        _dataStruct.SoundIsOn = !_dataStruct.SoundIsOn;

        SaveData();

        return _dataStruct.SoundIsOn;
    }

    private void SaveData()
    {
        File.WriteAllText(_dataPath, JsonUtility.ToJson(_dataStruct, true));
    }

    private void SortRecords()
    {
        _dataStruct.RecordDatas = _dataStruct.RecordDatas.OrderByDescending(x => x.Score).ToList();
    }
}