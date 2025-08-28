using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Card {
    public string name;
    public string type;
    public float value;
    public bool isActive;
    public float defaultValue;
    public List<int> cardList;
    public float[] multiplierList;
    public int probability;
    public float defaultCost;
    public float cost;
    public string explanatoryText;
}


[CreateAssetMenu(fileName = "PalameterController", menuName = "ScriptableObjects/PalameterController")]
public class PalameterController : ScriptableObject, ISerializationCallbackReceiver {

    int m_level;

    [SerializeField]
    StageEventBase StartUpStageEventBase;

    StageEventBase m_StageEventBase;

    public StageEventBase stageEventBase {
        get { return m_StageEventBase; }
        set { m_StageEventBase = value; }
    }

    public int level {
        get { return m_level; }
        set { m_level = value; }
    }

    private void LevelUp() {
        m_level++;
        m_levelPalameterMultiplier = MathF.Pow(2, (level - 1) / 6f);
        foreach (string cardName in m_usualCardNameList) {
            Card card = m_foxUsualCardDic[cardName];
            card.cost = MultiplyLevelMutiplier(card.defaultCost);
        }
        foreach (string cardName in m_abnormalCardNameList) {
            Card card = m_foxAbnormalCardDic[cardName];
            card.cost = MultiplyLevelMutiplier(card.defaultCost);
        }
    }

    float m_levelPalameterMultiplier;


    public Card foxAttackSpeedCard {
        get { return m_foxUsualCardDic["AttackSpeed"]; }
    }
    public Card foxAttackPowerCard {
        get { return m_foxUsualCardDic["AttackPower"]; }
    }
    public Card foxMoveSpeedCard {
        get { return m_foxUsualCardDic["MoveSpeed"]; }
    }
    public Card foxRangeCard {
        get { return m_foxUsualCardDic["Range"]; }
    }
    public Card foxMaxHPCard {
        get { return m_foxUsualCardDic["MaxHP"]; }
    }
    public Card foxKnockbackCard {
        get { return m_foxUsualCardDic["KnockBack"]; }
    }
    public Card getCoinCard {
        get { return m_foxUsualCardDic["GetCoin"]; }
    }
    public Card timeLimit {
        get { return m_foxUsualCardDic["TimeLimit"]; }
    }

    public Card shopPriceCard {
        get { return m_foxUsualCardDic["ShopPrice"]; }
    }
    public Card reRollPriceCard {
        get { return m_foxUsualCardDic["ReRollPrice"]; }
    }
    public Card lackCard {
        get { return m_foxUsualCardDic["Lack"]; }
    }

    int m_foxHP;

    float m_foxInvincibilityTime = 0.5f;
    public float foxInvincibilityTime {
        get { return m_foxInvincibilityTime; }
        set { m_foxInvincibilityTime = value; }
    }

    public int UReRollPrice {
        get { return (int)(m_levelPalameterMultiplier * m_foxUsualCardDic["ReRollPrice"].value); }
    }

    public float AReRollPrice {
        get { return (float)(m_levelPalameterMultiplier * 0.5); }
    }


    static readonly string[] m_usualCardNameList = new string[] {
        "AttackSpeed",
        "AttackPower",
        "MoveSpeed",
        "Range",
        "MaxHP",
        "Knockback",
        "GetCoin",
        "TimeLimit",
        "ShopPrice",
        "ReRollPrice",
        "Lack",
    };
    static readonly string[] m_abnormalCardNameList = new string[]{
        "AttackSpeedStatic",
        "AttackPowerStatic",
        "MoveSpeedStatic",
        "RangeStatic",
        "MaxHPStatic",
        "KnockbackStatic",
        "GetCoinStatic",
        "ShopPriceStatic",
        "ReRollPriceStatic",
        "TimeLimitStatic",
        "GetToken",
    };
    static readonly string[] m_CardNameList = {
        "AttackSpeed",
        "AttackPower",
        "MoveSpeed",
        "Range",
        "MaxHP",
        "Knockback",
        "GetCoin",
        "TimeLimit",
        "ShopPrice",
        "ReRollPrice",
        "Lack",

        "AttackSpeedStatic",
        "AttackPowerStatic",
        "MoveSpeedStatic",
        "RangeStatic",
        "MaxHPStatic",
        "KnockbackStatic",
        "GetCoinStatic",
        "ShopPriceStatic",
        "ReRollPriceStatic",
        "TimeLimitStatic",
        "GetToken",
    };


