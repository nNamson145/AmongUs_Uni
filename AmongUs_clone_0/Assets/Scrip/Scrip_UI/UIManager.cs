using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum namebutton
    {
        ButtonKill,
        ButtonSabotage,
        ButtonVent,
        ButtonReport,
        ButtonUse
    }

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //IMPOSTOR
    public void OnClickKill()
    {

    }

    public void OnClickSabotage()
    {

    }

    public void OnClickVent()
    {

    }

    public void OnClickReport()
    {

    }

    

    public void SetButtonInteractable(namebutton stringName, bool interactable)
    {
        Button button = transform.Find(stringName.ToString()).GetComponent<Button>();
        if (button != null)
        { 
            button.interactable = interactable;
        }
        else
        {
            return;
        }
    }
}
