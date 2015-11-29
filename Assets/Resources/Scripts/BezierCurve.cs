using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour {

    // メンバ変数
    [SerializeField]private Vector3 m_start;             // 始点
    [SerializeField]private List<Vector3> m_turnings;    // 中継点
    [SerializeField]private Vector3 m_end;               // 終点
    
    [SerializeField]private float   m_finishedTime = 1f; // 所要時間

    private float m_timer = 0f;


    // 値の算出
    public Vector3 Calculate() {

        Vector3 result = new Vector3(0f, 0f, 0f);

        List<Vector3> pointList = new List<Vector3>();
        // 始点を挿入
        pointList.Add(m_start);

        // 中継点を挿入
        if ( m_turnings.Count != 0 ) {
            foreach( Vector3 m in m_turnings ) {
                pointList.Add(m);
            }
        }

        // 終点を挿入
        pointList.Add(m_end);

        result = CalculateNowPoint(pointList);

        //else
        //{
        //    List<Vector3> pointList = new List<Vector3>();

        //    // 始点～中継点を移動する点を算出
        //    pointList.Add(m_start + (m_turnings[0] - m_start) * m_timer / m_finishedTime);

        //    for ( int i = 0; i < m_turnings.Count - 1; ++i ) {
        //        pointList.Add(m_turnings[i] + (m_turnings[i + 1] - m_turnings[i]) * m_timer / m_finishedTime);
        //    }

        //    // 中継点ｎ～終点を移動する点を算出
        //    pointList.Add(m_turnings[m_turnings.Count - 1] + 
        //        (m_end - m_turnings[m_turnings.Count - 1]) * m_timer / m_finishedTime);

        //    result = CalculateNowPoint(m_turnings);
        //}

        // 中継点～終点を移動する点m2を算出
        //Vector3 m2 = m_turnings + (m_end - m_turnings) * m_timer / m_finishedTime;

        // m1～m2を移動する点の軌跡こそがベジエ曲線！
        //result = m1 + (m2 - m1) * m_timer / m_finishedTime;

        m_timer = Mathf.Clamp(m_timer + Time.deltaTime, 0f, m_finishedTime);

        return result;
    }


    Vector3 CalculateNowPoint(List<Vector3> _list) {
        List<Vector3> pointList = new List<Vector3>();

        for (int i = 0; i < _list.Count - 1; ++i)
        {
            pointList.Add(_list[i] + (_list[i + 1] - _list[i]) * m_timer / m_finishedTime);
        }

        if ( pointList.Count == 1) {
            return pointList[0];
        }
        else
        {
            return CalculateNowPoint(pointList);
        }
    }

}
