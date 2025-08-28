using UnityEngine;

public class SpwanBox : StageEventBase {

    Transform m_spawnerTransform;

    private void Awake() {
        m_enemyLifeController = GameObject.Find("StageSet/controller").GetComponent<EnemyLifeController>();
        m_spawnerTransform = transform.Find("Spawner0");
    }

    [SerializeField]
    string m_enemyType;
    [SerializeField]
    int m_spawnerNum;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            m_enemyLifeController.spawnEnemy(m_enemyType, m_spawnerTransform, m_spawnerNum);
            Destroy(gameObject);
        }
    }

}
