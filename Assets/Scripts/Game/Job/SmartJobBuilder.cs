using System;
using System.Collections.Generic;

public static class SmartJobBuilder {
    private static int easyMoney = 210; // amount paid per easy item
    private static int minEasy = 1;
    private static int maxEasy = 2;
    private static List<InventoryType> EASY = new List<InventoryType> {InventoryType.LIGHT_BULB, InventoryType.WRENCH, InventoryType.PAINT};

    private static int mediumMoney = 810; // amount paid per medium item
    private static float minMediumMoney = 5000; // required to get this amount of total money before MEDIUM difficulty
    private static int minMedium = 2;
    private static int maxMedium = 3;
    private static List<InventoryType> MEDIUM = new List<InventoryType> {InventoryType.BATTERY, InventoryType.PLUNGER};

    private static int hardMoney = 1310; // amount paid per hard item
    private static float minHardMoney = 30000; // required to get this amount of total money before HARD difficulty
    private static int minHard = 3;
    private static int maxHard = 6;
    private static List<InventoryType> HARD = new List<InventoryType> {InventoryType.HAMMER};

    private static Random rnd = new Random();

    public static JobInfo BuildSmartJob(float currentTotalMoney) {
        var job = new JobInfo();
        if (currentTotalMoney > minHardMoney) { // pick hard job
            job.jobItems.AddRange(EASY);
            job.jobItems.AddRange(MEDIUM);
            job.jobItems.AddRange(HARD);
            job.jobItems = RandomFrom(job.jobItems, minHard, maxHard);
        } else if (currentTotalMoney > minMediumMoney) { // pick medium job
            job.jobItems.AddRange(EASY);
            job.jobItems.AddRange(MEDIUM);
            job.jobItems = RandomFrom(job.jobItems, minMedium, maxMedium);
        } else { // pick easy job
            job.jobItems.AddRange(EASY);
            job.jobItems = RandomFrom(job.jobItems, minEasy, maxEasy);
        }

        job.price = CalculatePrice(job.jobItems);
        return job;
    }

    private static List<InventoryType> RandomFrom(List<InventoryType> items, int min, int max) {
        List<InventoryType> picked = new List<InventoryType>();
        picked.AddRange(items);
        var amountToRemove = picked.Count - rnd.Next(min, max + 1);
        for (int i = 0; i < amountToRemove; i++) {
            picked.RemoveAt(rnd.Next(0, picked.Count));
        }

        return picked;
    }

    private static float CalculatePrice(List<InventoryType> items) {
        var total = 0.0f;
        items.ForEach((i) => {
            if (EASY.Contains(i)) total += easyMoney;
            else if (MEDIUM.Contains(i)) total += mediumMoney;
            else if (HARD.Contains(i)) total += hardMoney;
            else total += easyMoney;
        });
        return total;
    }
}

public class JobInfo {
    public List<InventoryType> jobItems = new List<InventoryType>();
    public float price = 0;
}