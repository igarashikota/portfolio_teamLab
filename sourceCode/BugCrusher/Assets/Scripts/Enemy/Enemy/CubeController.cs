using UnityEngine;
using UnityEngine.AI;

public class CubeController : EnemyController {
    new readonly string m_enemyName = "cube";
    public override string enemyName {
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

    Collider[] m_cubeThornColliders;
    protected int m_attackFlameCount = 5;
    protected int m_isAttackOffCount = 4;



    //private void Awake() {




    //}


    private void Start() {
        m_knockbackFlag = true;
        m_navStopFlameCount = 0.2f;
        m_Fox = GameObject.Find("狐");
        m_foxPosition = m_Fox.transform.position;
        m_enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        //m_palameterController = GameObject.Find("StageSet/controller").gameObject.GetComponent<PalameterController>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_cubeThornColliders = transform.Find("アーマチュア/Root").GetComponentsInChildren<Collider>();
        m_enemyLifeController = GameObject.Find("StageSet/controller").gameObject.GetComponent<EnemyLifeController>();
        m_enemyAnimator = GetComponent<Animator>();
        foreach (Collider collider in m_cubeThornColliders) {
            collider.enabled = false;
        }
        m_enemyNavMeshAgent.enabled = false;

        string m_enemyName = "cube";
        getStatas(m_enemyName);

        Debug.Log("getStatusSuccess" + m_enemyName + " HP: " + m_enemyHP + " attack:" + m_enemyAttackPower + "AttackSpeed" + m_enemyAttackSpeed);
    }



    /// <summary>
    /// 狐の攻撃がcubeに当たった時の処理
    /// </summary>
    public override void HitFoxAttack(int damage) {
        m_enemyHP -= damage;
        Knockback();
        Debug.Log(m_enemyName + " HP: " + m_enemyHP);
        if (m_enemyHP <= 0) {
            m_enemyLifeController.destroyEnemy(this);
        }
    }

    /// <summary>
    /// cubeの攻撃が狐に当たった時の処理
    public override int HitEnemyAttack() {
        return m_enemyAttackPower;
    }

    public override void WhenDestroy() {

    }

    /// <summary>
    /// ノックバック処理
    /// </summary>
    public override void Knockback() {

        m_enemyNavMeshAgent.enabled = false;
        m_knockbackFlag = true;
        if (!m_attackFlag) {
            m_navStopFlameCount = 0.1f;
        }
        m_foxKnockback = m_palameterController.foxKnockback;
        m_foxPosition = m_Fox.transform.position;
        Vector3 knockbackVector = new Vector3(transform.position.x - m_foxPosition.x, 0, transform.position.z - m_foxPosition.z);
        knockbackVector /= knockbackVector.magnitude;
        knockbackVector.y = 1f;
        knockbackVector *= m_foxKnockback;
        knockbackVector.y += 2f;
        m_rigidbody.AddForce(knockbackVector, ForceMode.Impulse);
    }


    private void Update() {
        float deltaTime = Time.deltaTime;
        //Debug.Log("UpdateCalled");
        if (m_navStopFlameCount <= 0) { //通常　&　nav停止終了処理
            if (!m_attackFlag && !m_knockbackFlag) { //(攻撃中かつノックバック中)でない
                m_foxPosition = m_Fox.transform.position;
                m_enemyNavMeshAgent.SetDestination(m_foxPosition);
                m_distansePow2 = (m_foxPosition - transform.position).sqrMagnitude;

                if (m_distansePow2 < 1.5) { //攻撃開始
                    //m_enemyNavMeshAgent.isStopped = true;
                    //Debug.Log("NavStop");
                    foreach (Collider collider in m_cubeThornColliders) { collider.enabled = true; }
                    m_enemyAnimator.SetBool("isAttack", true);
                    m_navStopFlameCount = m_attackFlameCount;
                    m_attackFlag = true;
                    m_AnimetorIsAttack = true;
                    m_enemyNavMeshAgent.enabled = false;
                }
            }
            else if (m_knockbackFlag && !m_attackFlag) { //ノックバック終了処理
                if (m_rigidbody.linearVelocity.magnitude < 0.1f) { //ノックバック後、着地処理
                    m_enemyNavMeshAgent.enabled = true;
                    m_knockbackFlag = false;
                }
            }
            else { //攻撃終了処理
                foreach (Collider collider in m_cubeThornColliders) { collider.enabled = false; }
                m_attackFlag = false;
                m_enemyNavMeshAgent.enabled = true;
            }

        }
        else { //r/nav停止中
            m_navStopFlameCount -= deltaTime;

            if (m_AnimetorIsAttack) {
                m_AnimetorIsAttack = false;
                m_enemyAnimator.SetBool("isAttack", false);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        string othertag = other.tag;
        switch (othertag) {
            case "StageOut":
                m_enemyLifeController.destroyEnemy(this);
                break;
        }
    }

    public override float EnemyProcessingLoad() {
        return 1f;
    }

    public override void GameEnd() {
        this.enabled = false;
    }
}
