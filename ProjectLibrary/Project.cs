using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectLibrary
{
    [Serializable]
    public class Project
    {
       
        public Dictionary<string,EntityCharacteristics> EntitiesCharacteristics = new Dictionary<string, EntityCharacteristics>();
        public Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        public void Save(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using(Stream stream= new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(stream, this);
            }
        }
        public static Project Load(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                Project project = formatter.Deserialize(stream) as Project;
                foreach (var item in project.EntitiesCharacteristics)
                {
                    item.Value.SetShader();
                }
             return project;
            }
        }
    }
}
