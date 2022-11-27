public enum SocialClass { Slayer, Commons, SemiNoble, Noble, King }
public enum Job
{
    Slayer, Smith, Bania, MasterSmith, Merchant,
    Knight, Scholar, Masterknight, Alchemist,
    Baron, Viscount, Earl, Marquess, Duke, GrandDuke,
    King
}
public enum CharacterStatType
{
    MySocialClass = 1, MyJob, MyAge, TodoProgress, MyRound, MyPositon, ActivePoint, MyItem,
    Slayer, Smith, Bania, MasterSmith, Merchant,
    Knight, Scholar, MasterKnight, Alchemist,
    Baron, Viscount, Earl, Marquess, Duke, GrandDuke,
    King
}

public enum UIPopUpOrder { MainUI, InvenPanel, MiniMapPanel, SettingPanel, QuestPanel }
public enum UIInventoryOrder { Name, MySocialClass, MyJob, MyAge, TodoProgress, Item = 8 }
public enum UIMainButtonOrder { Setting, SkipDay }
public enum UIMainImageOrder { Job = 2, TodoProgress = 4, ActivePoint = 7 }
public enum UISettingButtonOrder { SettingClose, Pause, Save, Load, Sound, Resume, LoadClose, SoundClose }
public enum UISettingPanelOrder { Pause, Save, Load, Sound }
public enum UISoundOrder { Background, Effect }