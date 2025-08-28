using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class ConsoleController : ConsoleFormat {
    [SerializeField]
    TextMeshProUGUI m_console;

    [SerializeField]
    Canvas m_consoleCanvas;

    [SerializeField]
    List<Button> m_buttons = new List<Button>(4);

    [SerializeField]
    Button m_reRollButton;

    [SerializeField]
    Button m_nextStageButton;

    [SerializeField] 
    PalameterController m_palameterController;

    bool m_isSelectStage;
    int m_selectingStageIndex = 0;
    int m_random;
    List<Card> m_cardList = new List<Card>(4);
    List<StageEventBase> m_stages = new List<StageEventBase>(4);


    List<string> m_consoleMessages = new List<string>();


    List<string> m_attackSpped = new List<string>() { 
    };

    void Awake() {
        //m_console = GameObject.Find("StageSet/Canvas/Console").GetComponent<TextMeshProUGUI>();
        //m_palameterController = GameObject.Find("StageSet/controller").GetComponent<PalameterController>();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        m_isSelectStage = false;
        m_console.text = "";
        DisplayStar();
        DrawCards();
        DisplayCard();
        foreach (string message in m_consoleMessages) {
            m_console.text += message;
            //Debug.Log(message);
        }
    }

    // Update is called once per frame
    //void Update() {

    //}

    public void PressStageSelectButton() {
        if (!m_isSelectStage) {
            m_isSelectStage = true;
            DrawStages();
            DisplayStage();
            
            m_consoleMessages[32] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�b�@�@�@�@�����[���@�@�@�@�b�@�@�@�@�@�@�@�b�@�@�@�X�e�[�W�m��@�@�@�b\r\n";
            m_console.text = "";
            foreach (string message in m_consoleMessages) {
                m_console.text += message;
                //Debug.Log(message);
            }

            m_nextStageButton.interactable = false;
            m_reRollButton.interactable = true;
            if (!m_palameterController.enoughToken(m_palameterController.AReRollPrice)) {
                m_reRollButton.interactable = false;
            }
        }
        else {
            string stageName = m_stages[m_selectingStageIndex].stageName;
            m_palameterController.stageEventBase = m_stages[m_selectingStageIndex];
            SceneManager.LoadScene(stageName);
        }
    }






    private void DisplayStar() {
        m_consoleMessages = new List<string> (m_DefaultConsoleMessages);
        m_random = Random.Range(15,25);
        for (int i = 0; i < m_random; i++) {
            int randomY = Random.Range(2, 9);
            int randomX = Random.Range(5, 60);
            if(randomX > 31) {
                randomX += 31;
            }
            randomX += 9 - randomY;
            m_consoleMessages[randomY] = m_consoleMessages[randomY].Remove(randomX,1).Insert(randomX,"��");
        }
    }

    private void DrawCards() {
        m_cardList = m_palameterController.abnormalNhopAssortment();
    }

    private void DrawStages() {
        m_stages = m_palameterController.nextStageAssortment();
    }

    private void DisplayCard() {

        foreach (Button button in m_buttons) {
            button.interactable = true;
        }


        if (!m_palameterController.enoughToken(m_palameterController.AReRollPrice)) {
            m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ �g�[�N��������܂���" + "\r\n";
            m_reRollButton.interactable = false;
        }
        else {
            m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ cost�F" + m_palameterController.AReRollPrice.ToString("f2") + "\r\n";
            m_reRollButton.interactable = true;
        }


        for (int i=0; i<4; i++) {

            Card card = m_cardList[i];
            List<string> text = new List<string>(m_abnoemalShopText[card.name]);
            float value;
            float nextValue;
            Button button = m_buttons[i];

            switch (card.type) {
                case "doStatic":
                    Card referenceCard = m_palameterController.m_foxUsualCardDic[card.name.Replace("Static", "")];
                    value = referenceCard.value / referenceCard.defaultValue;
                    text[11] = text[11].Remove(2, 9).Insert(2, "����" + HanToZenNum(value.ToString("F2").PadLeft(6, '�@')) + "�{");
                    text[12] = text[12].Remove(3, 7).Insert(3, HanToZenNum(referenceCard.cardList[^1].ToString()) + "���@���@�O��");
                    text[13] = text[13].Remove(2, 10).Insert(2, "�R�X�g�F" + HanToZenNum(card.cost.ToString("F2").PadLeft(6, '�@')));
                    break;
                case "stack":
                    if (card.cardList[^1] == card.multiplierList.Count() - 1) { //�ő�܂Ŏ��؂��Ă���ꍇ
                        button.interactable = false;
                        value = card.value / card.defaultValue;
                        text[11] = text[11].Remove(4, 5).Insert(4, HanToZenNum(value.ToString("F2").PadLeft(4, '�@')) + "�{");
                        text[12] = text[12].Remove(3, 8).Insert(3, HanToZenNum(card.cardList[^1].ToString()) + "�^" + HanToZenNum(((card.multiplierList.Count() - 1).ToString())) + "���擾�ς�");
                        text[13] = text[13].Remove(1, 11).Insert(1, "����ȏ�擾�o���܂���");
                    }
                    else {
                        value = card.value / card.defaultValue;
                        nextValue = (card.value / card.multiplierList[card.cardList[^1]]) * card.multiplierList[card.cardList[^1] + 1] / card.defaultValue; text[11] = text[11].Remove(0, 13).Insert(0, HanToZenNum(value.ToString("F2").PadLeft(4, '�@')) + "�{�@���@" + HanToZenNum(nextValue.ToString("F2").PadLeft(4, '�@')) + "�{");
                        text[12] = text[12].Remove(3, 8).Insert(3, HanToZenNum(card.cardList[^1].ToString()) + "�^" + HanToZenNum((card.multiplierList.Count() - 1).ToString()) + "���擾�ς�");
                        text[13] = text[13].Remove(2, 10).Insert(2, "�R�X�g�F" + HanToZenNum(card.cost.ToString("F2").PadLeft(6, '�@')));
                    }                        
                    break;

            }

            if (m_palameterController.enoughToken(card.cost)) {
                //text[14] = text[14].Remove(3, 6).Insert(3, "�w���\�ł�");
            }
            else {
                button.interactable = false;
                text[14] = text[14].Remove(3, 6).Insert(3, "�w���s�ł�");
            }

            m_consoleMessages[11] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@[�@" + m_palameterController.token.ToString("f2") + " token�@]\r\n";


            for (int j=0; j<15; j++) {
                m_consoleMessages[13 + j] = m_consoleMessages[13+j].Remove(25+44*i, 14);
                m_consoleMessages[13+j] = m_consoleMessages[13+j].Insert(25+44*i, text[j]);
            }
        }
    }




    public void PressButton0() {
        PressButtons(0);
    }

    public void PressButton1() {
        PressButtons(1);
    }

    public void PressButton2() {
        PressButtons(2);
    }

    public void PressButton3() {
        PressButtons(3);
    }

    public void ReRoll() {
        if (!m_isSelectStage) {
            DrawCards();
            m_palameterController.UseToken(m_palameterController.AReRollPrice);


            DisplayCard();

            m_console.text = "";
            foreach (string message in m_consoleMessages) {
                m_console.text += message;
            }
        }
        else {
            DrawStages();
            m_palameterController.UseToken(m_palameterController.AReRollPrice);
            DisplayStage();

            m_console.text = "";
            foreach (string message in m_consoleMessages) {
                m_console.text += message;
            }
            m_nextStageButton.interactable = false;
            for (int i = 0; i < 4; i++) {
                m_buttons[i].interactable = true;
            }
        }

    }

    private void PressButtons(int buttonNum) {
        if (!m_isSelectStage) { //�X�e�[�W�Z���N�g���łȂ��ꍇ

            Card card = m_cardList[buttonNum];
            m_palameterController.upgradeCard(card.name);
            m_palameterController.UseToken(card.cost);

            Button button = m_buttons[buttonNum];
            button.interactable = false;
            List<string> text = new List<string>( m_abnoemalShopText[card.name] );
            Debug.Log(text[13]);
            float value;
            float nextValue;

            m_consoleMessages[11] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@[�@" + m_palameterController.token.ToString("f2") + " token�@]\r\n";

            switch (card.type) {
                case "doStatic":
                    Card referenceCard = m_palameterController.m_foxUsualCardDic[card.name.Replace("Static", "")];
                    value = referenceCard.value / referenceCard.defaultValue;
                    text[11] = text[11].Remove(2, 9).Insert(2, "����" + HanToZenNum(value.ToString("F2").PadLeft(6, '�@')) + "�{");

                    text[13] = text[13].Remove(3, 6).Insert(3, "�擾�ς݂ł�");
                    break;
                case "stack":
                    value = card.value / card.defaultValue;

                    if (card.cardList[^1] == card.multiplierList.Count() - 1) { //�ő�܂Ŏ��؂��Ă���ꍇ
                        button.interactable = false;
                        text[11] = text[11].Remove(4, 5).Insert(4, HanToZenNum(value.ToString("F2").PadLeft(4, '�@')) + "�{");
                        text[12] = text[12].Remove(3, 8).Insert(3, HanToZenNum(card.cardList[^1].ToString()) + "�^" + HanToZenNum(((card.multiplierList.Count() - 1).ToString())) + "���擾�ς�");
                        text[13] = text[13].Remove(1, 11).Insert(1, "����ȏ�擾�o���܂���");
                    }
                    else {
                        nextValue = (card.value / card.multiplierList[card.cardList[^1]]) * card.multiplierList[card.cardList[^1] + 1] / card.defaultValue;
                        text[11] = text[11].Remove(4, 5).Insert(4, HanToZenNum(value.ToString("F2").PadLeft(4, '�@')) + "�{");
                        text[12] = text[12].Remove(3, 8).Insert(3, HanToZenNum(card.cardList[^1].ToString()) + "�^" + HanToZenNum(((card.multiplierList.Count() - 1).ToString())) + "���擾�ς�");
                        text[13] = text[13].Remove(3, 6).Insert(3, "�擾�ς݂ł�");
                    }
                    break;
            }

            for (int j = 0; j < 15; j++) {
                m_consoleMessages[13 + j] = m_consoleMessages[13 + j].Remove(25 + 44 * buttonNum, 14);
                m_consoleMessages[13 + j] = m_consoleMessages[13 + j].Insert(25 + 44 * buttonNum, text[j]);
            }

            for (int i = 0; i < 4; i++) {
                if(!m_buttons[i].interactable) {
                    continue;
                }

                if (m_palameterController.enoughToken(m_cardList[i].cost)) {
                    //text[14] = text[14].Remove(3, 6).Insert(3, "�w���\�ł�");
                }
                else {
                    m_buttons[i].interactable = false;
                    m_consoleMessages[27] = m_consoleMessages[27].Remove(25 + 44 * i, 14);
                    m_consoleMessages[27] = m_consoleMessages[27].Insert(25 + 44 * i, "�@�@�@�@�w���s�ł��@�@�@�@");
                }
            }

            if (!m_palameterController.enoughToken(m_palameterController.AReRollPrice)) {
                m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ �g�[�N��������܂���" + "\r\n";
                m_reRollButton.interactable = false;
            }else {
                m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ cost�F" + m_palameterController.AReRollPrice.ToString("f2") + "\r\n";
                m_reRollButton.interactable = true;
            }

                m_console.text = "";
            foreach (string message in m_consoleMessages) {
                m_console.text += message;
                //Debug.Log(message);
            }
        }
        else { //�X�e�[�W�Z���N�g���̏ꍇ
            m_nextStageButton.interactable = true;
            m_selectingStageIndex = buttonNum;
            for (int i = 0; i < 4; i++) {
                if (i == buttonNum) {
                    m_buttons[i].interactable = false;
                }
                else {
                    m_buttons[i].interactable = true;
                }
            }
        }


        

    }


    void DisplayStage() {
        foreach (Button button in m_buttons) {
            button.interactable = true;
        }

        for (int i=0; i<4; i++) {
            StageEventBase stage = m_stages[i];


            List<string> text = new List<string>();
            for(int _=0; _<15; _++) {
                text.Add("");
            }


            text[1] = stage.stageDisplayName;
            text[5] = stage.rule;

            text[11] = stage.difficulty;
            text[13] = "�@�l���R�C���F" + HanToZenNum(stage.clearCoin.ToString("f2").PadLeft(6, '�@')) + "�@";




            for (int j = 0; j < 15; j++) {
                string str = text[j];
                if(str == "") {
                    str = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@";
                }
                m_consoleMessages[13 + j] = m_consoleMessages[13 + j].Remove(25 + 44 * i, 14);
                m_consoleMessages[13 + j] = m_consoleMessages[13 + j].Insert(25 + 44 * i, str);
            }

            m_consoleMessages[11] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@[�@" + m_palameterController.token.ToString("f2") + " token�@]\r\n";

            if (!m_palameterController.enoughToken(m_palameterController.AReRollPrice)) {
                m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ �g�[�N��������܂���" + "\r\n";
                m_reRollButton.interactable = false;
            }
            else {
                m_consoleMessages[34] = "�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@ cost�F" + m_palameterController.AReRollPrice.ToString("f2") + "\r\n";
                m_reRollButton.interactable = true;
            }

        }
        
    }




    /// <summary>
    /// ���p������S�p�����ɕϊ�����B
    /// </summary>
    /// <param name="s">�ϊ����镶����</param>
    /// <returns>�ϊ���̕�����</returns>
    public string HanToZenNum(string s) {
        return Regex.Replace(s, "[0-9]", p => ((char)(p.Value[0] - '0' + '�O')).ToString()).Replace(".","�D");
    }
}
