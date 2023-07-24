using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;
using Managers;

public class SelectionManager : Singleton<SelectionManager>
{
    public EntityData onPreviewEntityData;
    public Entity selectedEntity;

    protected override void Awake()
    {
        onPreviewEntityData = null;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(gameEvent: GameEvent.LeftMouseButtonActionPerformed, OnLeftMouseButtonActionPerformed);
        EventManager.AddHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(gameEvent: GameEvent.LeftMouseButtonActionPerformed, OnLeftMouseButtonActionPerformed);
        EventManager.RemoveHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
    }

    private void OnLeftMouseButtonActionPerformed()
    {
        if (onPreviewEntityData == null && !InputManager.Instance.IsMouseOverUI())
        {
            SelectEntity();
        }
    }

    private void OnRightMouseButtonActionPerformed()
    {
        if (onPreviewEntityData == null 
            && selectedEntity != null 
            && selectedEntity.GetComponent<LiveUnitEntity>()
            &&!InputManager.Instance.IsMouseOverUI())
        {
            if (GridSystem.Instance.GetTile(InputManager.Instance.mouseUser.mouseInWorldPosition).isOccupied && selectedEntity.GetComponent<MilitaryEntity>())
                selectedEntity.GetComponent<MilitaryEntity>().AttackPhase(InputManager.Instance.mouseUser.mouseInWorldPosition);
            else
                selectedEntity.GetComponent<LiveUnitEntity>().Move(InputManager.Instance.mouseUser.mouseInWorldPosition);
        }
    }

    private void SelectEntity()
    {
        selectedEntity = GridSystem.Instance.GetTile(InputManager.Instance.mouseUser.mouseInWorldPosition).GetPlacedEntity();

        if (selectedEntity != null)
        {
            UIManager.Instance.OpenInformationPanel(selectedEntity.data);
        }
        else if (selectedEntity == null)
        {
            DeselectEntity();
        }
    }

    private void DeselectEntity()
    {
        UIManager.Instance.CloseInformationPanel();
    }
}