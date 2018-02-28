using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataLogic : MonoBehaviour
{
    public Dropdown dropdown;
    public int slot;
    public GameData.GameState state;
    
    // Use this for initialization
    void Awake ()
    {      
        Language.Initialize();        
	}

    private void Start()
    {
        dropdown = GetComponentInChildren<Dropdown>();
        Language.SetLanguage(Language.Lang.enUS);
        state = GameData.LoadGame(slot);
    }

    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Language.SetLanguage(Language.Lang.esES);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Language.SetLanguage(Language.Lang.enUS);
        }*/

        if(Input.GetKeyDown(KeyCode.S))
        {
            GameData.gameState = state;
            GameData.SaveGame(slot);
        }
    }

    public void SetLanguage()
    {
        if(dropdown.value == 0)
        {
            Language.SetLanguage(Language.Lang.enUS);
        }
        else if(dropdown.value == 1)
        {
            Language.SetLanguage(Language.Lang.esES);
        }
    }
}
