using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI m_Console;

    TextMeshProUGUI m_HPdisplay;

    Slider m_HPbar;
    int m_foxMaxHP = 0;
    int m_foxHP = 0;

    public int foxMaxHP {
        set { m_foxMaxHP = value; }
    }
    public int foxHP {
        set { m_foxHP = value; }
    }

    [SerializeField] PalameterController m_PalameterController;


    List<string> m_ConsoleList = new List<string>();

    private void Awake() {
        m_HPbar = GameObject.Find("HPbar").GetComponent<Slider>();
        //m_Console = GameObject.Find("Console").GetComponent<TextMeshProUGUI>();
        //m_PalameterController = GameObject.Find("StageSet/controller").gameObject.GetComponent<PalameterController>();
        m_HPdisplay = m_HPbar.transform.Find("HPDisplay").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        //m_Console.text = "";
    }



    bool m_isMothAttackInvalid = false;


    public void DisplayFoxHP(int foxHP, int foxMaxHP) {
        m_foxMaxHP = foxMaxHP;
        m_foxHP = foxHP;

        m_HPbar.maxValue = m_foxMaxHP;
        m_HPbar.value = m_foxHP;
        string HPDisplay = "HP:" + m_foxHP.ToString() + "/" + m_foxMaxHP.ToString();
        m_HPdisplay.text = HPDisplay;
    }

    public void DisplayFoxHP(int foxHP) {
        m_foxHP = foxHP;
        m_HPbar.value = m_foxHP;
        string HPDisplay = "HP:" + m_foxHP.ToString() + "/" + m_foxMaxHP.ToString();
        m_HPdisplay.text = HPDisplay;
    }


    public bool isMothAttackInvalid {
        set {
            if (m_isMothAttackInvalid) {
                if (value) { //trueÅ®true

                }
                else { //trueÅ®false
                    m_ConsoleList.Remove("<color=\"red\">NullReferenceExceptionÅ@çUåÇÇ™ñ≥å¯Ç…Ç»ÇËÇ‹ÇµÇΩ</color>");
                    Debug.Log("NullReferenceExceptionÅ@çUåÇÇ™óLå¯Ç…Ç»ÇËÇ‹ÇµÇΩ" + m_ConsoleList.Count);
                    ConsoleOutput();
                    m_isMothAttackInvalid = value;
                }
            }
            else {
                if (value) { //falseÅ®true
                    m_ConsoleList.Add("<color=\"red\">NullReferenceExceptionÅ@çUåÇÇ™ñ≥å¯Ç…Ç»ÇËÇ‹ÇµÇΩ</color>");
                    Debug.Log("NullReferenceExceptionÅ@çUåÇÇ™ñ≥å¯Ç…Ç»ÇËÇ‹ÇµÇΩ" + m_ConsoleList.Count);
                    ConsoleOutput();
                    m_isMothAttackInvalid = value;
                }
                else {
                    //m_Console.text = "";
                }
            }
        }
    }

    public void ConsoleOutput() {
        int length = m_ConsoleList.Count;
        //Debug.Log("consoloutput" + length);
        string ConsoleLog = "";
        int ConsoleSize = 1;
        if (length != 0) {
            for (int i = 0; i < ConsoleSize; i++) {
                ConsoleLog += m_ConsoleList[(length - ConsoleSize) + i] + "\n";
            }
        }
        m_Console.text = ConsoleLog;
    }

}
