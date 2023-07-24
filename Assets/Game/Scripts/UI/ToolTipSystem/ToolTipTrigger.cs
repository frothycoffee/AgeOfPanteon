using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string header;
    public string content;
    public float delayTime = 1f;

    private bool _isPointerOn;

    public void OnPointerClick(PointerEventData eventData)
    {
        ToolTipSystem.Hide();
        _isPointerOn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOn = true;
        StartCoroutine(ShowWithDelayCoroutine(delayTime));
    }

    public void OnMouseEnter()
    {
        _isPointerOn = true;
        StartCoroutine(ShowWithDelayCoroutine(delayTime));
    }

    public void OnMouseExit()
    {
        _isPointerOn = false;
        StopCoroutine(ShowWithDelayCoroutine(delayTime));
        ToolTipSystem.Hide();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOn = false;
        StopCoroutine(ShowWithDelayCoroutine(delayTime));
        ToolTipSystem.Hide();
    }

    private IEnumerator ShowWithDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_isPointerOn)
        {
            ToolTipSystem.Show(content, header);
        }
    }
}
