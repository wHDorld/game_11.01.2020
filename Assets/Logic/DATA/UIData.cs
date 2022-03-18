using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIData
{
    public class UI_Manager
    {
        public GameObject ui;
        public void Initiate()
        {
            ui = GameObject.Find("MainCanvas");
            CloseUI();
        }

        public void OpenUI()
        {
            ui.SetActive(true);
        }
        public void CloseUI()
        {
            ui.SetActive(false);
        }
    }
}
