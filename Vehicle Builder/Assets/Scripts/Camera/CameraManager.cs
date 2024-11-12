using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Character attachedCharacter;

    public bool TryToAttachToCharacter(Character character)
    {
        if (character.IsValidCharacter())
        {
            attachedCharacter = character;
            character.SetCamera(this);
            return true;
        }
        else return false;
    }

    ~CameraManager()
    {
        if(attachedCharacter != null)
        {
            attachedCharacter.SetConroller(null);
        }
    }
    
    // Where the code for your camera exists
}
