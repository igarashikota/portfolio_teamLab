using UnityEngine;
using UnityEngine.UI;

public class FoxController : MonoBehaviour {

    GameObject m_mainCamera;
    GameObject m_contoroller;
    [SerializeField] PalameterController m_palameterController;
    GameObject m_foxWeaponBorn;
    CapsuleCollider m_foxWeaponCollider;
    EnemyLifeController m_enemyLifeController;
    FoxWeaponController m_foxWeaponController;
    UIController m_UIController;
    SceneController m_sceneController;

    [SerializeField]
    GameObject m_foxWarpBall;

    [SerializeField]
    GameObject m_foxSpawnPoint;

    float m_foxWarpBallCoolTime = 0f;
    float m_foxWarpBallCoolTimeRemainingCount;
    bool m_foxWarpBallCoolTimeFlag = false;
    float m_foxWarpBallSuccessIntervalTime = 0.5f;
    Image m_foxWarpBallCooltimeImage;


    Vector3 m_foxPastPos = Vector3.zero;
    Vector3 m_foxCurrentPos = Vector3.zero;
    Vector3 m_deltaPos = Vector3.zero;

    bool m_foxOnStage;
    public bool foxOnStage {
        set { m_foxOnStage = value; }
    }

    int m_foxMaxHP = 100;
    public int foxMaxHP {
        set { m_foxMaxHP = value; }
    }

    int m_foxHP = 100;
    public int foxHP {
        get { return m_foxHP; }
        set { m_foxHP = value; }
    }

    float m_foxAttackSpeed;
    float m_foxTurnSpeed = 15f;
    bool m_foxIsWalking;
    float m_foxMoveSpeed;
    float m_foxAbsoluteVelocity;
    bool m_foxIsAttack;
    float m_foxRange;

    public float foxAttackSpeed {
        set {
            m_foxAttackSpeed = value;
            m_foxAnimator.SetFloat("attackSpeed", m_foxAttackSpeed / 30f);
        }
    }
    public float foxTurnSpeed {
        set { m_foxTurnSpeed = value; }
    }
    public float foxMoveSpeed {
        set { m_foxMoveSpeed = value; }
    }
    public float foxRange {
        set { m_foxRange = value; }
    }

    public float foxAbsoluteVelocity {
        get { return m_foxAbsoluteVelocity; }
    }

    float m_foxInvincibilityTimeRemainingCount = 0f;
    float m_foxInvincibilityTime = 0.5f;
    bool m_foxInvincibilityFlag = false;
    public float foxInvencibilityTime {
        get { return m_foxInvincibilityTime; }
        set { m_foxInvincibilityTime = value; }
    }


    int animationFlame;
    float cameraRotate = 0.2f;

    GameObject m_lookTarget;

    public Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    Animator m_foxAnimator;
    Rigidbody m_foxRigidbody;
    int m_attackFlame;
    int m_attackSpeed_1 = 1;

    //Vector3 m_foxSpawnPoint;
    public float m_horizontal;
    public float m_vertical;


    float m_cameraPhi;
    float m_cameraTheta;

    float m_cameraR = 2.5f;
    float m_cameraOffsetY = 2f;

    Vector3 m_desiredForward;
    Vector3 m_foxAttackForward;
    Vector3 m_cameraForward;

    //bool m_isMothAttackInvalid = false;

