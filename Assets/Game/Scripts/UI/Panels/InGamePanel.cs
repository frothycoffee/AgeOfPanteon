using System.Collections;
using System.Collections.Generic;
using UI.Panels;
using UnityEngine;
using Managers;

public class InGamePanel : MonoBehaviour
{
    public GameObject productionPanelButton;
    
    public ProductionPanel productionPanel;
    public InformationPanel informationPanel;

    public void TogglePanel(GameObject panel)
    {
        bool toggle = panel.activeSelf ? false : true;

        panel.SetActive(toggle);
    }
}
