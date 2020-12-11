using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class wep
{
    public bool isMelee = false;
    public Sprite spr;
    public Vector2 tr;
    public Vector2 scale;
    public float CoolTime;
    public Transform bulletPos; 
    public string BulletName;
    public int weaponIndex;
    public float reLoadTime;
    public int BulletCount;
    public float ClusterRate;
    public wep DeepCopy()
    {
        wep Copytem = new wep();
        Copytem.isMelee = this.isMelee;
        Copytem.reLoadTime = this.reLoadTime;
        Copytem.BulletCount = this.BulletCount;
        Copytem.spr = this.spr;
        Copytem.tr = this.tr;
        Copytem.scale = this.scale;
        Copytem.CoolTime = this.CoolTime;
        Copytem.bulletPos = this.bulletPos;
        Copytem.BulletName = this.BulletName;
        Copytem.weaponIndex = this.weaponIndex;
        Copytem.ClusterRate = this.ClusterRate;
        
        return Copytem;
    }
}
public class weapon : MonoBehaviour
{
    public wep weaponObj;
}
