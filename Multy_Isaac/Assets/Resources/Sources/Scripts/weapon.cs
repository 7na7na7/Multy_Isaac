using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
   public Sprite spr;
   public Vector2 tr;
   public Vector2 scale;
   public float CoolTime;
   public Vector2 bulletPos; 
   public string BulletName;
   public int weaponIndex;
   
   
   public weapon DeepCopy()
   {
      weapon Copytem = new weapon();
      Copytem.spr = this.spr;
      Copytem.tr = this.tr;
      Copytem.scale = this.scale;
      Copytem.CoolTime = this.CoolTime;
      Copytem.bulletPos = this.bulletPos;
      Copytem.BulletName = this.BulletName;
      Copytem.weaponIndex = this.weaponIndex;

      return Copytem;
   }
}
