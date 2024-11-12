using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum VehicleType {Car, Boat};

public class Character : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private CameraManager attachedCamera;
    
    public GameObject characterPrefab;
    private int ID;

    public void SetConroller(GameController _controller){controller = _controller;}
    public void SetCamera(CameraManager _camera){_camera = attachedCamera;}

    public bool IsValidCharacter()
    {
        // Add any checks to see if the character is valid
        return true;
    }
}
