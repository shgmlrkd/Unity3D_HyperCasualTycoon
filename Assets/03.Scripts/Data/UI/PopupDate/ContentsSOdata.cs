using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentsdData", menuName = "UI/Popup/ContentsdData")]
public class ContentsData : ScriptableObject
{

    [SerializeField] private List<ContentData> contents;
    
    public List<ContentData> Contents => contents;

}
[Serializable]
public class ContentData
{
    [SerializeField] private TypeId typeId;
    [SerializeField] private FoodType foodType;
    [SerializeField] private string typeText;
    [SerializeField] private Sprite image;
    [SerializeField] private string info;
    [SerializeField] private int upgradeMaxCount;
   

    [SerializeField] private Color textColor;
    [SerializeField] private Color backgroundColor;

   
    [SerializeField] private Vector3 position;
    [SerializeField] private GameObject employeePrefab;
    

    public TypeId TypeId => typeId;
    public FoodType FoodType => foodType;
    public string TypeText => typeText;
    public Sprite Image => image;
    public string Info => info;
    public int UpgradeMaxCount => upgradeMaxCount;
    public Color TextColor => textColor;
    public Color BackgroundColor => backgroundColor;

    public Vector3 Position => position;

    public GameObject EmployeePrefab => employeePrefab;
}
