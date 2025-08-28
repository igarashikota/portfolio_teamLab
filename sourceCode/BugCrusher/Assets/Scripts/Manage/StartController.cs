using UnityEngine;
using TMPro;

public class StartController : MonoBehaviour {
    FoxController m_foxController;
    //[SerializeField]
    TextMeshProUGUI m_Console;

    [SerializeField]
    GameObject m_mainUICanvas;

    string[] m_StartMessage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_foxController = GameObject.Find("狐").GetComponent<FoxController>();
        m_Console = gameObject.transform.Find("Console").GetComponent<TextMeshProUGUI>();
        m_foxController.enabled = false;
        m_StartMessage = new string[]{"\r\n",
"\r\n",
"■■■■■■　　　　　　　　　　　　　　　　　　　　　　　　　■■■■　　　　　　　　　　　　　　　　　　　　　　　　　　　　■■\r\n",
"　■　　　■■　　　　　　　　　　　　　　　　　　　　　　■■　　　■■　　　　　　　　　　　　　　　　　　　　　　　　　　　　■\r\n",
"　■　　　　■　　　　　　　　　　　　　　　　　　　　　　■　　　　　■　　　　　　　　　　　　　　　　　　　　　　　　　　　　■\r\n",
"　■　　　■■　　■■　　　■■　　　　■■■■■■　　■　　　　　　　　　■■　■■■　■■　　　■■　　　　■■■■　　　　■　■■■　　　　　　■■■　　　■■　■■■\r\n",
"　■■■■■　　　　■　　　　■　　　　■　　■■　　　■　　　　　　　　　　■■　　　　　■　　　　■　　　■　　　■■　　　■■　　　■　　　　■　　■■　　　■■\r\n",
"　■　　　■■　　　■　　　　■　　　■　　　　■　　　■　　　　　　　　　　■　　　　　　■　　　　■　　　■■　　　　　　　■　　　　■　　　■　　　　■　　　■\r\n",
"　■　　　　■　　　■　　　　■　　　■　　　　■　　　■　　　　　　　　　　■　　　　　　■　　　　■　　　　■■■■　　　　■　　　　■　　　■■■■■■　　　■\r\n",
"　■　　　　■　　　■　　　　■　　　■　　　　■　　　　■　　　　　　　　　■　　　　　　■　　　　■　　　　　　　　■　　　■　　　　■　　　■　　　　　　　　■\r\n",
"　■　　　■■　　　■　　　■■　　　　■　　■■　　　　■■　　　■■　　　■　　　　　　■　　　■■　　　■　　　　■　　　■　　　　■　　　　■　　　■　　　■\r\n",
"■■■■■■　　　　　■■■　■■　　　■■■■■　　　　　　■■■■　　　■■■　　　　　　■■■　■■　　■■■■■　　　■■■　　■■■　　　　■■■　　　■■■\r\n",
"　　　　　　　　　　　　　　　　　　　　　　　　■\r\n",
"　　　　　　　　　　　　　　　　　　　■■　　　■\r\n",
"　　　　　　　　　　　　　　　　　　　■■　　■\r\n",
"　　　　　　　　　　　　　　　　　　　　■■■\r\n",
"\r\n",
"\r\n",
"\r\n",
"\r\n",
"\r\n",
" 　　　　　　　＋──────────────＋\r\n",//　　　　　　　＋──────────────＋
" 　　　　　　　│　　　　　　　　　　　　　　│\r\n",//　　　　　　　│　　　　　　　　　　　　　　│
" 　　　　　　　│　　　　　スタート　　　　　│\r\n",//　　　　　　　│　　　チュートリアル　　　　│
"　 　　　　　　│　　　　　　　　　　　　　　│\r\n",//　　　　　　　│　　　　　　　　　　　　　　│
" 　　　　　　　＋──────────────＋\r\n",//　　　　　　　＋──────────────＋
"\r\n",
"\r\n",
"\r\n",
"\r\n",
"\r\n",
"PS C:\\Users\\PC_User>　" };

        m_Console.text = "";
        foreach (string text in m_StartMessage)
        {
            Debug.Log(text);
            m_Console.text += text;
        }
        //m_Console.text = "test";

        Debug.Log("StartController Start");
        Time.timeScale = 0f;
        m_mainUICanvas.SetActive(false);
    }

    public void DownStartButton() {
        Debug.Log("DownStartButton");
        m_mainUICanvas.SetActive(true);
        m_foxController.enabled = true;
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
