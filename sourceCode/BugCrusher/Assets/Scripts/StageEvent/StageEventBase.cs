using UnityEngine;

public class StageEventBase : MonoBehaviour {
    [SerializeField]
    protected EnemyLifeController m_enemyLifeController;

    //[SerializeField]
    //protected GameObject m_stage;

    [SerializeField]
    protected PalameterController m_palameterController;


    protected GameObject m_StageOut;

    //protected string m_stageType;
    //protected class StageOutPos {
    //    public Vector3 m_stageOutPos;
    //    public Vector3 m_stageOutRot;
    //    public Vector3 m_stageOutSize;
    //}
    //protected StageOutPos m_stageOutPos;


    protected int m_probability = 1000;
    public int probability {
        get { return m_probability; }
    }
    protected string m_stageName;
    public virtual string stageName {
        get { return m_stageName; }
    }

    protected int m_clearCoin;
    public virtual int clearCoin {
        get { return m_clearCoin; }
    }

    protected string m_stageDisplayName;
    public virtual string stageDisplayName {
        get { return m_stageDisplayName; }
    }

    public virtual string difficulty {
        get { return ""; }
    }

    public virtual string rule {
        get { return ""; }
    }


    public virtual void Clear() {

    }


    /// <summary>
    /// 確率を取得します。
    /// </summary>
    /// <returns>int このカードが選ばれる確率</returns>
    public virtual int GetProbability() {
        return 1;
    }

    //public virtual void CrearGetCoin() {

    //}
}
