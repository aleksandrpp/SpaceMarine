using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace AK.SpaceMarine
{
    public class WorldData
    {
        private int _score;

        public int Score
        {
            get => _score;
            set => _score += value;
        }
    }
    
    [Serializable]
    public class UserData
    {
        public int 
            BestScore, 
            LastScore;

        public void SaveToFile()
        {
            using FileStream fs = File.Create(GetPath());
            string json = JsonConvert.SerializeObject(this);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            fs.Write(bytes, 0, bytes.Length);
        }

        public void LoadFromFile()
        {
            string path = GetPath();
            if (!File.Exists(path)) return;
            
            using StreamReader sr = File.OpenText(path);
            string json = sr.ReadLine();
            if (json == null) return;
            
            UserData data = JsonConvert.DeserializeObject<UserData>(json);
            
            BestScore = data.BestScore;
            LastScore = data.LastScore;

            Debug.Log($"Loaded from {path}");
        }

        private string GetPath(params string[] path)
        {
            return $"{Path.Combine(Application.persistentDataPath, GetType().Name, Path.Combine(path))}.json";
        }
    }
}