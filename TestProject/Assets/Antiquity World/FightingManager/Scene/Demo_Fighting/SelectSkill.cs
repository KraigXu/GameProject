using System.Collections;
using System.Collections.Generic;
using GameSystem.Skill;
using Invector.vMelee;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Demo(Fighting_skill)场景脚本
/// </summary>
public class SelectSkill : MonoBehaviour
{
    public vMeleeManager Manager;
    
    [SerializeField]
    private Image _container;
    [SerializeField]
    private Text _number;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Button _rightBtn;
    [SerializeField]
    private Button _leftBtn;

    private int _currentId = 1; //当前技能ID

    private SkillGroup _skill;
	// Use this for initialization
	void Start () {
	    _rightBtn.onClick.AddListener(NextSkill);	
        _leftBtn.onClick.AddListener(PreviousSkill);

	    ChangeInfo();

	}
	
	void Update () {

	    if (Input.GetKeyUp(KeyCode.LeftArrow))
	    {
            PreviousSkill();
	    }

	    if (Input.GetKeyUp(KeyCode.RightArrow))
	    {
            NextSkill();
	    }

	}
    /// <summary>
    /// 更新界面显示信息
    /// </summary>
    public void ChangeInfo()
    {
        _skill = SkillSystem.Instance.NewSkillGroup(_currentId);
        Manager.SkillGroups[0] = _skill;
        _number.text = _currentId + "/" + SkillSystem.DicSkillInstancePool.Count;
        _name.text = _skill.Name;
        _container.sprite = _skill.Icon;
    }

    private void NextSkill()
    {
        if (_currentId < SkillSystem.DicSkillInstancePool.Count)
        {
            _currentId++;
        }
        else
        {
            _currentId = 1;
        }
        ChangeInfo();
    }

    private void PreviousSkill()
    {
        if (_currentId >1)
        {
            _currentId--;
        }
        else
        {
            _currentId = SkillSystem.DicSkillInstancePool.Count;
        }
        ChangeInfo();

    }


}
