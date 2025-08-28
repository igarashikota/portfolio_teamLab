using UnityEngine;

public class FirstStage : StageEventBase {
    //private void Awake() {
    //    m_enemyLifeController = GetComponent<EnemyLifeController>();
    //    m_probability = 10000;
    //}

    private void Start() {
        int enemyNum = (int)m_palameterController.MultiplyLevelMutiplier(2);
        m_enemyLifeController.spawnEnemy("cube", 0, 1);
        m_enemyLifeController.spawnEnemy("cube", 1, 1);
    }

    private void Update() {
        if (m_enemyLifeController.EnemyNums["cube"] == 0){
            m_enemyLifeController.spawnEnemy("cube", 0, 1);
            m_enemyLifeController.spawnEnemy("cube", 1, 1);
        }
    }

    public override int GetProbability() {
        return 100;
    }

    public override string stageName {
        get { return "FirstStage"; }
    }

    public override int clearCoin {
        get { return 500; }
    }

    public override string stageDisplayName {
        get { return "　　ファーストステージ　　　"; }
    }

    public override string difficulty {
        get { return "　　　　データなし　　　　　"; }
    }

    public override string rule {
        get { return "　　　　　ゲート　　　　　　"; }
    }

    public override void Clear() {
        m_palameterController.GetCoin(clearCoin);
    }

}
