using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// 敵の基底クラス
/// </summary>
public class EnemyController : MonoBehaviour {
    protected readonly string m_enemyName;
    public virtual string enemyName {
        get { return m_enemyName; }
    }

    protected GameObject m_Fox;
    protected float m_foxKnockback;
    protected Vector3 m_foxPosition;
    protected NavMeshAgent m_enemyNavMeshAgent;
    protected bool m_knockbackFlag = true;
    protected float m_navStopFlameCount = 0;
    protected bool m_AnimetorIsAttack = false;
    [SerializeField] protected PalameterController m_palameterController;
    protected FoxController m_foxController;
    protected Rigidbody m_rigidbody;
    protected EnemyLifeController m_enemyLifeController;
    //protected Collider[] m_cubeThornColliders;
    protected float m_distansePow2 = 0f;
    protected int m_attackFlame = 0;
    protected bool m_attackFlag = false;
    //protected int m_attackFlameCount = 250;
    //protected int m_isAttackOffCount = 249;
    protected Animator m_enemyAnimator;


    //protected PalameterController m_palameterController;
    //static int m_defHP;
    //protected string m_enemyName;
    protected int m_enemyMaxHP;
    protected int m_enemyHP;
    protected int m_enemyAttackPower;
    protected float m_enemyAttackSpeed;
    protected float m_enemyMoveSpeed;
    protected float m_enemyRange;
    protected float m_enemyKnockback;

    protected bool m_warpBallHealFlag = true;

    public int enemyMaxHP {
        get { return m_enemyMaxHP; }
    }
    public int enemyCurrentHP {
        get { return m_enemyHP; }
    }
    public int enemyAttackPower {
        get { return m_enemyAttackPower; }
    }
    public float enemyAttackSpeed {
        get { return m_enemyAttackSpeed; }
    }
    public float enemyMoveSpeed {
        get { return m_enemyMoveSpeed; }
    }
    public float enemyAttackRange {
        get { return m_enemyRange; }
    }
    public float enemyKnockback {
        get { return m_enemyKnockback; }
    }

    public bool warpBallHealFlag {
        get { return m_warpBallHealFlag; }
        set { m_warpBallHealFlag = value; }
    }


    /// <summary>
    /// 敵の各種変数初期化処理
    /// タイミングは子クラスのStart()で行います。
    /// </summary>
    /// <param name="name"></param>
    protected void getStatas(string name) {
        PalameterController.EnemyStatas statas = m_palameterController.GetEnemyStatus(name);
        //m_enemyName = status[0];
        m_enemyHP = statas.HP;
        m_enemyMaxHP = statas.HP;
        m_enemyAttackSpeed = statas.AttackSpeed;
        m_enemyAttackPower = statas.AttackPower;
        m_enemyMoveSpeed = statas.MoveSpeed;
        m_enemyRange = statas.Range;
        m_enemyKnockback = statas.Knockback;
    }

    //protected EnemyController m_enemyController;






    //private void Awake() {
    //    m_enemyController = GetComponent<EnemyController>();
    ////    m_palameterController = GameObject.Find("StageSet/controller").gameObject.GetComponent<PalameterController>();
    ////    List<string> status = m_palameterController.GetEnemyStatus(m_enemyController.EnemyName);
    ////    m_enemyName = status[0];
    ////    m_enemyHP = int.Parse(status[1]);
    ////    m_enemyAttack = int.Parse(status[2]);
    ////    m_enemyAttackSpeed = float.Parse(status[3]);
    ////    m_enemyAttackRange = float.Parse(status[4]);
    ////    m_enemyKnockback = float.Parse(status[5]);

    ////    Debug.Log("getStatusSuccess" + m_enemyName + " HP: " + m_enemyHP + " attack:"+ m_enemyAttack + "AttackSpeed" + m_enemyAttackSpeed);


    //}

    /// <summary>
    /// 狐の攻撃が敵に当たった時の処理
    /// </summary>
    public virtual void HitFoxAttack(int damage) {
        m_enemyHP -= damage;
        Knockback();
        Debug.Log(m_enemyName + " HP: " + m_enemyHP);
        if (m_enemyHP <= 0) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 敵の攻撃が狐に当たった時の処理
    /// </summary>
    public virtual int HitEnemyAttack() {
        return m_enemyAttackPower;
    }


    public virtual void WhenDestroy() {

    }



    public virtual void Knockback() {

    }

    public virtual float EnemyProcessingLoad() {
        return 0f;
    }

    public virtual void GameEnd() {
    }
}