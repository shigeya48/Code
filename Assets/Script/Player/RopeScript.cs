using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    // ホックの到達点
    public Vector2 arrivalPoint;

    // 射出スピード
    public float speed;

    // ノード間の距離
    public float distance;

    // プレイヤーに与える移動値
    public float addForcePowor;

    // ノードプレファブ
    public GameObject nodePrefab;

    // プレイヤー
    public GameObject player;

    // 最後のノード
    public GameObject lastNode;

    // LineRenderer
    public LineRenderer lr;

    // LineRendererの頂点数
    int vertexCount = 2;

    // ノードを管理するList
    public List<GameObject> Nodes = new List<GameObject>();

    // ↑のノードの座標
    List<Vector2> NodesPos = new List<Vector2>();

    // ホックが到達点にたどり着いた場合のフラグ
    bool isarrival = false;

    // ホックが何かに当たった場合のフラグ
    public bool hitFlg = false;

    // ホックの状態ステート
    public enum TentacleState
    {
        EXTEND, // 射出中
        HIT,    // 何かに当たった
        RESTORE // 回収
    }

    // ステートの変数
    public TentacleState tentacleState;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");

        lastNode = transform.gameObject;

        Nodes.Add(transform.gameObject);

        tentacleState = TentacleState.EXTEND;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (tentacleState)
        {
            case TentacleState.EXTEND:
                TentacleExtend();
                break;
            case TentacleState.HIT:
                TentacleHit();
                break;
            case TentacleState.RESTORE:
                TentacleRestore();
                break;
        }

        if (player.GetComponent<ThrowHook>().allRestore)
        {
            RestorePreparation();
            tentacleState = TentacleState.RESTORE;
        }

        RenderLine();
    }

    /// <summary>
    /// 触手を伸ばす処理
    /// </summary>
    void TentacleExtend()
    {
        transform.position = Vector2.MoveTowards(transform.position, arrivalPoint, speed * Time.deltaTime);

        if ((Vector2)transform.position != arrivalPoint)
        {
            if (Vector2.Distance((Vector2)player.transform.position, (Vector2)lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (!isarrival)
        {
            isarrival = true;

            while (Vector2.Distance((Vector2)player.transform.position, (Vector2)lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }


        if ((Vector2)transform.position == arrivalPoint)
        {
            if (hitFlg)
            {
                lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();

                player.GetComponent<ThrowHook>().hooks.Add(gameObject);

                player.GetComponent<ThrowHook>().HitAngleCheck();

                EffectManager.Instance.InfectionEffect(arrivalPoint);

                tentacleState = TentacleState.HIT;
            }
            else
            {
                RestorePreparation();
                tentacleState = TentacleState.RESTORE;
            }
            
        }
    }

    /// <summary>
    /// 触手が当たった場合の処理
    /// </summary>
    void TentacleHit()
    {
        if (player.GetComponent<ThrowHook>().ButtonDownFlg())
        {
            player.GetComponent<Rigidbody2D>().velocity += (Vector2)((transform.position - player.transform.position).normalized * addForcePowor * Time.deltaTime);
        }
    }

    public void RestorePreparation()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            NodesPos.Add((Vector2)Nodes[i].transform.position);
        }
        transform.DetachChildren();

        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].transform.parent = player.transform;
        }

    }

    /// <summary>
    /// 触手を引く処理
    /// </summary>
    void TentacleRestore()
    {
        if (Nodes.Count > 1)
        {
            Vector2 restoreDirection = ((Vector2)transform.position - (Vector2)Nodes[1].transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, restoreDirection);
        }
        else
        {
            Vector2 restoreDirection = ((Vector2)transform.position - (Vector2)player.transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, restoreDirection);
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            if (i < Nodes.Count - 1)
            {
                Nodes[i].transform.position = Vector2.MoveTowards(Nodes[i].transform.position, Nodes[i + 1].transform.position, speed * Time.deltaTime);
            }
            else
            {
                Nodes[i].transform.position = Vector2.MoveTowards(Nodes[i].transform.position, player.transform.position, speed * Time.deltaTime);

                if (Nodes[i].transform.position == player.transform.position)
                {
                    Destroy(Nodes[i]);
                    Nodes.RemoveAt(i);

                    vertexCount--;
                }
            }
        }
    }

    /// <summary>
    /// LineRendererを管理
    /// </summary>
    void RenderLine()
    {
        lr.positionCount = vertexCount;

        int i;
        for (i = 0; i< Nodes.Count; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
        }

        lr.SetPosition(i, player.transform.position);
    }

    /// <summary>
    /// ノードを生成する
    /// </summary>
    void CreateNode()
    {
        Vector2 nodePos = player.transform.position - lastNode.transform.position;
        nodePos.Normalize();
        nodePos *= distance;
        nodePos += (Vector2)lastNode.transform.position;

        GameObject node = (GameObject)Instantiate(nodePrefab, nodePos, Quaternion.identity);

        node.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = node.GetComponent<Rigidbody2D>();

        lastNode = node;

        Nodes.Add(lastNode);

        vertexCount++;
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">当たったCollision</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemySquare" || other.gameObject.tag == "EnemyTriangle")
        {
            EnemyHit(other);
        }
    }

    /// <summary>
    /// 敵に当たった場合
    /// </summary>
    /// <param name="enemy">敵のCollision</param>
    void EnemyHit(Collision2D enemy)
    {
        transform.parent = enemy.gameObject.transform;

        foreach (ContactPoint2D point in enemy.contacts)
        {
            arrivalPoint = point.point;
        }

        hitFlg = true;
    }

}
