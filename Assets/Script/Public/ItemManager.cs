using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int OrderItem(string ItemType)
    {
        int Order = 0;
        int ItemNumber = int.Parse(string.Format("{0}", ItemType));
        //Debug.Log("OrderItem에서 현재 순서 " + Order);
        //Debug.Log("Character.instance.MyItem.Count가 " + Character.instance.MyItem.Count);

        for ( ; Order < Character.instance.MyItem.Count; Order++)
        {
            if(int.Parse(string.Format("{0}", Character.instance.MyItem[Order])) >= ItemNumber)
            {
                break;
            }
        }
        //Debug.Log("따라서 해당 아이템의 순서는 " + Order);
        return Order;
    }

    public bool CanDeleteItem(string CheckItemNumber)
    {
        string ItemNumber = CheckItemNumber.Substring(0, 4);
        int ItemOrder = OrderItem(ItemNumber);

        if (Character.instance.MyItemCount[ItemOrder] - (int)(CheckItemNumber[5] - '0') >= 0)
        {
            return true;
        }

        return false;
    }
    public bool IsExistItem(string CheckItemNumber)
    {
        for (int i = 0; i < Character.instance.MyItem.Count; i++)
        {
            if(CheckItemNumber == Character.instance.MyItem[i])
            {
                Debug.Log(CheckItemNumber + "라는 아이템이 존재");
                return true;
            }
        }
        Debug.Log(CheckItemNumber + "라는 아이템이 존재하지 않음");
        return false;
    }
}
