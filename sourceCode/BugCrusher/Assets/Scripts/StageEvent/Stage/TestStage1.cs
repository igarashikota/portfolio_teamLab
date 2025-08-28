using UnityEngine;

public class TestStage1 : StageEventBase {
    //private void Awake() {
    //    m_enemyLifeController = GetComponent<EnemyLifeController>();
    //    m_probability = 10000;
    //}

    private void Start() {
        int enemyNum = (int)m_palameterController.MultiplyLevelMutiplier(5);
        m_enemyLifeController.spawnEnemy("cube", 0, enemyNum);
        //m_enemyLifeController.spawnEnemy("cube", 1, 10);
    }

    public override int GetProbability() {
        return 10000;
    }

    public override string stageName {
        get { return "TestStage1"; }
    }

    public override int clearCoin {
        get { return 500; }
    }

    public override string stageDisplayName {
        get { return "�@�@�@�e�X�g�X�e�[�W�P�@�@�@"; }
    }

    public override string difficulty {
        get { return "�@�@�@�@�@���ʁ@�@�@�@�@�@�@"; }
    }

    public override string rule {
        get { return "�@�@�@�@�@�Q�[�g�@�@�@�@�@�@"; }
    }

    public override void Clear() {
        m_palameterController.GetCoin(clearCoin);
    }

}
