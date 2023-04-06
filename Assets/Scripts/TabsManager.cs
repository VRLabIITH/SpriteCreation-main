using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Tabs manager script is used to manage the Sketch, Build and Simulate tabs. 
/// </summary>
/// <summary>
/// HideAllTabs is used to set all the tabs as inactive.
/// ShowTab is used for activating the specific tab when the tab button is clicked. 
/// </summary>
public class TabsManager : MonoBehaviour
{
    public GameObject tabbutton1;
    public GameObject tabbutton2;
    public GameObject tabbutton3;

    public GameObject tabcontent1;
    public GameObject tabcontent2;
    public GameObject tabcontent3;



    public void HideAllTabs()
    {
        tabcontent1.SetActive(false);
        tabcontent2.SetActive(false);
        tabcontent3.SetActive(false);

        tabbutton1.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabbutton2.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
        tabbutton3.GetComponent<Button>().image.color = new Color32(212, 212, 212, 255);
    }

    public void ShowTab1()
    {
        HideAllTabs();
        tabcontent1.SetActive(true);
        tabbutton1.GetComponent<Button>().image.color = new Color32(78, 255, 124, 150);
    }

    public void ShowTab2()
    {
        HideAllTabs();
        tabcontent2.SetActive(true);
        tabbutton2.GetComponent<Button>().image.color = new Color32(255, 87, 83, 150);
    }

    public void ShowTab3()
    {
        HideAllTabs();
        tabcontent3.SetActive(true);
        tabbutton3.GetComponent<Button>().image.color = new Color32(78, 170, 255, 150);
    }
}