using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ContentsListSOData", menuName = "UI/Popup/ContentsListSOData")]
public class ContentsListSOData : ScriptableObject
{
    [Serializable]
    public struct Contents
    {
        public RestaurantType type;
        public ContentsData contents;
    }

    [SerializeField]
    private List<Contents> contentsList = new List<Contents>();

    public List<Contents> ContentsList => contentsList;

}
