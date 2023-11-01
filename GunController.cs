using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Animator gunAnim;
    [SerializeField] private Transform gun;
    [SerializeField] private float gunDistans = 1.5f;

    private bool gunFacingRight =true;

    [Header("Bullt")]
    [SerializeField] private GameObject bulltPrefab;
    [SerializeField] private float bulltspeed;
    public int currentBullets;
    public int maxBullets;

    private void Start()
    {
        ReloadGun();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        //クォータ二オンとは回転を表す時に使用される仕組み
        //クォータニオンを使用して銃の回転を設定します。Quaternion.Eulerはオイラー角からクォータニオンを作成し、
        //Mathf.Atan2は方向ベクトルのy成分とx成分の間の角度を計算します。最後に、Mathf.Rad2Degは角度をラジアンから度に変換します。
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistans, 0, 0);

        if (Input.GetKeyDown(KeyCode.Mouse0)&& Havebullets())
            Shoot(direction);

        if (Input.GetKeyDown(KeyCode.R))
            ReloadGun();

        GunFlipController(mousePos);
    }

    private void GunFlipController(Vector3 mousePos)
    {
        if (mousePos.x < gun.position.x && gunFacingRight)
            GunFlip();
        if (mousePos.x > gun.position.x && !gunFacingRight)
            GunFlip();
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }

    public void Shoot(Vector3 direction)
    {
        gunAnim.SetTrigger("Shoot");    //アニメータのtrigger名と一緒でなければならない

        GameObject newBullet = Instantiate(bulltPrefab, gun.position,Quaternion.identity );//回転のところgun.rotationでよくね？
        newBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulltspeed;

        Destroy(newBullet, 7);
    }
    private void ReloadGun()
    {
        currentBullets = maxBullets;
    }
    public bool Havebullets()
    {
        if(currentBullets <= 0)
        {
            return false;
        }
        currentBullets--;
        return true;
    }
}
