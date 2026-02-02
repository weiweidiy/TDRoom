using Game;
using System.Collections.Generic;
using UnityEngine;

public class PetManager
{
    Dictionary<int, GameObject> petPrefabs;

    public PetManager(Dictionary<int, GameObject> petPrefabs)
    {
        this.petPrefabs = petPrefabs;
    }

}
