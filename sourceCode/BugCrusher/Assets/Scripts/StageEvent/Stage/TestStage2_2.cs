using UnityEngine;

public class TestStage2_2 : StageEventBase {
    private void Awake() {
        m_enemyLifeController = GetComponent<EnemyLifeController>();
        m_probability = 10000;
        m_stageName = "TestStage2";
        m_clearCoin = 600;
        m_stageDisplayName = "　　　テストステージ２ー２　　　";
    }

    private void Start() {
        m_enemyLifeController.spawnEnemy("cube", 0, (int)m_palameterController.MultiplyLevelMutiplier(5f));
        m_enemyLifeController.spawnEnemy("cube", 1, (int)m_palameterController.MultiplyLevelMutiplier(5f));
    }
    
    
    

    public override int GetProbability() {
        return 10000;
    }

    public override string stageName {
        get { return "TestStage2_2"; }
    }

    public override int clearCoin {
        get { return 600; }
    }

    public override string stageDisplayName {
        get { return "　　テストステージ２ー２　　"; }
    }

    public override string difficulty {
        get { return "　　　　　簡単　　　　　　　"; }
    }

    public override string rule {
        get { return "　　　　　ゲート　　　　　　"; }
    }

    public override void Clear() {
        m_palameterController.GetCoin(clearCoin);
    }
}