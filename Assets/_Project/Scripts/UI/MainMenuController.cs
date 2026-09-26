using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalDrop.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject playModePanel;
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Scene Names")]
        [SerializeField] private string battleRoyaleSceneName = "Ashvale_BR";
        [SerializeField] private string trainingSceneName = "Training";

        public void OpenPlayModes() => ShowOnly(playModePanel);
        public void OpenInventory() => ShowOnly(inventoryPanel);
        public void OpenSettings() => ShowOnly(settingsPanel);
        public void BackToMain() => ShowOnly(mainPanel);

        public void StartSolo() => SceneManager.LoadScene(battleRoyaleSceneName);
        public void StartTraining() => SceneManager.LoadScene(trainingSceneName);

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void ShowOnly(GameObject panelToShow)
        {
            mainPanel.SetActive(panelToShow == mainPanel);
            playModePanel.SetActive(panelToShow == playModePanel);
            inventoryPanel.SetActive(panelToShow == inventoryPanel);
            settingsPanel.SetActive(panelToShow == settingsPanel);
        }
    }
}