    int m_sumUsualProbability;
    int m_sumAbnormalProbability;
    int m_stageEventProbability;



    Dictionary<string, float[]> m_multiplierListSarch = new Dictionary<string, float[]> {
        { "0.7f",  new float[] { 1f, 0.97f, 0.93f, 0.88f, 0.8f,  0.7f } },
        { "1.5f",  new float[] { 1f, 1.04f, 1.1f,  1.18f, 1.28f, 1.5f } },
        { "2f",    new float[] { 1f, 1.05f, 1.15f, 1.3f,  1.5f,  2f   } },
        { "3f",    new float[] { 1f, 1.2f,  1.45f, 1.75f, 2.3f,  3f   } },
    };

    public Dictionary<string, Card> m_foxUsualCardDic = new Dictionary<string, Card>();
    public Dictionary<string, Card> m_foxAbnormalCardDic = new Dictionary<string, Card>();



    //int m_isMothAttackInvalidCount = 0;
    //public int isMothAttackInvalidCount {
    //    get {
    //        return m_isMothAttackInvalidCount;

    //    }
    //    set {
    //        m_isMothAttackInvalidCount = value;
    //        if (isMothAttackInvalidCount == 0) {
    //            m_UIController.isMothAttackInvalid = false;
    //        }
    //        else if (isMothAttackInvalidCount == 1) {
    //            m_UIController.isMothAttackInvalid = true;
    //        }
    //    }
    //}

    public int foxHP {
        get { return m_foxHP; }
        set { m_foxHP = value; }
    }

    public int foxAttackPower {
        get { return (int)m_foxUsualCardDic["AttackPower"].value; }
    }
    public int foxAttackSpeed {
        get { return (int)m_foxUsualCardDic["AttackSpeed"].value; }
    }
    public float foxMoveSpeed {
        get { return m_foxUsualCardDic["MoveSpeed"].value; }
    }
    public float foxRange {
        get { return m_foxUsualCardDic["Range"].value; }
    }
    public float foxMaxHP {
        get { return m_foxUsualCardDic["MaxHP"].value; }
    }
    public float foxKnockback {
        get { return m_foxUsualCardDic["Knockback"].value; }
    }
    public float foxGetCoin {
        get { return m_foxUsualCardDic["GetCoin"].value; }
    }
    public float foxShopPrice {
        get { return m_foxUsualCardDic["ShopPrice"].value; }
    }
    public float foxReRollPrice {
        get { return m_foxUsualCardDic["ReRollPrice"].value; }
    }
    public float foxTimeLimit {
        get { return m_foxUsualCardDic["TimeLimit"].value; }
    }
    public float foxLack {
        get { return m_foxUsualCardDic["Lack"].value; }
    }


    public float foxWarpBallHealCoefficient {
        get { return 0.5f; }
    }
    public int foxWarpBallHealConstant {
        get { return 10; }
    }


    float m_token;

    public float token {
        get { return m_token; }
    }


    int m_coin;
    public int coin {
        get { return m_coin; }
    }

    [SerializeField]
    StageEventBase[] m_stageEventBaseList;

    //GameObject[] m_stageEventGameobjectList;



    //private void Awake() {
    //    //m_EnemyLifeController = GetComponent<EnemyLifeController>();
    //    Debug.Log("Awake")  ;
    //    updatePalameter();

    //    Debug.Log("nextStage" + nextStageAssortment()[0]);
    //}