    void Awake() {
        m_mainCamera = GameObject.Find("StageSet/Main Camera");
        m_contoroller = GameObject.Find("StageSet/controller");
        //m_palameterController = m_contoroller.GetComponent<PalameterController>();
        m_foxWeaponBorn = transform.Find("アーマチュア/DEF_Root/DEF_HipRoot/DEF_Spine/DEF_Chest/DEF_UpperChest/DEF_Shoulder_R/DEF_UpperArm_R/DEF_LowerArm01_R/DEF_LowerArm02_R/DEF_LowerArm03_R/DEF_Hand_R/武器").gameObject;
        m_foxWeaponCollider = m_foxWeaponBorn.GetComponent<CapsuleCollider>();
        m_enemyLifeController = m_contoroller.gameObject.GetComponent<EnemyLifeController>();
        m_foxWeaponController = m_foxWeaponBorn.GetComponent<FoxWeaponController>();
        m_lookTarget = transform.Find("lookTarget").gameObject;

        m_foxAnimator = GetComponent<Animator>();
        m_foxRigidbody = GetComponent<Rigidbody>();
        m_UIController = GameObject.Find("StageSet/Canvas").GetComponent<UIController>();
        m_sceneController = m_contoroller.GetComponent<SceneController>();
        m_foxWarpBallCooltimeImage = GameObject.Find("StageSet/Canvas/WarpBallCooltime").GetComponent<Image>();

        //m_foxSpawnPoint = GameObject.FindWithTag("Stage").gameObject.transform.Find("FoxSpawnPoint").transform.position;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        m_foxMaxHP = (int)m_palameterController.foxMaxHP;
        m_foxAttackSpeed = m_palameterController.foxAttackSpeed;
        m_foxMoveSpeed = m_palameterController.foxMoveSpeed;
        m_foxRange = m_palameterController.foxRange;
        m_foxCurrentPos = transform.position;

        m_foxWarpBallCoolTime = 5f;

        m_attackFlame = 0;
        m_attackSpeed_1 = (int)(m_foxAttackSpeed - 1);
        m_cameraTheta = 1.22f;
        m_cameraPhi = 0f;

        m_desiredForward = transform.forward;

        Debug.Log("getStatusSuccess: fox" + " HP: " + m_foxMaxHP + " attack:" + m_foxAttackSpeed + " MoveSpeed:" + m_foxMoveSpeed);

        m_foxAnimator.SetFloat("attackPlaySpeed", m_foxAttackSpeed / 30f);

        m_foxHP = (int)m_palameterController.foxHP;

        m_foxMoveSpeed = m_palameterController.foxMoveSpeed;
        m_foxTurnSpeed = m_palameterController.foxMoveSpeed * 10f;
        m_foxRange = m_palameterController.foxRange;
        m_foxMaxHP = (int)m_palameterController.foxMaxHP;
        m_UIController.DisplayFoxHP(foxHP, m_foxMaxHP);

        m_foxInvincibilityTime = m_palameterController.foxInvincibilityTime;
        m_foxWarpBallSuccessIntervalTime = 1f;
    }


