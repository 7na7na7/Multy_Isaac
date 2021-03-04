using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TemManager : MonoBehaviour
{
    private PhotonView pv;
    
    public int temCount = 100;
    private List<GameObject> temPrefabs=new List<GameObject>();
    public List<tem> temDatas;
    public List<wep> weaponList;
    public List<GameObject> temList;
    public int Index = 0;
    public int BulletIndex=0;
    public GameObject[] bulletPrefabs;
    public List<GameObject> bulletList=new List<GameObject>();

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Awake()
    {
        for (int j = 1; j < temCount; j++)
        {
            GameObject go = Resources.Load("Sources/Items/item" + j) as GameObject;
            if (go != null)
            {
                temPrefabs.Add(go);   
            }
        }
        for (int i = 0; i < temPrefabs.Count; i++)
        {
            tem tem = temPrefabs[i].GetComponent<Item>().item.DeepCopy();
            temDatas.Add(tem);

            if (tem.weaponIndex > 0)
            {
                wep weapon = temPrefabs[i].GetComponent<weapon>().weaponObj.DeepCopy();
                weaponList.Add(weapon);
            }
        }
    }

    public wep GetWeapon(int Index)
    {
        wep tem = weaponList.Find(data => data.weaponIndex == Index);
        wep copyTem=new wep();
        if (tem != null)
        {
            copyTem = tem.DeepCopy();
            return copyTem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return copyTem;
        }
    }
    public tem GetItemList(int Index)
    {
        tem tem = temDatas.Find(data => data.index == Index);
        tem copyTem=new tem();
        if (tem != null)
        {
            copyTem = tem.DeepCopy();
            return copyTem;
        }
        else
        {
            print(Index+"(이)라는 인덱스는 없어용!");
            return copyTem;
        }
    }
    public GameObject GetItemGameObject(int Index)
    {
        for (int i = 0; i < temDatas.Count; i++)
        {
            if (temDatas[i].index == Index)
                return temPrefabs[i];
        }
        print(Index + "(이)라는 인덱스는 없어용!");
        return temPrefabs[0];
    }

    public void setTem(int dex, Vector3 pos)
    {
        if(dex>1000)
            setBullet(dex-1001,pos);
        else
        {
            if (PhotonNetwork.OfflineMode)
                SetItem(dex,pos);
            else
                pv.RPC("SetItem",RpcTarget.AllBuffered,dex,pos);   
        }
    }

    public void delTem(int dex)
    {
        if(PhotonNetwork.OfflineMode)
            DelItem(dex);
        else
            pv.RPC("DelItem",RpcTarget.AllBuffered,dex);
    }
    
    public void setBullet(int dex, Vector3 pos)
    {
        if(PhotonNetwork.OfflineMode)
            SetBullet(dex,pos);
        else
            pv.RPC("SetBullet",RpcTarget.AllBuffered,dex,pos);
    }

    public void delBullet(int dex)
    {
        if(PhotonNetwork.OfflineMode)
            DelBullet(dex);
        else
            pv.RPC("DelBullet",RpcTarget.AllBuffered,dex);
    }


    [PunRPC]
    void SetItem(int dex, Vector3 pos)
    {
        GameObject tem=Instantiate(GetItemGameObject(dex), pos, Quaternion.identity);
        tem.GetComponent<Item>().Index = Index;
        temList.Add(tem);
        Index++;
    }

    [PunRPC]
    void DelItem(int dex)
    {
        for (int i = 0; i < temList.Count; i++)
        {
            if (temList[i].GetComponent<Item>().Index == dex)
            {
                Destroy(temList[i]);
                temList.RemoveAt(i);
                break;
            }
        }
    }
    
    [PunRPC]
    void SetBullet(int dex, Vector3 pos)
    {
        GameObject tem=Instantiate(bulletPrefabs[dex], pos, Quaternion.identity);
        tem.GetComponent<pickUpTem>().Index = BulletIndex; 
        bulletList.Add(tem);
        BulletIndex++;
    }
    
    [PunRPC]
    void DelBullet(int dex)
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i].GetComponent<pickUpTem>().Index == dex)
            {
                Destroy(bulletList[i]);
                bulletList.RemoveAt(i);
                break;
            }
        }
    }
}
