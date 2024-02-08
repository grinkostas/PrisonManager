using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Upgrades;

public class UpgradesLevelUpTexts : MonoBehaviour
{
    [SerializeField] private List<UpgradeLevelUpText> _upgradeLevelUpTexts;

    public string GetText(Upgrade upgrade) => _upgradeLevelUpTexts.Find(x => x.Upgrade.Id == upgrade.Id).LevelUpText;

    [System.Serializable]
    private class UpgradeLevelUpText
    {
        public string LevelUpText;
        public Upgrade Upgrade;
    }
}
