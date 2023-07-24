using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace UI.Panels
{
    public class ProductionPanelItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _itemImage;

        [SerializeField]
        private TextMeshProUGUI _itemNameText;

        private string _itemName;

        private string _itemInfo;

        private bool _isEntityPreviewable;

        public void SetItemProperties(EntityData entityData)
        {
            _itemName = entityData.entityName;
            _itemInfo = entityData.entityInfo;
            _isEntityPreviewable = entityData.isEntityPreviewable;

            _itemImage.sprite = entityData.productSprite;
            _itemNameText.text = _itemName;


            GetComponent<ToolTipTrigger>().header = _itemName;
            GetComponent<ToolTipTrigger>().content = _itemInfo;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isEntityPreviewable == true)
                EventManager.Brodcast(gameEventString: GameEvent.ShowEntityPreview, name: _itemName);
            else
                EventManager.Brodcast(gameEventString: GameEvent.SpawnEntity, name: _itemName);
        }
    }
}