    /// <summary>
    /// 敵のステータスを取得します。全てのパラメーターは、2^(level/6)*random(0.8, 1.2)倍されます。
    /// </summary>
    public EnemyStatas GetEnemyStatus(string enemyName) {
        EnemyStatas enemyStatas = new EnemyStatas();
        EnemyStatas defaultEnemyStatas = m_defaultEnemyStatas[enemyName];

        float randomNum = UnityEngine.Random.Range(0.8f, 1.2f);

        enemyStatas.name = defaultEnemyStatas.name;
        enemyStatas.HP = (int)(MultiplyLevelMutiplier(defaultEnemyStatas.HP) * randomNum);
        enemyStatas.AttackSpeed = (MultiplyLevelMutiplier(defaultEnemyStatas.AttackSpeed) * randomNum);
        enemyStatas.AttackPower = (int)(MultiplyLevelMutiplier(defaultEnemyStatas.AttackPower) * randomNum);
        enemyStatas.Range = (MultiplyLevelMutiplier(defaultEnemyStatas.Range) * randomNum);
        enemyStatas.Knockback = (MultiplyLevelMutiplier(defaultEnemyStatas.Knockback) * randomNum);

        return enemyStatas;
    }

    public struct EnemyStatas {
        public string name;
        public int HP;
        public float AttackSpeed;
        public int AttackPower;
        public float MoveSpeed;
        public float Range;
        public float Knockback;
    }

    Dictionary<string, EnemyStatas> m_defaultEnemyStatas = new Dictionary<string, EnemyStatas>() {
        {"cube", new EnemyStatas() { name="cube", HP=50, AttackSpeed=250, AttackPower=10, MoveSpeed=1, Range=1, Knockback=3 } },
        {"moth", new EnemyStatas() { name="moth", HP=0, AttackSpeed=30, AttackPower=10, MoveSpeed=1, Range=1, Knockback=3 } },
    };


    /// <summary>
    /// statusをlevelに応じて倍率をかけます。
    /// </summary>
    public float MultiplyLevelMutiplier(float status) {
        return status * m_levelPalameterMultiplier;
    }


    /// <summary>
    /// カードのレベルを上げます。
    /// </summary>
    public void upgradeCard(string CardName) {
        Card card = null;
        if (m_usualCardNameList.Contains(CardName)) {
            card = m_foxUsualCardDic[CardName];
        }
        else if (m_abnormalCardNameList.Contains(CardName)) {
            card = m_foxAbnormalCardDic[CardName];
        }
        else {
            Debug.LogError("upgradCard : CardName is not found.");
            return;
        }

        string cardType = card.type;
        if (cardType == "stack") {
            card.cardList[^1]++;
        }
        else if (cardType == "doStatic") {
            setCardStatic(CardName.Replace("Static", ""));
        }

        updatePalameter();
    }

    /// <summary>
    /// stack系カードのstatic化を行います。
    /// </summary>
    public void setCardStatic(string CardName) {
        if (m_usualCardNameList.Contains(CardName)) {
            m_foxUsualCardDic[CardName].cardList.Add(0);
        }
        else if (m_abnormalCardNameList.Contains(CardName)) {
            m_foxAbnormalCardDic[CardName].cardList.Add(0);
        }
        else {
            Debug.LogError("setCardStatic CardName is not found.");
        }
    }

