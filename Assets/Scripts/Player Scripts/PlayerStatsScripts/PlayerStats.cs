using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public enum statNames {damage, moveSpeed, fireRate,projectiles,accuracy,maxHealth,regenRate}
public class PlayerStats {
    

    public List<Stat> statValues;
    public PlayerStats() {
        this.statValues = new List<Stat>();
        for (int i=0;i<Enum.GetNames(typeof(statNames)).Length;i++) {statValues.Add(new Stat(1)); }//fill list with empties, then overwrite. little bit hacky but whatev
        statValues[(int)statNames.damage] = generateStat(11,new[] {0,20,50,100,180,300,500,800,1250,2000,3500}, new[] {10d,15,20,25,30,40,50,60,70,85,100}, "Damage");
        statValues[(int)statNames.fireRate] = generateStat(11,new[] {0,20,50,100,180,300,500,800,1250,2000,3500},new[] {0.4,0.37,0.34,0.31,0.28,0.25,0.22,0.19,0.16,0.15,0.12d}, "Fire Rate");
        statValues[(int)statNames.moveSpeed] = generateStat(6,new[] {0,100,200,500,800,1250},new[] {4,4.8,5.6,6.4,7.2,8d}, "Movement Speed");
        statValues[(int)statNames.accuracy] = generateStat(6,new[] {0,100,180,300,500,800},new[] {10d,8.5,7,5.5,4,2.5}, "Accuracy");
        statValues[(int)statNames.projectiles] = generateStat(6, new[] {0,500,800,1250,2000,3000}, new[] {1d,2,3,4,5,6}, "Bullets");
        statValues[(int)statNames.maxHealth] = generateStat(11,new[] {0,20,50,100,180,300,500,800,1250,2000,3000}, new[] {50d,75,100,125,150,175,200,250,300,350,400}, "Max HP");
        statValues[(int)statNames.regenRate] = generateStat(6,new[] {0,100,300,800,1250,2000},new[] {0d,0.5,1,2,4,8}, "Regen");
    }

    Stat generateStat(int size, int[] prices, double[] values, string _label) {
        Stat newStat = new Stat(size);
        newStat.label = _label;
        for (int i=0;i<newStat.upgradeLevels.Length;i++) {
            newStat.upgradeLevels[i] = new Upgrade(prices[i],values[i]);
        }
        return newStat;
    }

    public double GetValue(Stat statName) {
        return statName.upgradeLevels[statName.currentLevel].value;
    }
}

public class Stat {
    public int currentLevel;
    public Upgrade[] upgradeLevels;
    public string label;
    public Stat(int upgradeCount) {
        this.currentLevel = 0;
        this.upgradeLevels = new Upgrade[upgradeCount];
    }

}

public class Upgrade {//represents an individual upgrade
    public int price;
    public double value;//number in stat e.g. 10 damage
    public Upgrade(int _price, double _value) {
        this.price = _price;
        this.value = _value;
    }

}