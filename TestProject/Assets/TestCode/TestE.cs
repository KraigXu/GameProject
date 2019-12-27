using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class TestE : MonoBehaviour
{
    public class Item
    {
        public int Gid;       //唯一ID
        public string Text;   //文本
        public string TypeGroup;

        public string Value;

    }

    void Start()
    {
        Item item=new Item();

        item.Gid = 1;
        item.Text = "{Name:UUUUU},{value:UIII},{Untiy:IIII}";
        item.TypeGroup = "1_2_3_4";
        item.Value = "";


        item.Gid = 2;
        item.Text = "{Name:AAA};";

    }




    // Update is called once per frame
    void Update()
    {

    }
}