    /// <summary>
    /// カードの情報を基に、カードのパラメーターを更新します。
    /// </summary>
    private void updatePalameter() {

        m_sumUsualProbability = 0;
        m_sumAbnormalProbability = 0;

        foreach (string cardName in m_usualCardNameList) {
            Card card = m_foxUsualCardDic[cardName];

            switch (card.type) {
                case "stack":
                    float value = card.defaultValue;
                    foreach (int cardLevel in card.cardList) {
                        value *= card.multiplierList[cardLevel];
                    }
                    m_sumUsualProbability += card.probability;
                    card.value = value;
                    break;
            }
        }

        foreach (string cardName in m_abnormalCardNameList) {
            Card card = m_foxAbnormalCardDic[cardName];
            m_sumAbnormalProbability += card.probability;

            switch (card.type) {
                case "isActive":
                    foreach (int cardLevel in card.cardList) {
                        
                    }
                    break;

                case "stack":
                    float value = card.defaultValue;
                    foreach (int cardLevel in card.cardList) {
                        value *= card.multiplierList[cardLevel];
                    }
                    card.value = value;
                    break;

                case "doStatic":
                    break;

                default:
                    Debug.LogError("updatePalameter : CardName is not found.");
                    break;
            }
        }

        m_stageEventProbability = 0;
        foreach (StageEventBase stageEvent in m_stageEventBaseList) {
            m_stageEventProbability += stageEvent.GetProbability();
        }
    }


    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() { //Awakeの前に呼ばれる。初期化する。


        m_foxUsualCardDic = new Dictionary<string, Card>() { { "Lack", new Card() { value = 1f } } }; //lack初回参照のため一時的に代入


        m_foxUsualCardDic = new Dictionary<string, Card> {
            {"AttackSpeed", new Card() { name="AttackSpeed", type="stack", value=0, defaultValue=30f,  cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText= "攻撃速度が上昇します。"} },
            {"AttackPower", new Card() { name="AttackPower", type="stack", value=0, defaultValue=30f,  cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText="攻撃力が上昇します。"} },
            {"MoveSpeed",   new Card() { name="MoveSpeed",   type="stack", value=0, defaultValue=2f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText="移動速度が上昇します。"} },
            {"Range",       new Card() { name="Range",       type="stack", value=0, defaultValue=2f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText="射程が上昇します。"} },
            {"MaxHP",       new Card() { name="MaxHP",       type="stack", value=0, defaultValue=100f, cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText="最大HPが上昇します。"} },
            {"Knockback",   new Card() { name="Knockback",   type="stack", value=0, defaultValue=3f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"],   defaultCost=200, probability=10000,
                                        explanatoryText="ノックバック力が上昇します。"} },
            {"GetCoin",     new Card() { name="GetCoin",     type="stack", value=0, defaultValue=1f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"], defaultCost=200, probability=5000,
                                        explanatoryText="コイン獲得枚数が増加します。"} },
            {"ShopPrice",   new Card() { name="ShopPrice",   type="stack", value=0, defaultValue=1f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["0.7f"], defaultCost=200, probability=10000,
                                        explanatoryText="ショップの価格が減少します。"} },
            {"ReRollPrice", new Card() { name="ReRollPrice", type="stack", value=0, defaultValue=1f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["0.7f"], defaultCost=200, probability=10000,
                                        explanatoryText="リロールの価格が減少します。"} },
            {"TimeLimit",   new Card() { name="TimeLimit",   type="stack", value=0, defaultValue=90f,  cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["1.5f"], defaultCost=200, probability=5000,
                                        explanatoryText="制限時間が増加します。"} },
            {"Lack",        new Card() { name="Lack",        type="stack", value=0, defaultValue=1f,   cardList=new List<int>(){0}, multiplierList=m_multiplierListSarch["2f"],   defaultCost=200, probability=(int)(10000/foxLack),
                                        explanatoryText="運が上昇します。"} }
        };

        m_foxAbnormalCardDic = new Dictionary<string, Card> {
            {"AttackSpeedStatic", new Card(){ name="AttackSpeedStatic", type="doStatic", defaultCost=1.5f, probability=10000} },
            {"AttackPowerStatic", new Card(){ name="AttackPowerStatic", type="doStatic", defaultCost=1.5f, probability=10000} },
            {"MoveSpeedStatic",   new Card(){ name="MoveSpeedStatic",   type="doStatic", defaultCost=1.5f, probability=10000} },
            {"RangeStatic",       new Card(){ name="RangeStatic",       type="doStatic", defaultCost=1.5f, probability=10000} },
            {"MaxHPStatic",       new Card(){ name="MaxHPStatic",       type="doStatic", defaultCost=1.5f, probability=10000} },
            {"KnockbackStatic",   new Card(){ name="KnockbackStatic",   type="doStatic", defaultCost=1.5f, probability=10000} },
            {"GetCoinStatic",     new Card(){ name="GetCoinStatic",     type="doStatic", defaultCost=1.5f, probability=10000} },
            {"ShopPriceStatic",   new Card(){ name="ShopPriceStatic",   type="doStatic", defaultCost=1.5f, probability=10000} },
            {"ReRollPriceStatic", new Card(){ name="ReRollPriceStatic", type="doStatic", defaultCost=1.5f, probability=10000} },
            {"TimeLimitStatic",   new Card(){ name="TimeLimitStatic",   type="doStatic", defaultCost=1.5f, probability=10000} },
            {"GetToken",    new Card() { name="GetToken", type="stack", value=0, defaultValue = 1f,   cardList = new List<int>(){0}, multiplierList = m_multiplierListSarch["1.5f"], defaultCost=1.5f, probability = 10000 } }
        };

        m_StageEventBase = StartUpStageEventBase;

        m_level = 0;
        LevelUp();
        m_coin = 10;


        //Debug.Log(m_stageEventGameobjectList.Length);
        ////m_stageEventBaseList = m_stageEventGameobjectList.Select(x => x.GetComponent<StageEventBase>()).ToArray();
        //foreach (GameObject stageEventGameobject in m_stageEventGameobjectList) {
        //    m_stageEventBaseList.Add(stageEventGameobject.GetComponent<StageEventBase>());
        //}
        //Debug.Log("m_stageEventBaseList:" + m_stageEventBaseList.Count);

        //m_stageEventGameobjectList = null;

        //updatePalameter();
        //m_foxHP = (int)m_foxUsualCardDic["MaxHP"].value;

        //m_token = 0;
        //m_coin = 0;

    }

