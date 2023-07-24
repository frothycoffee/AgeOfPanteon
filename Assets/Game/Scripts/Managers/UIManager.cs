using System.Collections;
using System.Collections.Generic;
using UI.Panels;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public InGamePanel inGamePanel;

        protected override void Awake()
        {

        }

        public void OpenProductionPanel()
        {
            TogglePanel(inGamePanel.productionPanel.gameObject);
        }

        public void OpenInformationPanel(EntityData entityData)
        {
            inGamePanel.informationPanel.gameObject.SetActive(true);
            inGamePanel.informationPanel.SetPanelProperties(entityData);
        }

        public void CloseInformationPanel()
        {
            inGamePanel.informationPanel.gameObject.SetActive(false);
        }

        private void TogglePanel(GameObject panel)
        {
            bool toggle = panel.activeSelf ? false : true;

            panel.SetActive(toggle);
        }
    }
}