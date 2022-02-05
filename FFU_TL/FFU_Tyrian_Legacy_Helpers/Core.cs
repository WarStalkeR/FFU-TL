namespace FFU_Tyrian_Legacy {
    public enum DataCategory {
        mods_bal_turr_Basic,
        weap_bal_turr_Basic,
        mods_bal_turr_Advanced,
        weap_bal_turr_Advanced,
        weap_bal_turr_ClusAMR,
        weap_bal_turr_MassDrv,
        empty_category_A,
        empty_category_B,
        mods_misc_LongueSupport,
        empty_category_C,
        mods_engines_Basic,
        mods_reactors_Basic,
        empty_category_D,
        loot_LowChanceAdv,
        mods_reactors_Advanced,
        weap_bal_turr_GravCharge,
        upgrades_clonebay_Cap,
        ships_civilian_Basic,
        mods_command_Improved,
        loot_EasyPirate,
        loot_EasyMilitary,
        loot_PhaseOne,
        loot_PhaseTwo,
        loot_NormalMilitary,
        loot_NormalPirate,
        loot_PirPhaseThree,
        loot_PirateKeep,
        rewards_Hitchhiker,
        loot_PirPhaseEight,
        loot_PirPhaseTwo,
        loot_TierTwo,
        loot_TierTwoPlus,
        loot_TierTwoEnergy,
        loot_TierThree,
        loot_WeapAntimatter,
        loot_MiningEquip,
        loot_CoreMining
    }
    public struct TexturePatch {
        public string artHex;
        public string emitHex;
        public string lightHex;
        public string tilesHex;
        public string blocksHex;
        public string airwayHex;
        public string repairHex;
        public int xOffset;
        public int yOffset;
        public int tWidth;
        public int tHeight;
    }
}