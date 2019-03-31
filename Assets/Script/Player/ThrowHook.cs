using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHook : MonoBehaviour
{
    // ホックプレファブ
    public GameObject hookPre;

    // ホック
    GameObject hook;

    // ホックの長さ
    public float tentacleRange;

    // ホックを回収する角度
    float angle = 20;

    // タイマー
    float timer;
    // タイムインターバル
    const float timeInterval = 0.2f;

    // クリックが押されているか判断するフラグ
    bool buttonDownFlg = false;

    // 攻撃中か判断
    bool attackFlg = false;

    // インプットを受け付けなくするフラグ
    public bool inputCancel = false;

    // 自分が死んだかどうか判断するフラグ
    bool isDeath = false;

    // ひもを全回収するフラグ
    public bool allRestore = false;

    // でているホックを管理する
    public List<GameObject> hooks = new List<GameObject>();

    // Wallレイヤー
    public LayerMask wall;

    // プレイヤーのマテリアル
    public Material playerColor;

    // プレイヤーのアニメーション
    Animator anim;

    // Rigidbody2d
    Rigidbody2D rd2;

    // プレイヤーの最大HP
    const float maxPlayerHP = 100;
    // プレイヤーの現在のHP
    public float playerHp;

    // ひものサウンドを管理するタイマー
    float soundTimer = 1;
    // ↑のタイムインターバル
    float soundTimeInterval = 1f;

    // リスタートした際の復活する座標を保存
    Vector2 restartPos;
    // 死んだ回数
    int deathCounter = 0;

    // ポーズ画面のCanvas
    public GameObject pauseCanvas;

    // ポーズしているかどうか判断する
    bool pauseFlg = false;

    // Use this for initialization
    void Start()
    {
        // カウンターの初期化
        deathCounter = 0;

        // Rigidbody2d
        rd2 = GetComponent<Rigidbody2D>();
        // Animation
        anim = transform.GetChild(0).GetComponent<Animator>();

        // マテリアルの初期化
        playerColor.EnableKeyword("_EMISSION");
        playerColor.SetColor("_EmissionColor", new Color(1, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの操作をキャンセルする
        if (inputCancel) return;

        // タイマーの加算
        timer += Time.deltaTime;
        soundTimer += Time.deltaTime;

        // 左マウスボタンインプット(ポーズ状態じゃなければ)
        if (Input.GetMouseButton(0) && !pauseFlg)
        {
            // ボタンが押されている
            buttonDownFlg = true;

            // 移動時の音を鳴らす
            if (soundTimer > soundTimeInterval)
            {
                soundTimer = 0;
                SoundManager.Instance.Tentacle_Play();
            }

            // 一定時間経過している場合、ホックを射出する
            if (timer > timeInterval)
            {
                timer = 0;
                Vector2 arrivalPoint;

                Vector2 tentacleDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                tentacleDirection.Normalize();

                RaycastHit2D hit = Physics2D.Linecast(transform.position, (Vector2)transform.position + tentacleDirection * tentacleRange, wall);

                hook = Instantiate(hookPre, transform.position, Quaternion.FromToRotation(Vector3.up, tentacleDirection));
               
                if (hit)
                {
                    hook.GetComponent<RopeScript>().hitFlg = true;
                    arrivalPoint = hit.point;
                }
                else
                {
                    hook.GetComponent<RopeScript>().hitFlg = false;
                    arrivalPoint = (Vector2)transform.position + tentacleDirection * tentacleRange;
                }

                hook.GetComponent<RopeScript>().arrivalPoint = arrivalPoint;
            }
        }
        else
        {
            // クリックが押されていない場合
            buttonDownFlg = false;
        }

        // 右マウスボタンインプット
        if (Input.GetMouseButtonDown(1))
        {
            if (!buttonDownFlg)
            {
                RestoreAll();
            }
        }

        // エスケープキーインプット(ポーズ)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseFlg)
            {
                PauseBackGame();
            }
            else if (!pauseFlg)
            {
                SoundManager.Instance.PauseButton_Play();
                pauseFlg = true;
                pauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    /// <summary>
    /// ポーズからゲームに戻る
    /// </summary>
    public void PauseBackGame()
    {
        SoundManager.Instance.PauseButton_Play();
        pauseFlg = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// ボタンが押されているか判断するプロパティ
    /// </summary>
    /// <returns></returns>
    public bool ButtonDownFlg()
    {
        return buttonDownFlg;
    }

    /// <summary>
    /// ホックを回収するかどうか角度を判断
    /// </summary>
    public void HitAngleCheck()
    {
        for (int i = 0; i < hooks.Count; i++)
        {
            if (hooks[i] != null)
            {
                if (Vector2.Angle(((Vector2)hooks[i].transform.position - (Vector2)transform.position).normalized,
              ((Vector2)hooks[hooks.Count - 1].transform.position - (Vector2)transform.position).normalized) > angle
              || hooks.Count > 5)
                {
                    hooks[i].GetComponent<RopeScript>().RestorePreparation();
                    hooks[i].GetComponent<RopeScript>().tentacleState = RopeScript.TentacleState.RESTORE;
                    hooks.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// ホックを全回収
    /// </summary>
    public void RestoreAll()
    {
        allRestore = true;

        hooks.Clear();

        Invoke("ResutoreReset", 0.2f);
    }

    /// <summary>
    /// 全回収完了時呼ぶ
    /// </summary>
    void ResutoreReset()
    {
        allRestore = false;
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="col"> 当たったCollision </param>
    void OnCollisionEnter2D(Collision2D col)
    {
        // 死んでいた場合キャンセル
        if (isDeath) return;

        // 敵に当たった場合
        if ((col.gameObject.tag == "EnemySquare" || col.gameObject.tag == "EnemyTriangle") && !attackFlg)
        {
            StartCoroutine(attackMove(col.gameObject));
        }
        // コアに当たった場合
        if (col.gameObject.tag == "Core")
        {
            StartCoroutine(CoreInfection(col));
        }
        // メインコアに当たった場合
        if (col.gameObject.tag == "MainCore")
        {
            StartCoroutine(MainCoreInfection(col));
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <param name="enemy"> 攻撃対象の敵 </param>
    /// <returns></returns>
    IEnumerator attackMove(GameObject enemy)
    {
        inputCancel = true;
        buttonDownFlg = false;

        attackFlg = true;

        if (enemy.tag == "EnemySquare")
        {
            enemy.GetComponent<EnemySquare>().DeathSquarePreparation();
        }
        else if (enemy.tag == "EnemyTriangle")
        {
            enemy.GetComponent<EnemyTriangle>().DeathTrianglePreparation();
        }

        anim.SetTrigger("Attack");

        RestoreAll();

        rd2.simulated = false;

        while (transform.position != enemy.transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, 20 * Time.deltaTime);
            yield return null;
        }

        anim.SetTrigger("Eat");

        yield return new WaitForSeconds(0.35f);

        Time.timeScale = 0;

        int c = 0;

        while (c < 20)
        {
            c++;
            yield return null;
        }

        c = 0;
        Time.timeScale = 1;

        SoundManager.Instance.Eat_Play();

        EffectManager.Instance.EatEffect((Vector2)transform.position);

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().Shake();

        if (enemy.tag == "EnemySquare")
        {
            enemy.GetComponent<EnemySquare>().DeathSquare();
        }
        else if (enemy.tag == "EnemyTriangle")
        {
            enemy.GetComponent<EnemyTriangle>().DeathTriangle();
        }

        PlayerCure();

        yield return new WaitForSeconds(0.15f);

        inputCancel = false;

        rd2.simulated = true;

        attackFlg = false;
    }

    /// <summary>
    /// ダメージ処理(瞬間)
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void PlayerDamage(float damage)
    {

        if (playerHp > 0)
        {
            playerHp -= damage;
            if (playerHp <= 0)
            {
                inputCancel = true;
                StartCoroutine(IsDeath());
                playerHp = 0;
            }
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().DamageCamera();
            SoundManager.Instance.Damage_Play();
        }
    }

    /// <summary>
    /// ダメージ処理(継続)
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void PlayerDamageWhile(float damage)
    {
        if (playerHp > 0)
        {
            playerHp -= damage * Time.deltaTime;
            if (playerHp <= 0)
            {
                inputCancel = true;
                StartCoroutine(IsDeath());
                playerHp = 0;
            }
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().DamageCamera();
            SoundManager.Instance.WhileDamage_Play();
        }
    }

    /// <summary>
    /// プレイヤーの回復
    /// </summary>
    void PlayerCure()
    {
        if (playerHp < maxPlayerHP)
        {
            playerHp += 5;
            if (playerHp > maxPlayerHP)
            {
                playerHp = maxPlayerHP;
            }
        }
    }

    /// <summary>
    /// 現在のHPを返す
    /// </summary>
    /// <returns>現在のHP</returns>
    public float PlayerHP()
    {
        return playerHp;
    }

    /// <summary>
    /// コアに当たった場合の処理
    /// </summary>
    /// <param name="col">当たったコア</param>
    /// <returns></returns>
    IEnumerator CoreInfection(Collision2D col)
    {
        col.gameObject.GetComponent<CoreScript>().Infection();

        Vector2 circlePos = (Vector2)col.gameObject.transform.position + new Vector2(0, -1.4f);

        restartPos = (Vector2)col.transform.position;

        RestoreAll();

        rd2.simulated = false;

        inputCancel = true;

        while ((Vector2)transform.position != circlePos)
        {
            transform.position = Vector2.MoveTowards(transform.position, circlePos, 5 * Time.deltaTime);
            yield return null;
        }

        anim.SetBool("Infection", true);
    }

    /// <summary>
    /// メインコアに当たった場合の処理
    /// </summary>
    /// <param name="col">当たったメインコア</param>
    /// <returns></returns>
    IEnumerator MainCoreInfection(Collision2D col)
    {
        col.gameObject.GetComponent<CrearScript>().InfectionCore();

        Vector2 circlePos = (Vector2)col.gameObject.transform.position + new Vector2(0, 1.9f);

        RestoreAll();

        rd2.velocity = Vector2.zero;
        rd2.simulated = false;

        inputCancel = true;

        while ((Vector2)transform.position != circlePos)
        {
            transform.position = Vector2.MoveTowards(transform.position, circlePos, 5 * Time.deltaTime);
            yield return null;
        }

        anim.SetBool("Infection", true);

    }

    /// <summary>
    /// 死んだ場合の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator IsDeath()
    {
        inputCancel = true;

        isDeath = true;

        SoundManager.Instance.Damage_Play();
        Time.timeScale = 0;

        int c = 0;

        while (c < 30)
        {
            c++;
            yield return null;
        }

        c = 0;
        Time.timeScale = 1;

        anim.SetBool("Death",true);

        int counter = 0;

        // 死んだときの処理
        while (counter < 3)
        {
            playerColor.EnableKeyword("_EMISSION");
            playerColor.SetColor("_EmissionColor", new Color(0, 0, 0));
            yield return new WaitForSeconds(0.05f);

            playerColor.EnableKeyword("_EMISSION");
            playerColor.SetColor("_EmissionColor", new Color(1, 0, 0));
            yield return new WaitForSeconds(0.05f);

            counter++;

            yield return null;
        }

        Color color = new Color(1.0f, 0.0f, 0.0f);

        while (color.r > 0)
        {
            color.r -= 1.0f * Time.deltaTime;

            playerColor.EnableKeyword("_EMISSION");
            playerColor.SetColor("_EmissionColor", color);

            if (color.r < 0)
            {
                color.r = 0;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        SoundManager.Instance.WhiteNoise_Play();

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().DeathCamera();

        RestoreAll();
        yield return new WaitForSeconds(3.5f);

        deathCounter++;

        if (deathCounter >= 10)
        {
            LoadScenes.Instance.FadeOut("Title");
        }
        else
        {
            transform.position = (Vector3)restartPos;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().RestartCamera();
			playerColor.EnableKeyword("_EMISSION");
			playerColor.SetColor ("_EmissionColor", new Color (1, 0, 0));

            playerHp = maxPlayerHP;
        }

        yield return new WaitForSeconds(2.0f);

        if (deathCounter < 10)
        {
            inputCancel = false;
            isDeath = false;
            anim.SetBool("Death", false);
        }

        yield return null;
    }
    
}
