public enum Direction { Left = 1, Right = -1 }

public enum SocialClass { Helot, Commons, SemiNoble, Noble, King }
public enum Job
{
    Slayer,
    Smith, Bania,
    Knight, Scholar,
    LowNobility, MiddleNobility, HighNobility,
    King
}
/*public enum Job
{
    Slayer,
    Smith, Bania, MasterSmith, Merchant,
    Knight, Scholar, Masterknight, Alchemist,
    Baron, Viscount, Earl, Marquess, Duke, GrandDuke,
    King
}*/
public enum CharacterStatType
{
    MySocialClass = 1, MyJob, MyAge, Reputation, Gold,//MyRound,
    MyPositon, ActivePoint, MyItem,
    Knight, Scholar,
    LowNobility, MiddleNobility, HighNobility,
    King
}
/*public enum CharacterStatType
{
    MySocialClass = 1, MyJob, MyAge, TodoProgress, MyRound, MyPositon, ActivePoint, MyItem,
    Slayer, Smith, Bania, MasterSmith, Merchant,
    Knight, Scholar, MasterKnight, Alchemist,
    Baron, Viscount, Earl, Marquess, Duke, GrandDuke,
    King
}*/

public enum StartButtonOrder { New, Load, Exit }
public enum UIPopUpOrder { MainUI, InvenPanel, SettingPanel, QuestPanel }
public enum UIInventoryOrder { Name, MySocialClass, MyJob, MyAge, TodoProgress, Item = 8 }
public enum UIMainButtonOrder { Setting, SkipDay }
public enum UIMainImageOrders { Day, Qurter, ActivePoint, Job, TodoProgress }
//public enum UISettingButtonOrder { SettingClose, Pause, Save, Load, Sound, Resume, LoadClose, SoundClose, Back, BackOk, BackCancel }
public enum UISettingButtonOrder { SettingClose, Pause, Save, Load, Sound, Back, Resume, LoadClose, SoundClose, BackOk, BackCancel }
public enum UISettingPanelOrder { Pause, Save, Load, Sound, Back }
public enum UISoundOrder { Background, Effect }