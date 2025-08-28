using UnityEngine;
using System.Collections.Generic;



public class EnemyLifeController : MonoBehaviour {
    [SerializeField]
    //AssetReference cubePrefabAssetReference;
    GameObject cubePrefab;
    [SerializeField]
    //AssetReference mothPrefabAssetReference;
    GameObject mothPrefab;

    [SerializeField]
    GameObject m_stage;

    GameObject m_fox;
    GameObject m_controller;
    List<Vector3> m_spawnerList = new List<Vector3>();
    [SerializeField] PalameterController m_palameterController;
    FoxController m_foxController;

    int m_enemyNum = 0;
    Dictionary<string, int> m_enemyKindsNums = new Dictionary<string, int>() {
        {"cube", 0},
        {"moth", 0},
    };

    public Dictionary<string, int> EnemyNums{
        get { return m_enemyKindsNums; }
    }

    List<EnemyController> m_enemyControllers = new List<EnemyController>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        //cubePrefab = Addressables.LoadAssetAsync<GameObject>(cubePrefabAssetReference).WaitForCompletion();
        //mothPrefab = Addressables.LoadAssetAsync<GameObject>(mothPrefabAssetReference).WaitForCompletion();

        m_controller = GameObject.Find("StageSet/controller");
        //m_palameterController = m_controller.GetComponent<PalameterController>();
        m_fox = GameObject.Find("狐");
        m_foxController = m_fox.GetComponent<FoxController>();
        //m_stage = 
        int childCount = m_stage.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Transform child = m_stage.transform.GetChild(i);
            if (child.name == "spawner" + i.ToString()) {
                m_spawnerList.Add(child.position);
            }
        }
    }


    /// <summary>
    /// 敵をスポナーから生成します。
    /// </summary>
    /// <param name="enemyType">敵の名前</param>
    /// <param name="spawnerNum">スポナー番号</param>
    /// <param name="enemyNum">敵の数</param>
    public void spawnEnemy(string enemyType, int spawnerNum, int enemyNum) {
        GameObject enemy;
        Vector3 spawnPos = m_spawnerList[spawnerNum];
        m_enemyKindsNums["cube"] += enemyNum;
        switch (enemyType) {
            case "cube":
                enemy = cubePrefab;
                for (int i = 0; i < enemyNum; i++) {
                    Vector3 RandomPos = Random.insideUnitSphere * 3;
                    m_enemyControllers.Add(Instantiate(enemy, spawnPos + RandomPos, Quaternion.identity).GetComponent<EnemyController>());
                }
                break;
        }
    }


    /// <summary>
    /// 敵をスポナーから生成します。
    /// </summary>
    /// <param name="enemyType">敵の名前</param>
    /// <param name="spawnTransform">スポーン地点</param>
    /// <param name="enemyNum">敵の数</param>
    public void spawnEnemy(string enemyType, Transform spawnTransform, int enemyNum) {
        GameObject enemy;
        Vector3 spawnPos = spawnTransform.position;
        m_enemyKindsNums["cube"] += enemyNum;
        switch (enemyType) {
            case "cube":
                enemy = cubePrefab;
                for (int i = 0; i < enemyNum; i++) {
                    Vector3 RandomPos = Random.insideUnitSphere * 3;
                    m_enemyControllers.Add(Instantiate(enemy, spawnPos + RandomPos, Quaternion.identity).GetComponent<EnemyController>());
                }
                break;
            case "moth":
                enemy = mothPrefab;
                for (int i = 0; i < enemyNum; i++) {
                    Vector3 RandomPos = Random.insideUnitSphere * 3;
                    m_enemyControllers.Add(Instantiate(enemy, spawnPos + RandomPos, Quaternion.identity).GetComponent<EnemyController>());
                }
                break;
        }
    }

    public void destroyEnemy(EnemyController enemyController) {
        m_enemyControllers.Remove(enemyController);
        m_enemyKindsNums[enemyController.enemyName]--;
        enemyController.WhenDestroy();
        Destroy(enemyController.gameObject);
    }

    public void HitEnemysAttack(Collider enemyAttackCollider) {
        m_foxController.HitEnemysAttack(enemyAttackCollider);
    }


    public void HitFoxsAttack(Collider enemyCollider) {
        int damage = (int)m_palameterController.foxAttackPower;
        if (m_foxController.isMothAttackInvalidCount != 0) {
            damage = 0;
        }
        enemyCollider.GetComponent<EnemyController>().HitFoxAttack(damage);
    }


    public void HitEachOtherBody(Collider enemyBodyCollider) {
        m_foxController.HitEachOtherBody(enemyBodyCollider);
        //string enemyName = enemyBodyCollider.GetComponent<EnemyController>().enemyName;
        //switch (enemyName) {
        //    case "moth":
        //        m_foxController.isMothAttackInvalidCount--;
        //        break;
        //}
        Debug.Log("enemyBodyCollider" + enemyBodyCollider.gameObject.name + "destroyed");
        destroyEnemy(enemyBodyCollider.GetComponent<EnemyController>());
    }

    //public void hitWarpBallOnStage() {
    //    int randomNum = Random.Range(0, m_enemyControllers.Count-1);
    //    Transform randomEnemy = m_enemyControllers[randomNum].transform;
    //    Vector3 enemyPos = randomEnemy.transform.position;
    //    randomEnemy.transform.position = m_fox.transform.position;
    //    m_fox.transform.position = enemyPos;
    //}

    public void hitWarpBallOnEnemy(Collider enemyCollider) {
        Vector3 enemyPos = enemyCollider.transform.position;
        //enemyCollider.transform.position = m_fox.transform.position + Vector3.up * 0.2f;
        //m_fox.transform.position = enemyPos + Vector3.up * 0.2f;

        (enemyCollider.transform.position, m_fox.transform.position) = (m_fox.transform.position + Vector3.up * 0.2f, enemyCollider.transform.position + Vector3.up * 0.2f);
        enemyCollider.GetComponent<EnemyController>().Knockback();
    }


    // Update is called once per frame
    void Update() {
        //r/num++;
        //if(num%120 == 0) {
        //    Instantiate(enemy, spawner[0], Quaternion.identity);
        //}
    }

    public void GameEnd() {
        foreach (EnemyController enemy in m_enemyControllers) {
            enemy.GameEnd();
        }
    }
}
