using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class ChestLootDemo : MonoBehaviour // All the interface stuff within the unity inspector
{
    public interface IWeighted
    {
        int Weight { get; } 
    }

    [Serializable]
    public class Chest : IWeighted
    {
        public string chestType;
        [SerializeField] private int weight;
        public int Weight => weight;
    }

    [Serializable]
    public class Prize : IWeighted
    {
        public string prizeType;
        [SerializeField] private int weight;
        public int Weight => weight;
    }

    [Header("Chest Weights")]
    [SerializeField] private List<Chest> chests;

    [Header("Prize Types / Weights")]
    [SerializeField] private List<Prize> woodChestPrizes;
    [SerializeField] private List<Prize> bronzeChestPrizes;
    [SerializeField] private List<Prize> silverChestPrizes;
    [SerializeField] private List<Prize> goldChestPrizes;
    [SerializeField] private List<Prize> platinumChestPrizes;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Runs the script, selecting a chest based on weight, then a prize from that chest, also based on weight
        {
            SelectChestAndPrize();
        }
    }

    private void SelectChestAndPrize()
    {
        Chest selectedChest = WeightedRandomSelection(chests); // Select a Chest

        List<Prize> prizeList = GetPrizeList(selectedChest); // Then select a prize from the selected chest
        if (prizeList != null && prizeList.Count > 0)
        {
            Prize selectedPrize = WeightedRandomSelection(prizeList);

            string output = $"Chest Type - {selectedChest.chestType} | Prize Type - {selectedPrize.prizeType}"; // Make the print nice and clear so it's easy to read
            Debug.Log(output);
        }
        else
        {
            Debug.LogError($"Unknown Chest Type!: {selectedChest.chestType}"); // Just in case there is a unknown chest type. Won't happen unless someone adds one thats not in the list below.
        }
    }



    private K WeightedRandomSelection<K>(List<K> items) where K : IWeighted // Picks a random item type based on weight
    {
        int totalWeight = 0;
        foreach (var item in items)
        {
            totalWeight += item.Weight;
        }

        int randomWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var item in items)
        {
            currentWeight += item.Weight;
            if (currentWeight >= randomWeight)
            {
                return item;
            }
        }

        return default; // if something breaks. Hopefully never
    }

    private List<Prize> GetPrizeList(Chest chest) // The list of chest that can be picked
    {
        switch (chest.chestType)
        {
            case "Wood Chest":
                return woodChestPrizes;
            case "Bronze Chest":
                return bronzeChestPrizes;
            case "Silver Chest":
                return silverChestPrizes;
            case "Gold Chest":
                return goldChestPrizes;
            case "Platinum Chest":
                return platinumChestPrizes;
            default:
                Debug.LogError("Unknown chest type: " + chest.chestType); // If not in list
                return null;
        }
    }
}
