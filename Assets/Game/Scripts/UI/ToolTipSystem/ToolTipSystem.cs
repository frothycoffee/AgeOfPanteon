using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem instance;

    public ToolTip toolTip;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float x = mousePos.x / Screen.width;
        float y = mousePos.y / Screen.height;

        if (x <= y && x <= 1 - y) //left
            toolTip.rectTransform.pivot = new Vector2(-0.15f, y);
        else if (x >= y && x <= 1 - y) //bottom
            toolTip.rectTransform.pivot = new Vector2(x, -0.1f);
        else if (x >= y && x >= 1 - y) //right
            toolTip.rectTransform.pivot = new Vector2(1.1f, y);
        else if (x <= y && x >= 1 - y) //top
            toolTip.rectTransform.pivot = new Vector2(x, 1.3f);
        transform.position = mousePos;
    }

    public static void Show(string content, string header = "")
    {
        instance.toolTip.gameObject.SetActive(true);
        instance.toolTip.SetText(content, header);
    }

    public static void Hide()
    {
        instance.toolTip.gameObject.SetActive(false);
    }
}
