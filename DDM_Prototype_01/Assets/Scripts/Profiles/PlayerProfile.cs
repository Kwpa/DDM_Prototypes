using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerProfile", order = 3)]
public class PlayerProfile : ScriptableObject
{
    public string _playerID = "player id";
    public string _username = "username";
}
