using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine.PlayerLoop;

public class SceneController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    FoxController m_foxController;
    GameObject m_2DCamera;

    [SerializeField]
    PalameterController m_palametorController;

    [SerializeField] GameObject m_ClearCanvas;
    GameObject m_instantedClearCanvas;
    [SerializeField] GameObject m_ShopCanvas;
    GameObject m_instantedShopCanvas;
    [SerializeField] GameObject m_GameoverCanvas;
    GameObject m_instantedGameoverCanvas;
    [SerializeField] GameObject m_timeOverCanvas;
    GameObject m_instantedTimeOverCanvas;

    TextMeshProUGUI m_remainingTimeCanvas;
    string m_remainingTimeText;

    Volume m_GlovalVolume;
    DepthOfField m_depthOfField;

    List<Button> m_usualShopButtons = new List<Button>();
    List<TextMeshProUGUI> m_usualShopBottonsTexts = new List<TextMeshProUGUI>();
    List<TextMeshProUGUI> m_usualShopCosts = new List<TextMeshProUGUI>();
    TextMeshProUGUI m_coinText;
    //Button m_ReRallButton;
    int m_ReRollCost;

    StageEventBase m_stageEventBase;

    bool[] m_isShopButtonSelected = new bool[4];
    //string[] m_shopButtonTexts = new string[4];

    float m_remainingtime;
    int m_remainingSecond;

    private CancellationTokenSource _cancellationTokenSource;
    CancellationToken token;


    private void Awake() {
        m_foxController = GameObject.Find("��").GetComponent<FoxController>();
        m_2DCamera = GameObject.Find("StageSet/2DCamera");
        var startController = GameObject.Find("StageSet/StartConsoleCanvas");
        if (startController == null) {
            //Debug.Log("StartController is null");
            m_2DCamera.SetActive(false);
        }
        //m_ClearCanvas = GameObject.Find("StageSet/ClearCanvas");
        //m_ShopCanvas = GameObject.Find("StageSet/ShopCanvas");
        //m_GameoverCanvas = GameObject.Find("StageSet/GameoverCanvas");
        //m_ClearCanvas.SetActive(false);
        //m_ShopCanvas.SetActive(false);
        //m_GameoverCanvas.SetActive(false); 

        m_remainingTimeCanvas = GameObject.Find("StageSet/Canvas/RemainingTime").GetComponent<TextMeshProUGUI>();

        m_GlovalVolume = GameObject.Find("StageSet/Global Volume").GetComponent<Volume>();
        m_GlovalVolume.profile.TryGet(out m_depthOfField);

        m_remainingtime = m_palametorController.foxTimeLimit;
        m_remainingSecond = (int)m_remainingtime;
        m_remainingTimeCanvas.text = ConvertTime(m_remainingSecond);
        Debug.Log("m_remainingTime = " + m_remainingtime);

        m_ReRollCost = 50;

        m_stageEventBase = GetStageEventBase();
    }

    private void Start() {
        token = this.GetCancellationTokenOnDestroy();
    }

    private StageEventBase GetStageEventBase() {
        return m_palametorController.stageEventBase;
    }


    /// <summary>
    /// �X�e�[�W���J�n���܂��BPalametorController����Ăяo����܂��B
    /// </summary>
    /// <param name="timeLimit"></param>
    /// <param name="stageEventBase"></param>
    public void StartStage(StageEventBase stageEventBase) {
        m_stageEventBase = stageEventBase;
        GameObject StageCreator = GameObject.Find("StageCreator");
        if (StageCreator != null) {
            StageCreator.GetComponent<StageCreator>().CreateStage(stageEventBase);
        }

        //m_remainingtime = m_palametorController.foxTimeLimit;
        //m_remainingSecond = (int)m_remainingtime;
        //m_remainingTimeCanvas.text = ConvertTime(m_remainingSecond);
    }

    public void Update() {
        m_remainingtime -= Time.deltaTime;
        if (m_remainingSecond != (int)m_remainingtime) {
            m_remainingSecond = (int)m_remainingtime;
            m_remainingTimeCanvas.text = ConvertTime(m_remainingSecond);
            if (m_remainingSecond <= 0) {
                TimeOver();
            }
        }
    }


    /// <summary>
    /// �X�e�[�W�N���A�������s���܂��B
    /// </summary>
    public void ClearStage() {
        m_stageEventBase.Clear();
        ClearStageUni(token).Forget();
    }


    /// <summary>
    /// �X�e�[�W�N���A�������s���܂��B���̃N���X�����Ŏg�p���܂��B
    /// </summary>
    private async UniTask ClearStageUni(CancellationToken token) {


        DisplayClear();
        Debug.Log("DisplayClear");

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token, ignoreTimeScale: true);

        Debug.Log("Go Shop");
        List<Card> cards = m_palametorController.usualNhopAssortment();
        UnDisplayClear();
        DisplayShop();

        //Time.timeScale = 1f;
        //SceneManager.LoadScene("ConsoleShop");

    }


    /// <summary>
    /// �N���A��ʂ�\�����܂��B
    /// </summary>
    private void DisplayClear() {
        m_instantedClearCanvas = Instantiate(m_ClearCanvas);
        Debug.Log("Clearing stage!");

        Time.timeScale = 0f;
        m_foxController.enabled = false;
        //m_ClearCanvas.SetActive(true);
        m_2DCamera.SetActive(true);

        m_depthOfField.active = true;
        //m_ClearCanvas.enabled = true;
    }


    /// <summary>
    /// �N���A��ʂ��\���ɂ��܂��B
    /// </summary>
    private void UnDisplayClear() {
        Destroy(m_instantedClearCanvas);
        //m_ClearCanvas.SetActive(false);
    }

    List<Card> m_cards = new List<Card>();


    /// <summary>
    /// �V���b�v��ʂ�\�����܂��B
    /// </summary>
    private void DisplayShop() {
        m_instantedShopCanvas = Instantiate(m_ShopCanvas);
        m_cards = m_palametorController.usualNhopAssortment();
        m_usualShopButtons.Clear();
        m_usualShopBottonsTexts.Clear();
        m_usualShopCosts.Clear();
        for (int i = 0; i < 4; i++) {
            Debug.Log("Card" + (i).ToString() + "Cost");
            Button button = m_instantedShopCanvas.transform.Find("Card"+(i).ToString()).GetComponent<Button>();
            m_usualShopButtons.Add(button);
            TextMeshProUGUI buttonText = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            m_usualShopBottonsTexts.Add(buttonText);
            Debug.Log("Card" + (i).ToString() + "Cost");
            TextMeshProUGUI cost = m_instantedShopCanvas.transform.Find("Card"+(i).ToString()+"Cost").GetComponent<TextMeshProUGUI>();
            m_usualShopCosts.Add(cost);

            m_isShopButtonSelected[i] = false;
            SetButton(m_cards[i], button, buttonText, cost);
        }
        m_coinText = m_instantedShopCanvas.transform.Find("Coin").GetComponent<TextMeshProUGUI>();
        m_coinText.text = "Coin : " + m_palametorController.coin.ToString("00000");
    }


    public void ReRoll() {
        
        m_cards = m_palametorController.usualNhopAssortment();
        //m_palametorController.UseCoin(m_ReRollCost);
        //m_ReRollCost *= 1.5;
        for (int i = 0; i < 4; i++) {
            m_isShopButtonSelected[i] = false;
            SetButton(m_cards[i], m_usualShopButtons[i], m_usualShopBottonsTexts[i], m_usualShopCosts[i]);
        }
        m_coinText.text = "Coin : " + m_palametorController.coin.ToString("00000");
        //if (m_palametorController.enoughCoin(m_ReRollCost+50)){
            //button.interactable = false;
        //}
    }


    /// <summary>
    /// �Q�[���I�[�o�[�����ł��B
    /// </summary>
    public void GameOver() {
        Time.timeScale = 0f;
        m_foxController.enabled = false;
        m_2DCamera.SetActive(true);
        m_depthOfField.active = true;
        m_instantedGameoverCanvas = Instantiate(m_GameoverCanvas);
        //m_GameoverCanvas.SetActive(true);

        //SceneManager.LoadScene("Tytle");
    }


    private void TimeOver() {
        Time.timeScale = 0f;
        m_foxController.enabled = false;
        m_2DCamera.SetActive(true);
        m_depthOfField.active = true;
        m_instantedTimeOverCanvas = Instantiate(m_timeOverCanvas);
        //m_timeOverCanvas.SetActive(true);
    }


    /// <summary>
    /// �ʏ�X�̎��̃X�e�[�W�{�^���������������ł��B
    /// </summary>
    public void nextStageButtonDown() {
        SceneManager.LoadScene("ConsoleShop");
    }


    private void OnDestroy() {
        Time.timeScale = 1f;
    }


    public void SelectUsualShopButton0() {
        selectUsualShopButton(0);
        m_isShopButtonSelected[0] = true;
        UnSelectButtonReload(0);
    }
    public void SelectUsualShopButton1() {
        selectUsualShopButton(1);
        m_isShopButtonSelected[1] = true;
        UnSelectButtonReload(1);
    }
    public void SelectUsualShopButton2() {
        selectUsualShopButton(2);
        m_isShopButtonSelected[2] = true;
        UnSelectButtonReload(2);
    }
    public void SelectUsualShopButton3() {
        Debug.Log("UnSelectButtonReload");
        selectUsualShopButton(3);
        m_isShopButtonSelected[3] = true;
        UnSelectButtonReload(3);
    }


    private void selectUsualShopButton(int buttonNum) {
        Card card = m_cards[buttonNum];
        Button button = m_usualShopButtons[buttonNum];
        m_palametorController.upgradeCard(card.name);
        button.interactable = false;
        button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "<size=24><color=orange>" + card.name + "</size></color>\n" 
                                                                            + card.explanatoryText + "\n\n" 
                                                                            +  "�擾�ς�\n" 
                                                                            + (card.value / card.defaultValue).ToString() + "�{" + "\n" 
                                                                            + card.cardList[^1] + "/" + (card.multiplierList.Count() - 1).ToString() + "���擾�ς�";

        m_palametorController.UseCoin((int)card.cost);
        m_coinText.text = "Coin : " + m_palametorController.coin.ToString("00000");
        m_usualShopCosts[buttonNum].text = "<color=grey>" + m_usualShopCosts[buttonNum].text + "</color>";
    }



    private void UnSelectButtonReload(int buttonNum) {
        for (int i = 0; i < 4; i++) {
            if (i != buttonNum) {
                if (!m_isShopButtonSelected[i]) {
                    bool enoughCoin = m_palametorController.enoughCoin((int)m_cards[buttonNum].cost);
                    if (!enoughCoin) {
                        m_usualShopButtons[i].interactable = false;
                        m_usualShopBottonsTexts[i].text += "\n<color=red>�R�C��������܂���</color>";
                        m_usualShopCosts[i].text = "<color=grey>" + m_usualShopCosts[i].text + "</color>";
                    }
                }
            }
        }
    }

    public void selectStage0() { m_palametorController.selectStage(0); }
    public void selectStage1() { m_palametorController.selectStage(1); }
    public void selectStage2() { m_palametorController.selectStage(2); }
    public void selectStage3() { m_palametorController.selectStage(3); }




    /// <summary>
    /// �w��{�^���Ɏw��J�[�h�̏����Z�b�g���܂��B
    /// </summary>
    /// <param name="card"></param>
    /// <param name="button"></param>
    /// <param name="buttonText"></param>
    private void SetButton(Card card, Button button, TextMeshProUGUI buttonText, TextMeshProUGUI cost) {
        button.interactable = true;
        string textName = "<size=24><color=orange>" + card.name + "</size></color>";
        string textOverview = "";
        string textCost = card.cost.ToString();
        if (card.type == "stack") {

            textOverview += card.explanatoryText + "\n\n";

            if (card.cardList[^1] == card.multiplierList.Count() - 1) { //�ő�܂Ŏ��؂��Ă���ꍇ
                button.interactable = false;
                float value = card.value / card.defaultValue;
                textOverview += "\n" + value.ToString("F2") + "�{";
                textOverview += "\n" + "����ȏ�擾�o���܂���";
                textCost = "";
            }
            else {
                bool enoughCoin = m_palametorController.enoughCoin((int)card.cost);
                float value = card.value / card.defaultValue;
                float nextValue = (card.value/ card.multiplierList[card.cardList[^1]]) * card.multiplierList[card.cardList[^1] + 1] / card.defaultValue;

                if (enoughCoin) {
                    textOverview += value.ToString("F2") + "�{�@���@" + nextValue.ToString("F2") + "�{"
                                + "\n" + card.cardList[^1].ToString() + "/" + (card.multiplierList.Count() - 1).ToString() + "���擾�ς�";
                }
                else {
                    button.interactable = false;
                    textOverview += value.ToString("F2") + "�{�@���@" + nextValue.ToString("F2") + "�{"
                                + "\n" + card.cardList[^1] + "/" + (card.multiplierList.Count() - 1).ToString() + "���擾�ς�" + "\n<color=red>�R�C��������܂���</color>";
                    textCost = "<color=grey>" + textCost + "</color>";
                }

            }
        }

        string text = textName + "\n" + textOverview;
        buttonText.text = text;
        cost.text = textCost;
        return;
    }


    /// <summary>
    /// �c��b�����e�L�X�g�ɕϊ����܂��B
    /// </summary>
    /// <param name="remainingSecond"></param>
    /// <returns>##:## or ##:##:##</returns>
    private string ConvertTime(int remainingSecond) {
        int second = remainingSecond % 60;
        int minute = (remainingSecond / 60) % 60;
        int hour = (remainingSecond / 3600);
        string timeText;
        if (hour == 0) {
            timeText = string.Format("{1:D2}:{2:D2}", hour, minute, second);
        }
        else {
            timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }
        return timeText;
    }

}
