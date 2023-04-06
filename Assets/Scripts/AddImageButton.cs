using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddImageButton : MonoBehaviour
{
    public GameObject imageManager;
    public GameObject addImagebutton;
    public GameObject removeImagebutton;
    public GameObject indicator;
    public GameObject addTraceImageButton;
    public GameObject removeTraceImagebutton;

    public void AddImageFunction()
    {
        imageManager.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;
        addImagebutton.SetActive(false);
        removeImagebutton.SetActive(true);
    }
    public void RemoveImageFunction()
    {
        //foreach(GameObject go in imageManager.GetComponent<AddImage>().imageObjects)
        //{
        //    Destroy(go);
        //}   
        imageManager.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;
        addImagebutton.SetActive(true);
        removeImagebutton.SetActive(false);
        indicator.transform.position = new Vector3(100f, 0f, 0f);
    }
    public void AddTraceImage()
    {
        imageManager.SetActive(true);
        Camera.main.GetComponent<Draw>().enabled = false;

        addTraceImageButton.SetActive(false);
        removeTraceImagebutton.SetActive(true);
    }

    public void RemoveTraceImage()
    {
        imageManager.SetActive(false);
        Camera.main.GetComponent<Draw>().enabled = true;

        addTraceImageButton.SetActive(true);
        removeTraceImagebutton.SetActive(false);
    }
}
