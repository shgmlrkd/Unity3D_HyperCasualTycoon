using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentsdData", menuName = "UI/Popup/ContentsdData")]
public class ContentListData : ScriptableObject
{

    [SerializeField] private List<ContentData> contentList;
    
    public List<ContentData> ContentList => contentList;

}
[Serializable]
public class ContentData
{
    [SerializeField] private string typeId;
    [SerializeField] private string typeText;
    [SerializeField] private Sprite image;
    [SerializeField] private string info;
    [SerializeField] private int upgradeMaxCount;
    [SerializeField] private List<int> upgradCount;
    [SerializeField] private List<int> payCount;

    [SerializeField] private Color textColor;
    [SerializeField] private Color backgroundColor;


    public string TypeId => typeId;
    public string TypeText => typeText;
    public Sprite Image => image;
    public string Info => info;
    public int UpgradeMaxCount => upgradeMaxCount;
    public List<int> UpgradCount => upgradCount;
    public List<int> PayCount => payCount;
    public Color TextColor => textColor;
    public Color BackgroundColor => backgroundColor;
}
