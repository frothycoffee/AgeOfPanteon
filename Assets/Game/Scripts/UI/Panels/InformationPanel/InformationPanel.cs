using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

namespace UI.Panels
{
    public class InformationPanel : MonoBehaviour
    {
        [SerializeField]
        private Image _itemImage;

        [SerializeField]
        private TextMeshProUGUI _itemNameText;

        [SerializeField]
        private TextMeshProUGUI _itemInfoText;

        [SerializeField]
        private TextMeshProUGUI _itemSizeText;

        [SerializeField]
        private TextMeshProUGUI _itemHealthText;

        [SerializeField]
        private Image _healthBarImg;

        private float _maxHealth;

        [SerializeField]
        private TextMeshProUGUI _itemDamageText;

        [SerializeField]
        private ProductionPanel _manufacturingPanel;

        private void OnDisable()
        {
            _manufacturingPanel.gameObject.SetActive(false);
        }

        private void Update()
        {
            _itemHealthText.text = _maxHealth + "/" + SelectionManager.Instance.selectedEntity.currentHealth;
            _healthBarImg.fillAmount = SelectionManager.Instance.selectedEntity.currentHealth / _maxHealth;
        }

        public void SetPanelProperties(EntityData entityData)
        {
            _itemImage.sprite = entityData.infoSprite;
            _itemNameText.text = entityData.entityName;
            _itemInfoText.text = entityData.entityInfo;
            _itemSizeText.text = entityData.width + "x" + entityData.height;
            _itemHealthText.text = entityData.entityMaxHealth + "/" + SelectionManager.Instance.selectedEntity.currentHealth;
            _maxHealth = entityData.entityMaxHealth;

            if (SelectionManager.Instance.selectedEntity.GetComponent<MilitaryEntity>())
            {
                _itemDamageText.gameObject.SetActive(true);
                MilitaryEntityData milData = (MilitaryEntityData)entityData;
                _itemDamageText.text = milData.entityDamage.ToString();
            }
            else
                _itemDamageText.gameObject.SetActive(false);

            _itemImage.useSpriteMesh = true;

            if (entityData.isEntityManufacturer)
            {
                CloseManufacturingPanel();
                OpenManufacturingPanel(entityData);
            }
            else
                CloseManufacturingPanel();
        }

        private void OpenManufacturingPanel(EntityData entityData)
        {
            _manufacturingPanel.productionType = entityData.manufactureType;
            _manufacturingPanel.gameObject.SetActive(true);
            _manufacturingPanel.SetPanelItems();
        }

        private void CloseManufacturingPanel()
        {
            _manufacturingPanel.gameObject.SetActive(false);
        }
    }
}