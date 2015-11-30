using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour {

    // メンバ変数
    [SerializeField]private Vector3 m_start;             // 始点
    [SerializeField]private List<Vector3> m_turnings;    // 中継点
    [SerializeField]private Vector3 m_end;               // 終点
    private LineRenderer            m_locusRenderer;     // 軌跡描画用
    private LineRenderer            m_turningRenderer;   // 中継点描画用
    
    [SerializeField]private float   m_finishedTime = 1f; // 所要時間

    private float m_timer = 0f;


    // 初期化処理
    public void Initialize() {
        // ラインレンダラの追加
        m_locusRenderer = this.transform.parent.gameObject.AddComponent<LineRenderer>();
        m_turningRenderer = this.gameObject.AddComponent<LineRenderer>();

        m_locusRenderer.material = Resources.Load("LineRenderer") as Material;
        m_turningRenderer.material = Resources.Load("LineRenderer") as Material;
    }


    // 更新処理
    void Update() {
        // 毎フレーム呼んでるのは途中でデータが変更された時のため
        SetLocusData();
    }


    // 軌跡の情報を設定する処理
    public void SetLocusData() {

        m_locusRenderer.SetColors(Color.red, Color.red);
        m_locusRenderer.SetWidth(0.1f, 0.1f);
        m_turningRenderer.SetColors(Color.green, Color.green);
        m_turningRenderer.SetWidth(0.1f, 0.1f);

        // 始点から終点までの座標をまとめる
        List<Vector3> pointList = new List<Vector3>();
        CollatePointData(pointList);

        // 中継点をつなぐラインレンダラにデータを挿入
        m_turningRenderer.SetVertexCount(pointList.Count);
        for( int i = 0; i < pointList.Count; ++i ) {
            m_turningRenderer.SetPosition(i, pointList[i]);
        }

        // 時間により通過する座標の算出
        List<Vector3> locus = new List<Vector3>();
        for (float time = 0f; time < m_finishedTime; time += Time.deltaTime) {
            locus.Add( CalculateNowPoint(pointList, time) );
        }

        // 座標データの格納
        m_locusRenderer.SetVertexCount( locus.Count );
        for ( int i = 0; i < locus.Count; ++i ) {
            m_locusRenderer.SetPosition(i, locus[i]);
        }
    }


    // 値の算出
    public Vector3 Calculate() {

        Vector3 result = new Vector3(0f, 0f, 0f);

        // 始点から終点までのデータを１つにまとめる
        List<Vector3> pointList = new List<Vector3>();
        CollatePointData(pointList);

        // 現在位置を取得する
        result = CalculateNowPoint(pointList, m_timer);

        m_timer = Mathf.Repeat(m_timer + Time.deltaTime, m_finishedTime);

        return result;
    }


    // 座標データをまとめる
    private void CollatePointData(List<Vector3> _list) {
        
        // 始点を挿入
        _list.Add(m_start);

        // 中継点を挿入
        if (m_turnings.Count != 0)
        {
            foreach (Vector3 m in m_turnings)
            {
                _list.Add(m);
            }
        }

        // 終点を挿入
        _list.Add(m_end);
    }



    // 再帰処理
    private Vector3 CalculateNowPoint(List<Vector3> _list, float _nowTime) {
        List<Vector3> pointList = new List<Vector3>();

        for (int i = 0; i < _list.Count - 1; ++i)
        {
            // 現在の座標～次の座標を移動する座標を算出し、格納
            pointList.Add(_list[i] + (_list[i + 1] - _list[i]) * _nowTime / m_finishedTime);
        }

        // 格納されたデータが１件ならそれが現在の位置
        if ( pointList.Count == 1) {
            return pointList[0];
        }
        else
        {
            // 同じ処理を繰り返す
            return CalculateNowPoint(pointList, _nowTime);
        }
    }

}
