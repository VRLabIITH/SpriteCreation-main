using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//allows the use of File class
using System.IO;
using SFB;


/// <summary>
/// This script is used for saving and loading sketches.
/// </summary>
/// <summary>
/// The Save function gets the filename from the input field and creates a text file using JsonUtility. 
/// The Load function opens a explorer window, where you can choose the file to be loaded. 
/// </summary>
public class FileManager : MonoBehaviour
{
    public GameObject fileNameInputField;

    public void Save()
    {
        string fileName = fileNameInputField.GetComponent<TMP_InputField>().text;
        string filePath = Application.dataPath + "/" + fileName + ".txt";

        SaveFile newSaveFile = new SaveFile(Camera.main.GetComponent<Draw>().listOfColouredLines);
        string jsonString = JsonUtility.ToJson(newSaveFile);
        File.WriteAllText(filePath, jsonString);
    }

    public void Load()
    {
        string fileName = fileNameInputField.GetComponent<TMP_InputField>().text;
        string filePath = Application.dataPath + "/" + fileName + ".txt";
        var extensions = new[] {
    new ExtensionFilter("", "", "", "txt") };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (File.Exists(paths[0]))
        {
            string jsonString = File.ReadAllText(paths[0]);
            SaveFile newSaveFile = JsonUtility.FromJson<SaveFile>(jsonString);
            newSaveFile.Draw();
            //Debug.Log("list of colliders: " + HapticDraw.listOfColliders.Count);
        }
        else
        {
            Debug.Log("No savefile with that name");
        }
    }
}