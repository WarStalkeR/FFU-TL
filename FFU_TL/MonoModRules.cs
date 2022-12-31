namespace MonoMod {
    static class MonoModRules {
        static MonoModRules() {
            bool isSingleplayer = true; //!CoOpSpRpG.CONFIG.openMP;
            InlineRT.MonoModRule.Flag.Set("SP", isSingleplayer);
        }
    }
}