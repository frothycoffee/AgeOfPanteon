using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UI.Panels
{
    public class ScrollController : MonoBehaviour
    {
        public List<GameObject> itemPool;

        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private float _itemHeight;

        [SerializeField]
        private float _itemSpacing;

        [SerializeField]
        private float _topBorder = 200f;

        [SerializeField]
        private float _bottomBorder = -200f;

        private float _itemOffset;

        private void OnEnable()
        {
            ResetBorders();
            ResetPoolItemIndeces();
            _scrollRect.onValueChanged.AddListener(CheckBorders);
        }

        private void Start()
        {
            _itemHeight = _content.GetComponent<GridLayoutGroup>().cellSize.y;
            _itemSpacing = _content.GetComponent<GridLayoutGroup>().spacing.y;

            _itemOffset = _itemHeight + _itemSpacing;
        }

        private void OnDisable()
        {
            ResetBorders();
            _scrollRect.onValueChanged.RemoveListener(CheckBorders);
        }

        private void ShiftItemToTop()
        {
            itemPool[itemPool.Count - 2].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemPool[0].GetComponent<RectTransform>().anchoredPosition.x,
                                                                                                      itemPool[0].GetComponent<RectTransform>().anchoredPosition.y + _itemOffset);

            itemPool[itemPool.Count - 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemPool[1].GetComponent<RectTransform>().anchoredPosition.x,
                                                                                                      itemPool[1].GetComponent<RectTransform>().anchoredPosition.y + _itemOffset);

            itemPool.Insert(0, itemPool[itemPool.Count - 1]);
            itemPool.Insert(0, itemPool[itemPool.Count - 2]);
            itemPool.RemoveRange(itemPool.Count - 2, 2);
        }

        private void ShiftItemToBottom()
        {
            itemPool[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemPool[itemPool.Count - 1].GetComponent<RectTransform>().anchoredPosition.x,
                                                                                                      itemPool[itemPool.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - _itemOffset);

            itemPool[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemPool[itemPool.Count - 2].GetComponent<RectTransform>().anchoredPosition.x,
                                                                                                      itemPool[itemPool.Count - 2].GetComponent<RectTransform>().anchoredPosition.y - _itemOffset);

            itemPool.Insert(itemPool.Count, itemPool[0]);
            itemPool.Insert(itemPool.Count, itemPool[1]);
            itemPool.RemoveRange(0, 2);
        }

        private void ShiftItemToBottom(int columnLenght)
        {
            int k = columnLenght;

            for (int i = 0; i < columnLenght; i++)
            {

                itemPool[i].transform.position = new Vector3(itemPool[itemPool.Count - k].transform.position.x,
                                                             itemPool[itemPool.Count - k].transform.position.y - _itemOffset,
                                                             itemPool[itemPool.Count - k].transform.position.z);

                k -= 1;
            }

            itemPool.Insert(itemPool.Count, itemPool[0]);
            itemPool.Insert(itemPool.Count, itemPool[1]);
            itemPool.RemoveRange(0, columnLenght);
        }

        private void CheckBorders(Vector2 value)
        {
            if (_content.GetComponent<RectTransform>().anchoredPosition.y >= _topBorder)
            {
                _topBorder += _itemOffset;
                _bottomBorder += _itemOffset;

                ShiftItemToBottom();
            }
            else if (_content.GetComponent<RectTransform>().anchoredPosition.y <= _bottomBorder)
            {
                _topBorder -= _itemOffset;
                _bottomBorder -= _itemOffset;

                ShiftItemToTop();
            }
        }

        private void ResetBorders()
        {
            _topBorder = _itemOffset;
            _bottomBorder = -_itemOffset;
        }

        private void ResetPoolItemIndeces()
        {
            itemPool.Sort(CompareListByName);
        }

        private int CompareListByName(GameObject a, GameObject b)
        {
            return a.name.CompareTo(b.name);
        }
    }
}