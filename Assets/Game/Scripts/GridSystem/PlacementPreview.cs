using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPreview : MonoBehaviour
{
    private SpriteRenderer _previewRenderer;
    private bool _isValid;
    private EntityData entityData;
    private GridSystem _gridSystem;

    private void OnEnable()
    {
        EventManager.AddHandler(gameEventString: GameEvent.ShowEntityPreview, actionString: ShowPreview);
        EventManager.AddHandler(gameEvent: GameEvent.ClearEntityPreview, action: ClearPreview);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(gameEventString: GameEvent.ShowEntityPreview, actionString: ShowPreview);
        EventManager.RemoveHandler(gameEvent: GameEvent.ClearEntityPreview, action: ClearPreview);
    }

    private void Start()
    {
        _previewRenderer = GetComponent<SpriteRenderer>();
        _gridSystem = GridSystem.Instance;
    }

    private void Update()
    {
        if (_previewRenderer.sprite != null)
        {
            Vector3 targetPosition = _gridSystem.GetMouseWorldSnappedPosition();
            Vector2Int origin = _gridSystem.GetGridPosition(targetPosition);

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

            _isValid = _gridSystem.IsGridAreaSuitable(entityData, origin);
            _previewRenderer.color = _isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }

    public void ShowPreview(string entityName)
    {
        entityData = EntitySpawnManager.Instance.GetEntityDataByName(entityName);

        SelectionManager.Instance.onPreviewEntityData = entityData;
        UIManager.Instance.CloseInformationPanel();

        _previewRenderer.enabled = true;
        _previewRenderer.transform.localScale = entityData.entityPrefab.GetComponent<Entity>().childSpriteRenderer.transform.localScale;
        _previewRenderer.sprite = entityData.previewSprite;
    }

    public void ClearPreview()
    {
        _previewRenderer.enabled = false;
        _previewRenderer.sprite = null;
    }
}
