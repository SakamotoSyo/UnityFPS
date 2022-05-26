using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    //移動速度。Inspectorから数値を変更できる。外部のクラスからは変更できない。
    [SerializeField] float moveSpeed;

    //Rigidbody型のplayerRidigbody変数を宣言
    Rigidbody playerRigidbody;

    //小数型の変数を２つ宣言
    float moveX, moveZ;
    //真偽値型の変数を２つ宣言
    bool moveZpermission, moveXpermission;

    // Start is called before the first frame update
    //最初のフレームだけ実行される処理
    void Start()
    {
        //このスクリプトがアタッチされているオブジェクト(Player)のRigidbodyを取得
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //毎フレーム実行される処理
    void Update()
    {
        //小数型の変数に、Horizontalで指定したキーに対して入力があった場合、度合いに応じた戻り値「-1」～「1」が割り当てられる
        moveX = Input.GetAxis("Horizontal");
        //小数型の変数に、Verticalで指定したキーに対して入力があった場合、度合いに応じた戻り値「-1」～「1」が割り当てられる
        moveZ = Input.GetAxis("Vertical");

        //Verticalで指定したキーに対して入力があった場合
        if (moveZ != 0)
        {
            //前後に移動する処理を開始する為の許可を出す
            moveZpermission = true;
        }
        //Horizontalで指定したキーに対して入力があった場合
        if (moveX != 0)
        {
            //左右に移動する処理を開始する為の許可を出す
            moveXpermission = true;
        }
    }

    void FixedUpdate()
    {
        //前後もしくは左右に移動する処理開始の許可が出たら、{}内の処理を実行する
        if (moveZpermission || moveXpermission)
        {
            //キー入力が無い時まで{}内の処理が呼ばれないよう、不許可に戻しておく
            moveZpermission = false;
            moveXpermission = false;
            //このスクリプトがアタッチされているオブジェクト(Player)の向きを基準としたX方向とZ方向に移動するよう、
            //１フレームの変化量に対応させてplayerRigidbodyの速度を上書きする
            //回転移動防止の為、インスペクターからFreezeRotation X Y Z にチェックを入れておく
            playerRigidbody.velocity = transform.rotation * new Vector3(moveX * moveSpeed, 0, moveZ * moveSpeed) * Time.deltaTime * 100;
        }
        else
        {
            //移動許可が出ていない時は力を加えないようにリセットする
            playerRigidbody.velocity = Vector3.zero;
        }
    }
}