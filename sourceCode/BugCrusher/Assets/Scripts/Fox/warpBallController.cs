using UnityEngine;

public class warpBallController : MonoBehaviour
{
    FoxController m_foxController;
    [SerializeField]
    PalameterController m_palameterController;

    EnemyLifeController m_enemyLifeController;

    float m_foxWarpBallHealCoefficient;
    int m_foxWarpBallHealConstant;

    void Start()
    {
        m_foxController = GameObject.Find("ŒÏ").GetComponent<FoxController>();
        m_enemyLifeController = GameObject.Find("StageSet/controller").GetComponent<EnemyLifeController>();

        m_foxWarpBallHealCoefficient = m_palameterController.foxWarpBallHealCoefficient;
        m_foxWarpBallHealConstant = m_palameterController.foxWarpBallHealConstant;

    }

    private void OnTriggerEnter(Collider other) {
        string othertag = other.tag;
        switch (othertag) {
            case "EnemyBody":
                EnemyController enemyController = other.GetComponent<EnemyController>();
                if (enemyController.warpBallHealFlag) { 
                    int healFoxValue = (int)(m_foxWarpBallHealCoefficient * (enemyController.enemyMaxHP - enemyController.enemyCurrentHP)) + m_foxWarpBallHealConstant;
                    m_foxController.HealFox(healFoxValue);
                    Debug.Log("healFoxValue" + healFoxValue);
                    enemyController.warpBallHealFlag = false;
                }
                m_enemyLifeController.hitWarpBallOnEnemy(other);
                Destroy(gameObject);
                break;
            case "Stage":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        string othertag = other.tag;
        switch (othertag) {
            case "StageOut":
                Destroy(gameObject);
                break;
        }
    }


}