    /// <summary>
    /// ノーマルショップの品揃えを取得します。
    /// </summary>
    /// <returns>List(Card) (4)</returns>
    public List<Card> usualNhopAssortment() {
        List<string> pickedCardList = new List<string>();

        for (int i = 0; i < 4; i++) {
            int random = UnityEngine.Random.Range(1, m_sumUsualProbability);
            string pickCard = "";
            foreach (string cardName in m_usualCardNameList) {
                random -= m_foxUsualCardDic[cardName].probability;
                if (random <= 0) {
                    pickCard = cardName;
                    break;
                }
            }

            if (pickedCardList.Contains(pickCard)) {
                i--;
                continue;
            }
            else {
                pickedCardList.Add(pickCard);
            }
        }

        List<Card> returnCardList = new List<Card>();
        foreach (string cardName in pickedCardList) {
            Card card = m_foxUsualCardDic[cardName];
            returnCardList.Add(card);
        }
        return returnCardList;
    }

    /// <summary>
    /// コンソールショップの品揃えを取得します。
    /// </summary>
    /// <returns>List(Card) (4)</returns>
    public List<Card> abnormalNhopAssortment() {
        List<string> pickedCardList = new List<string>();

        for (int i = 0; i < 4; i++) {
            int random = UnityEngine.Random.Range(1, m_sumAbnormalProbability);
            Debug.Log("random:" + random + " ProbabilitySum" + m_sumAbnormalProbability);
            string pickCard = "";
            foreach (string cardName in m_abnormalCardNameList) {
                random -= m_foxAbnormalCardDic[cardName].probability;
                if (random <= 0) {
                    pickCard = cardName;
                    break;
                }
            }

            if (pickedCardList.Contains(pickCard)) {
                i--;
                continue;
            }
            else {
                pickedCardList.Add(pickCard);
            }
        }

        List<Card> returnCardList = new List<Card>();
        foreach (string cardName in pickedCardList) {
            Card card = m_foxAbnormalCardDic[cardName];
            returnCardList.Add(card);
        }
        return returnCardList;
    }


