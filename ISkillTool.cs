public interface ISkillTool
{
    SkillType RequiredSkill { get; }
    int RequiredLevel { get; }
    float EfficiencyBonus { get; } // Multiplier für XP oder Geschwindigkeit
    bool CanUseForSkill(SkillType skill, int playerLevel);
}