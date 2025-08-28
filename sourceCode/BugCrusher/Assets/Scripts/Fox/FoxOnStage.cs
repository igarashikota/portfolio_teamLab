using UnityEngine;

public class FoxOnStage : MonoBehaviour
{
    GameObject m_fox;
    FoxController m_foxController;
    Animator m_foxAnimator;



    private void Awake() {
        m_fox = transform.parent.gameObject;
        m_foxController = m_fox.GetComponent<FoxController>();
        m_foxAnimator = m_fox.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Stage")) {
            //Debug.Log("Fox is on stage!");
            m_foxController.foxOnStage =true;
            m_foxAnimator.SetBool("isOnStage", true);
            m_foxAnimator.SetFloat("dydt", 0f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Stage")) {
            //Debug.Log("Fox left the stage!");
            m_foxController.foxOnStage = false;
            m_foxAnimator.SetBool("isOnStage", false);
        }
    }
}
