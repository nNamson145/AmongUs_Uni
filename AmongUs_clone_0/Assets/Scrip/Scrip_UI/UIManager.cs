using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
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

    private PlayerController localPlayer;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void RegisterLocalPlayer(PlayerController controller)
    {
        localPlayer = controller;
        Debug.Log("[CLIENT] Trying to register");
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
        if (localPlayer != null )
        {
            localPlayer.GetComponent<ImpostorController>().Kill();
        }
        else
        {
            Debug.LogWarning("Local player not yet registered.");
        }
    }

    public void OnClickSabotage()
    {

    }

    public void OnClickVent()
    {
        if (localPlayer != null )
        {
            localPlayer.GetComponent<ImpostorController>().UseVent();
            return;
        }
        Debug.LogError("Impostor can not use vent");
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