    void Update() {
        //カメラ系
        float mx = Input.GetAxis("Mouse X");//カーソルの横の移動量を取得
        float my = Input.GetAxis("Mouse Y");//カーソルの縦の移動量を取得

        m_cameraPhi -= mx * cameraRotate;
        m_cameraTheta += my * cameraRotate;

        if (m_cameraPhi < 0) {
            m_cameraPhi = 6.28318f;
        }
        else if (m_cameraPhi > 6.28318f) {
            m_cameraPhi = 0;
        }
        if (m_cameraTheta > 2.96705f) {
            m_cameraTheta = 2.96705f;
        }
        else if (m_cameraTheta < 0.17453f) {
            m_cameraTheta = 0.17453f;
        }

        float sinCameraTheta = Mathf.Sin(m_cameraTheta);

        float cameraX = m_cameraR * sinCameraTheta * Mathf.Sin(m_cameraPhi);
        float cameraY = m_cameraOffsetY + m_cameraR * Mathf.Cos(m_cameraTheta);
        float cameraZ = -m_cameraR * sinCameraTheta * Mathf.Cos(m_cameraPhi);

        Vector3 cameraPos = new Vector3(cameraX, cameraY, cameraZ);
        //Vector3 lookToTargetBornPos = -cameraPos;
        cameraPos += transform.position;
        //lookToTargetBornPos += transform.forward;


        m_mainCamera.transform.position = cameraPos;
        //lookToTargetBorn.transform.position = lookToTargetBornPos;

        m_mainCamera.transform.LookAt(m_lookTarget.transform);
        m_cameraForward = Vector3.Scale(m_mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;


        //ワープボール系
        if (m_foxWarpBallCoolTimeFlag) {
            m_foxWarpBallCoolTimeRemainingCount -= Time.deltaTime;
            float coolTime = 1 - (m_foxWarpBallCoolTimeRemainingCount / m_foxWarpBallCoolTime);
            m_foxWarpBallCooltimeImage.GetComponent<Image>().fillAmount = coolTime;
            if (m_foxWarpBallCoolTimeRemainingCount <= 0) {
                m_foxWarpBallCoolTimeFlag = false;
            }
        }
        else {
            if (Input.GetButton("Fire2")) {
                Debug.Log("Fire2");
                GameObject foxWarpBall = Instantiate(m_foxWarpBall, m_lookTarget.transform.position, Quaternion.identity);
                Vector3 foxWarpBallPower = m_mainCamera.transform.forward * 10 + m_deltaPos * 1f;
                Debug.Log("foxlinerVelosity:" + m_foxRigidbody.linearVelocity);
                foxWarpBallPower.y += 6f;
                foxWarpBall.GetComponent<Rigidbody>().AddForce(foxWarpBallPower, ForceMode.Impulse);
                //Debug.Log("foxWarpBallPower:" + foxWarpBallPower + " looktargetTransform:" + m_lookTarget.transform.position);

                m_foxWarpBallCoolTimeRemainingCount = m_foxWarpBallCoolTime;
                m_foxWarpBallCoolTimeFlag = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        if (m_foxInvincibilityFlag) {
            m_foxInvincibilityTimeRemainingCount -= Time.deltaTime;
            if (m_foxInvincibilityTimeRemainingCount <= 0) {
                m_foxInvincibilityFlag = false;
            }
        }



        //プレイヤー移動系

        m_Movement = m_cameraForward * m_vertical + m_mainCamera.transform.right * m_horizontal;
        m_Movement.Normalize();
        float m_deltaTime = Time.deltaTime;
        m_Movement *= m_foxMoveSpeed * m_deltaTime;


        m_foxCurrentPos = transform.position;
        m_deltaPos = (m_foxCurrentPos - m_foxPastPos) / m_deltaTime;
        m_foxPastPos = m_foxCurrentPos;
        if (!m_foxOnStage) { //空中
            m_Movement *= 4 / 5f;
            float dydt = m_deltaPos.y;
            m_foxAnimator.SetFloat("dydt", dydt);
        }
        else { //地上
            m_desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, m_foxTurnSpeed * m_deltaTime, 0f);
            if (Input.GetButton("Jump")) {
                m_foxAnimator.SetFloat("dydt", 5f);
                m_foxRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            }
        }



        if (m_attackFlame == 0) {
            if (Input.GetButton("Fire1")) {//ボタン押されたとき
                m_foxAttackForward = m_cameraForward;
                m_desiredForward = Vector3.RotateTowards(transform.forward, m_foxAttackForward, m_foxTurnSpeed * m_deltaTime, 0f);
                m_foxIsAttack = true;
                m_attackFlame = (int)m_foxAttackSpeed;
                m_foxAnimator.SetBool("isAttack", m_foxIsAttack);

                Vector3 localScale = Vector3.one * m_foxRange;
                m_foxWeaponBorn.transform.localScale = localScale;
                m_foxWeaponCollider.enabled = true;

            }
            else {  //通常時
                m_foxRigidbody.MovePosition(m_foxRigidbody.position + m_Movement);
                m_foxAbsoluteVelocity = m_Movement.magnitude;
                m_foxAnimator.SetFloat("speed", m_foxAbsoluteVelocity);
                m_foxAnimator.SetFloat("runPlaySpeed", m_foxAbsoluteVelocity / 0.04f);
                m_foxAnimator.SetFloat("walkPlaySpeed", m_foxAbsoluteVelocity / 0.016f);
                m_foxIsAttack = false;
                m_foxAnimator.SetBool("isAttack", m_foxIsAttack);
                m_foxWeaponBorn.transform.localScale = Vector3.one;
            }
        }
        else if (m_attackFlame == 1) {
            m_desiredForward = Vector3.RotateTowards(transform.forward, m_foxAttackForward, m_foxTurnSpeed * m_deltaTime, 0f);
            m_foxWeaponCollider.enabled = false;
            m_foxWeaponController.enemyHitFlagReset();
            m_attackFlame--;
        }
        else {
            m_desiredForward = Vector3.RotateTowards(transform.forward, m_foxAttackForward, m_foxTurnSpeed * m_deltaTime, 0f);
            m_foxIsAttack = false;
            m_attackFlame--;
            m_foxAnimator.SetBool("isAttack", m_foxIsAttack);
        }



        m_Rotation = Quaternion.LookRotation(m_desiredForward);
        m_foxRigidbody.MoveRotation(m_Rotation);



        //bool hasHorizontalInput = !Mathf.Approximately(m_horizontal, 0f);
        //bool hasVerticalInput = !Mathf.Approximately(m_vertical, 0f);
        //m_foxIsWalking = hasHorizontalInput || hasVerticalInput;

        //if(animationFlame == 59) {
        //    animationFlame = 0;
        //}
        //m_Animator.SetFloat("animationFlame", animationFlame/60f);
    }


    void OnTriggerEnter(Collider other) {
        GameObject enemy = other.gameObject;
        string enemyTag = enemy.tag;
        switch (enemyTag) {
            case "EnemyAttack":
                HitEnemysAttack(other);
                break;
            case "MothAttackInvalid":
                isMothAttackInvalidCount++;
                break;
            case "GoalGate":
                m_sceneController.ClearStage();
                break;
        }
    }

    void OnTriggerExit(Collider other) {
        GameObject otherGameObject = other.gameObject;
        string otherTag = otherGameObject.tag;
        if (otherTag == "MothAttackInvalid") {
            isMothAttackInvalidCount--;
            //Debug.Log("isMothAttackInvalidCount" + m_palameterController.isMothAttackInvalidCount);
        }
        else if (otherTag == "StageOut") {
            transform.position = m_foxSpawnPoint.transform.position;
            Debug.Log("StageOut");
        }
    }


    private void OnCollisionEnter(Collision other) {
        GameObject enemy = other.gameObject;
        if (enemy.tag == "EnemyBody") {
            if (!m_foxInvincibilityFlag) {
                m_enemyLifeController.HitEachOtherBody(other.collider);
            }
            else {
                m_enemyLifeController.destroyEnemy(other.collider.GetComponent<EnemyController>());
            }
        }
    }

    /// <summary>
    /// 敵の攻撃が狐に当たった時の処理
    /// </summary>
    public void HitEnemysAttack(Collider enemysAttackCollider) {
        EnemyController enemyController = enemysAttackCollider.transform.parent.parent.parent.gameObject.GetComponent<EnemyController>();
        int enemyAttack = (int)enemyController.enemyAttackPower;
        float enemyKnockback = (int)enemyController.enemyKnockback;


        if (!m_foxInvincibilityFlag) {
            m_foxHP -= enemyAttack;
            m_palameterController.foxHP = m_foxHP;
            m_UIController.DisplayFoxHP(m_foxHP);
            m_foxInvincibilityTimeRemainingCount = m_foxInvincibilityTime;
            m_foxInvincibilityFlag = true;
        }


        if (m_foxHP <= 0) {
            Debug.Log("GameOver");
            m_sceneController.GameOver();
        }
        //Debug.Log("NowHP:" + foxHP);

        Vector3 enemyPosition = enemyController.transform.position;

        Vector3 knockbackVector = new Vector3(transform.position.x - enemyPosition.x, 0, transform.position.z - enemyPosition.z);
        knockbackVector /= knockbackVector.magnitude;
        knockbackVector.y = 1f;
        knockbackVector *= enemyKnockback;
        m_foxRigidbody.AddForce(knockbackVector, ForceMode.Impulse);
    }

    /// <summary>
    /// 敵と狐どうしがぶつかった時の処理
    /// </summary>
    public void HitEachOtherBody(Collider enemyBodyCollider) {
        EnemyController enemyController = enemyBodyCollider.gameObject.GetComponent<EnemyController>();
        int enemyCurrentHP = (int)enemyController.enemyCurrentHP;
        float enemyKnockback = (int)enemyController.enemyKnockback;

        m_foxHP -= enemyCurrentHP;
        m_palameterController.foxHP = m_foxHP;
        m_UIController.DisplayFoxHP(m_foxHP);
        if (m_foxHP <= 0) {
            Debug.Log("GameOver");
            m_sceneController.GameOver();
        }
        Debug.Log("NowHP:" + foxHP);
        Vector3 enemyPosition = enemyBodyCollider.transform.position;
        Vector3 knockbackVector = new Vector3(transform.position.x - enemyPosition.x, 0, transform.position.z - enemyPosition.z);
        knockbackVector /= knockbackVector.magnitude;
        knockbackVector.y = 1f;
        knockbackVector *= enemyKnockback;
        m_foxRigidbody.AddForce(knockbackVector, ForceMode.Impulse);
    }

    int m_isMothAttackInvalidCount = 0;
    public int isMothAttackInvalidCount {
        get {
            return m_isMothAttackInvalidCount;

        }
        set {
            m_isMothAttackInvalidCount = value;
            Debug.Log("isMothAttackInvalidCount" + m_isMothAttackInvalidCount);
            if (isMothAttackInvalidCount == 0) {
                m_UIController.isMothAttackInvalid = false;
            }
            else if (isMothAttackInvalidCount == 1) {
                m_UIController.isMothAttackInvalid = true;
            }
        }
    }


    /// <summary>
    /// 狐のHP回復です。
    /// </summary>
    /// <param name="HealValue"></param>
    public void HealFox(int HealValue) {
        m_foxHP += HealValue;
        if (m_foxHP > m_foxMaxHP) {
            m_foxHP = m_foxMaxHP;
        }
        m_UIController.DisplayFoxHP(m_foxHP, m_foxMaxHP);
    }

    //public void SetFoxWarpBallSuccessInvisualTime() {
    //    m_foxInvincibilityTimeRemainingCount = m_foxWarpBallSuccessIntervalTime;
    //}


}

