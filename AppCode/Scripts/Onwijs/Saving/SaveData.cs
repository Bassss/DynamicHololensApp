//using UnityEngine;    // For Debug.Log, etc.

//using System.Text;
//using System.IO;
//#if UNITY_EDITOR
//using System.Runtime.Serialization.Formatters.Binary;
//#endif

//using System;
//using System.Runtime.Serialization;
//using System.Reflection;
//using System.Collections.Generic;

//// === This is the info container class ===
//[Serializable()]
//public class SaveData : ISerializable
//{

//    // === Values ===
//    // Edit these during gameplay
//    public Dictionary<string, float> objectX = new Dictionary<string, float>();
//    public Dictionary<string, float> objectY = new Dictionary<string, float>();
//    public Dictionary<string, float> objectZ = new Dictionary<string, float>();
//    // === /Values ===

//    // The default constructor. Included for when we call it during Save() and Load()
//    public SaveData() { }

//    // This constructor is called automatically by the parent class, ISerializable
//    // We get to custom-implement the serialization process here
//    public SaveData(SerializationInfo info, StreamingContext ctxt)
//    {
//        // Get the values from info and assign them to the appropriate properties. Make sure to cast each variable.
//        // Do this for each var defined in the Values section above
//        objectX = (Dictionary<string, float>)info.GetValue("objectX", typeof(Dictionary<string, float>));
//        objectY = (Dictionary<string, float>)info.GetValue("objectY", typeof(Dictionary<string, float>));
//        objectZ = (Dictionary<string, float>)info.GetValue("objectZ", typeof(Dictionary<string, float>));
//    }

//    // Required by the ISerializable class to be properly serialized. This is called automatically
//    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
//    {
//        // Repeat this for each var defined in the Values section
//        info.AddValue("objectX", (objectX));
//        info.AddValue("objectY", (objectY));
//        info.AddValue("objectZ", (objectZ));
//    }
//}

//// === This is the class that will be accessed from scripts ===
//public class SaveLoad
//{
//    public static SaveData saveData;
//    public static string currentFilePath = "SaveData.OMR";    // Edit this for different save files


//    // Call this to write data
//    public static void Save()  // Overloaded
//    {
//        Save(currentFilePath);
//    }
//    public static void Save(string filePath)
//    {
//        SaveData data = SaveLoad.saveData;

//        Stream stream = File.Open(filePath, FileMode.Create);
//        BinaryFormatter bformatter = new BinaryFormatter();
//        bformatter.Binder = new VersionDeserializationBinder();
//        bformatter.Serialize(stream, data);
//        stream.Close();
//    }

//    // Call this to load from a file into "data"
//    public static void Load() { Load(currentFilePath); }   // Overloaded
//    public static void Load(string filePath)
//    {
//        SaveLoad.saveData = new SaveData();
//        Stream stream = File.Open(filePath, FileMode.Open);
//        BinaryFormatter bformatter = new BinaryFormatter();
//        bformatter.Binder = new VersionDeserializationBinder();
//        SaveLoad.saveData = (SaveData)bformatter.Deserialize(stream);
//        stream.Close();
//        Debug.Log(SaveLoad.saveData.objectX);
//        // Now use "data" to access your Values
//    }

//}

//// === This is required to guarantee a fixed serialization assembly name, which Unity likes to randomize on each compile
//// Do not change this
//public sealed class VersionDeserializationBinder : SerializationBinder
//{
//    public override Type BindToType(string assemblyName, string typeName)
//    {
//        if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
//        {
//            Type typeToDeserialize = null;

//            assemblyName = Assembly.GetExecutingAssembly().FullName;

//            // The following line of code returns the type. 
//            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

//            return typeToDeserialize;
//        }

//        return null;
//    }
//}
////#endif