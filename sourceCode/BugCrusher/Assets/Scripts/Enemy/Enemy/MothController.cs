using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ‰é‚ÌƒNƒ‰ƒX
/// </summary>
public class MothController : EnemyController {
    new readonly string m_enemyName = "moth";
    public override string enemyName{
        get { return m_enemyName; }
    }

    //GameObject m_Fox;
    //float m_foxKnockback;
    //Vector3 m_foxPosition;
    //r/navMeshAgent m_enemyNavMeshAgent;
    //bool m_knockbackFlag = false;
    //int m_navStopFlameCount = 0;
    //PalameterController m_palameterController;
    //Rigidbody m_rigidbody;
    //Collider[] m_cubeThornColliders;
    //public float m_distansePow2 = 0f;
    //int m_attackFlame = 0;
    //bool m_attackFlag = false;
    //int m_attackFlameCount = 250;
    //int m_isAttackOffCount = 249;
    //Animator m_enemyAnimator;

    private void Awake() {
        m_knockbackFlag = true;
        m_Fox = GameObject.Find("ŒÏ");
        m_foxPosition = m_Fox.transform.position;
        //m_enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        //m_palameterController = GameObject.Find("StageSet/controller").gameObject.GetComponent<PalameterController>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_enemyAnimator = GetComponent<Animator>();
        m_enemyLifeController = GameObject.Find("StageSet/controller").gameObject.GetComponent<EnemyLifeController>();
        m_foxController = m_Fox.GetComponent<FoxController>();

    }

    float m_getTokenValue = 0.5f;


    private void Start() {
        //î•ñæ“¾
        string m_enemyName = "moth";
        getStatas(m_enemyName);

        Debug.Log("getStatusSuccess: " + m_enemyName + " HP: " + m_enemyHP + " attack:" + m_enemyAttackPower + "AttackSpeed" + m_enemyAttackSpeed);
    }


    /// <summary>
    /// ŒÏ‚ÌUŒ‚‚ª‰é‚É“–‚½‚Á‚½‚Ìˆ—
    /// </summary>
    /// <param name="damage"></param>
    public override void HitFoxAttack(int damage) {
        //Debug.Log(m_palameterController.isMothAttackInvalidCount);
        m_enemyLifeController.destroyEnemy(this);
    }


    /// <summary>
    /// ˆ—‚Ì•‰‰×‚ğ•Ô‚µ‚Ü‚·B
    /// </summary>
    public override float EnemyProcessingLoad() {
        return 1f;
    }

    public override void WhenDestroy() {
        m_palameterController.GetToken(m_getTokenValue);
        m_foxController.isMothAttackInvalidCount--;
    }

    public override void GameEnd() {
        this.enabled = false;
    }
}
