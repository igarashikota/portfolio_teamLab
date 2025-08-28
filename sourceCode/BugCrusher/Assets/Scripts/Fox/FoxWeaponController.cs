using System.Collections.Generic;
using UnityEngine;


public class FoxWeaponController : MonoBehaviour
{
    EnemyLifeController m_enemyLifeController;
    HashSet<int> m_enemyHitFlag = new HashSet<int>();

    private void Awake() {
        m_enemyLifeController = GameObject.Find("StageSet/controller").gameObject.GetComponent<EnemyLifeController>();
    }



    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "EnemyBody") {
            int InstanceID = other.gameObject.GetInstanceID();
            if (!m_enemyHitFlag.Contains(InstanceID)) {
                m_enemyLifeController.HitFoxsAttack(other);
                m_enemyHitFlag.Add(InstanceID);
            }
        }
    }

    public void enemyHitFlagReset() {
        m_enemyHitFlag.Clear();
    }
}

