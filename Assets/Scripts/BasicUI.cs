using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicUI : MonoBehaviour {

    void OnGUI()
    {
        int posX = 10;
        int posY = 10;
        int width = 80;
        int height = 50;
        int buffer = 10;
        
        List<string> itemList = Managers.Inventory.GetItemList();
        if (itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No Items");
        }
        foreach (string item in itemList) {
            int count = Managers.Inventory.GetItemCount(item);
            Texture2D image = Resources.Load<Texture2D>("Icons/" + item);
            GUI.Box(new Rect(posX, posY, width, height),
                new GUIContent("(" + count + ")", image));
            posX += width + buffer;
        }

    }
}
