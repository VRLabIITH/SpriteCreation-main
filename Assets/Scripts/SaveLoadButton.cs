using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadButton : MonoBehaviour
{
    public GameObject SaveButton;
    public GameObject LoadButton;
    public GameObject FileNameInput;
    public GameObject saveLoadButton;

    public void ActivateSaveLoad()
    {

        SaveButton.SetActive(true);
        LoadButton.SetActive(true);
        FileNameInput.SetActive(true);
        saveLoadButton.SetActive(false);
    }

    public void DeactivateSaveLoad()
    {
        SaveButton.SetActive(false);
        LoadButton.SetActive(false);
        FileNameInput.SetActive(false);
        saveLoadButton.SetActive(true);
    }
}
