using UnityEngine;
using Kryz.CharacterStats;

public enum EquipmentType
{
    Helmet,
    ChestArmour,
    Gauntlets,
    LegArmour,
    PrimaryWeapon,
    SecondaryWeapon,
    HeavyWeapon,
    NovaShard,
}

[CreateAssetMenu]
public class EquippableItem : Item {

    [Space]
    public EquipmentType equipmentType;
    [Space]
    public int recoveryBonus;
    public int resilienceBonus;
    public int agilityBonus;
    [Space]
    public float recoveryPrecentBonus;
    public float resiliencePrecentBonus;
    public float agilityPrecentBonus;

    public void Equip(Character c)
    {
        if (recoveryBonus != 0)
            c.Recovery.AddModifier(new StatModifier(recoveryBonus, StatModType.Flat, this));
        if (resilienceBonus != 0)
            c.Resilience.AddModifier(new StatModifier(resilienceBonus, StatModType.Flat, this));
        if (agilityBonus != 0)
            c.Agility.AddModifier(new StatModifier(agilityBonus, StatModType.Flat, this));

        if (recoveryPrecentBonus != 0)
            c.Recovery.AddModifier(new StatModifier(recoveryPrecentBonus, StatModType.PercentMult, this));
        if (resiliencePrecentBonus != 0)
            c.Recovery.AddModifier(new StatModifier(resiliencePrecentBonus, StatModType.PercentMult, this));
        if (agilityPrecentBonus != 0)
            c.Recovery.AddModifier(new StatModifier(agilityPrecentBonus, StatModType.PercentMult, this));
    }

    public void Unequip(Character c)
    {
        c.Recovery.RemoveAllModifiersFromSource(this);
        c.Resilience.RemoveAllModifiersFromSource(this);
        c.Agility.RemoveAllModifiersFromSource(this);
    }
}
