using GameInput;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    public MouseUser mouseUser;

    protected override void Awake()
    {

    }

    public bool IsMouseOverUI() 
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