    //void OnEnable() {
    //    List<string> temp = usualNhopAssortment();
    //    Debug.Log("num:" + temp.Count + temp[0] + " " + temp[1] + " " + temp[2] + " " + temp[3]);
    //    List<string> temp2 = abnormalNhopAssortment();
    //    Debug.Log("num:" + temp2.Count + temp2[0] + " " + temp2[1] + " " + temp2[2] + " " + temp2[3]);
    //}



    public void GetToken(float getToken) {
        m_token += getToken;
    }

    public void GetCoin(int getCoin) {
        m_coin += getCoin;
    }

    public void UseCoin(int useCoin) {
        m_coin -= useCoin;
    }

    public void UseToken(float useToken) {
        m_token -= useToken;
    }

    public bool enoughCoin(int useCoin) {
        if (m_coin >= useCoin) {
            return true;
        }
        else {
            return false;
        }
    }


    public bool enoughToken(float useToken) {
        if (m_token >= useToken) {
            return true;
        }
        else {
            return false;
        }
    }

    private void OnEnable() {

        Debug.Log(m_stageEventBaseList.Length);
        //m_stageEventBaseList = m_stageEventGameobjectList.Select(x => x.GetComponent<StageEventBase>()).ToArray();
        //m_stageEventBaseList = new List<StageEventBase>();
        //foreach (StageEventBase stageEventBase in m_stageEventBaseList) {
            //StageEventBase stageEventBase = stageEventGameobject.GetComponent<StageEventBase>();
            //stageEventBase.InputStatasWhenOnAfterDesirialize();
            //m_stageEventBaseList.Add(stageEventBase);
        //}

        //m_stageEventGameobjectList = null;

        updatePalameter();
        m_foxHP = (int)m_foxUsualCardDic["MaxHP"].value;

        m_token = 20f;
        m_coin = 500;

        Debug.Log("nextStage" + nextStageAssortment()[0].name);
        Debug.Log("stageEventProbability = " + m_stageEventProbability);
        Debug.Log("TimeLimit = " + foxTimeLimit);

    }



    List<StageEventBase> m_stageShopAssortmentList;

    /// <summary>
    /// 次のステージの品揃えを取得します。内部変数の代入も行います。
    /// </summary>
    /// <returns>List(StageEventBase) (4)</returns>
    public List<StageEventBase> getStageShopAssortment() {
        m_stageShopAssortmentList = nextStageAssortment();
        return m_stageShopAssortmentList;
    }

    /// <summary>
    /// 次のステージの品揃えを取得します。
    /// </summary>
    /// <returns>List(StageEventBase) (4)</returns>
    public List<StageEventBase> nextStageAssortment() {
        List<StageEventBase> pickedStageList = new List<StageEventBase>();

        for (int i = 0; i < 4; i++) {
            int random = UnityEngine.Random.Range(1, m_stageEventProbability);
            StageEventBase pickStage = null;
            foreach (StageEventBase stageEvent in m_stageEventBaseList) {
                random -= stageEvent.GetProbability();
                if (random <= 0) {
                    pickStage = stageEvent;
                    break;
                }
            }

            if (pickedStageList.Contains(pickStage)) {
                i--;
                continue;
            }
            else {
                pickedStageList.Add(pickStage);
            }
        }
        return pickedStageList;
    }


    /// <summary>
    /// コンソールショップ移動時に呼び出されます。
    /// </summary>
    /// <param name="buttonNum"></param>
    public void selectStage(int buttonNum) {
        StageEventBase stageEventBase = m_stageShopAssortmentList[buttonNum];
        SceneManager.LoadScene(stageEventBase.stageName);

    }

    public void Restart(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
        //OnAfterDeserialize();
        //SceneManager.LoadScene("FirstStage");
    }

}
