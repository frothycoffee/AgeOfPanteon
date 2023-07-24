using System.Collections.Generic;
using UnityEngine;
using Managers;
using UnityEngine.UI;

namespace UI.Panels
{
    public class ProductionPanel : MonoBehaviour
    {
        [Header("Panel Fields")]

        public EntityType productionType;

        public int minPanelItemCount;

        private int _productCount = 0;

        [SerializeField]
        private GameObject _productionPanelItemTemplatePrefab;

        [Header("Scroll View Fields")]

        [SerializeField]
        private Transform _scrollView;

        [SerializeField]
        private ScrollController _scrollController;

        private List<GameObject> _productionPanelItemPool = new List<GameObject>();

        private void Awake()
        {
            InitialisePanelItems();
        }

        private void OnEnable()
        {
            ActivateItems();
        }

        private void OnDisable()
        {
            DeactivateItems();
        }

        private void InitialisePanelItems()
        {
            for (int i = 0; i < minPanelItemCount; i++)
            {
                GameObject newProduct = Instantiate(_productionPanelItemTemplatePrefab, _scrollView.GetComponent<ScrollRect>().content);

                newProduct.name = newProduct.name + _productCount.ToString("00");

                _productCount++;

                newProduct.GetComponent<ProductionPanelItem>();

                _productionPanelItemPool.Add(newProduct);
                _scrollController.itemPool.Add(newProduct);
            }
        }

        int i = 0;

        public void SetPanelItems()
        {
            foreach (var entityData in EntitySpawnManager.Instance.entityDatas)
            {
                if (productionType == entityData.entityType)
                {
                    int index = i;

                    for (; index < i + 1; index++)
                    {
                        _productionPanelItemPool[index].GetComponent<ProductionPanelItem>().SetItemProperties(entityData);
                    }

                    i++;

                    if (i >= _productionPanelItemPool.Count)
                        break;
                }
            }

            if (i < _productionPanelItemPool.Count)
                SetPanelItems();
        }

        private void ActivateItems()
        {
            _scrollView.GetComponent<ScrollRect>().content.anchoredPosition = Vector3.zero;
            _scrollController.enabled = true;
            _scrollView.gameObject.SetActive(true);
        }

        private void DeactivateItems()
        {
            _scrollView.GetComponent<ScrollRect>().content.anchoredPosition = Vector3.zero;
            _scrollController.enabled = false;
            _scrollView.gameObject.SetActive(false);
            i = 0;
        }
    }
}