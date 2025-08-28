using UnityEngine;

public class usualShopButtonDown : MonoBehaviour
{
    SceneController m_sceneController;

    private void Start() {
        m_sceneController = GameObject.Find("StageSet/controller").GetComponent<SceneController>();
    }

    public void SelectUsualShopButton0() {
        m_sceneController.SelectUsualShopButton0();
    }
    public void SelectUsualShopButton1() {
        m_sceneController.SelectUsualShopButton1();
    }
    public void SelectUsualShopButton2() {
        m_sceneController.SelectUsualShopButton2();
    }
    public void SelectUsualShopButton3() {
        m_sceneController.SelectUsualShopButton3();
    }

    public void ReRoll() {
        m_sceneController.ReRoll();
    }

    public void nextStage() {
        m_sceneController.nextStageButtonDown();
    }
}
