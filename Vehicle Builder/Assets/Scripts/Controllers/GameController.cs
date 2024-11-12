using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Character possesedCharacter;
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private bool Enabled = true;
    public void SetEnabled(bool value) => Enabled = value;

    public bool TryToPossesCharacter(Character character)
    {
        if (character.IsValidCharacter())
        {
            possesedCharacter = character;
            character.SetConroller(this);
            return true;
        }
        else return false;
    }

    ~GameController()
    {
        if(possesedCharacter != null)
        {
            possesedCharacter.SetConroller(null);
        }
    }

    // Add actions to be passed along to the possesed character here



    //  -------------------

    
}
