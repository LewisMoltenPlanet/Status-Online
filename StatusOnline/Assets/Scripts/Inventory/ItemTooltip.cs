using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ItemTooltip : MonoBehaviour {

    [SerializeField] Text itemNameText;
    [SerializeField] Text itemSlotText;
    [SerializeField] Text itemStatsText;

    private StringBuilder sB = new StringBuilder();

    public void ShowTooltip(EquippableItem item)
    {
        itemNameText.text = item.itemName;
        itemSlotText.text = item.equipmentType.ToString();

        sB.Length = 0;
        AddStat(item.resilienceBonus, "RESILIENCE");
        AddStat(item.recoveryBonus, "RECOVERY");
        AddStat(item.agilityBonus, "AGILITY");

        gameObject.SetActive(true);
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName , bool isPercent = false)
    {
        if (sB.Length > 0)
            sB.AppendLine();

        if (value > 0)
            sB.Append("+");

        if (isPercent)
        {
            sB.Append(value * 100);
            sB.Append("% ");
        }
        else
        {
            sB.Append(value);
            sB.Append(" ");
        }

        sB.Append(statName);
    }
}
