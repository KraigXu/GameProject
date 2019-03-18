using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class UiSkillItem : MonoBehaviour
{

    [SerializeField]
    private RectTransform _panel;

    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text _type;
    [SerializeField]
    private Text _level;


    [SerializeField] private List<Text> _propertys = new List<Text>();

    [SerializeField]
    private RectTransform _text;

    [SerializeField] private Text _text1;
    [SerializeField] private Text _text2;
    

    
    public class RowProperty
    {
        public string Name;
        public List<string> Types;
        public string Value;
        public List<string> Property=new List<string>();
    }
    

	void Start () {
        

    }
	
	void Update () {
		
	}

   
    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(string value)
    {

        RowProperty properties=new RowProperty();

        if(properties==null)
            return;

        _name.text = properties.Name;
        _type.text = properties.Types.ToString();
        _level.text = properties.Value;

        for (int i = 0; i < properties.Property.Count; i++)
        {

            


        }



        List<RowProperty> properties1=new List<RowProperty>();


        
    }
    
}
