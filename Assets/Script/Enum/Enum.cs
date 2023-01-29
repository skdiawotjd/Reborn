public enum Direction { Left = 1, Right = -1 }

public enum SocialClass { Helot, Commons, SemiNoble, Noble, King }
public enum Job
{
    Slayer,
    Smith, Bania,
    Knight, Alchemist,
    LowNobility, MiddleNobility, HighNobility,
    King
}
public enum CharacterStatType
{
    MySocialClass = 1, MyJob, MyAge, Reputation, Gold,
    MyPositon, ActivePoint, Proficiency, MyItem,
    Smith, Bania,
    Knight, Alchemist,
    LowNobility, MiddleNobility, HighNobility,
    King
}

public enum StartButtonOrder { New, Load, Exit }
public enum UIPopUpOrder { MainUI, InvenPanel, SettingPanel, QuestPanel, ConversationPanel }
public enum UIInventoryOrder { Name, MySocialClass, MyJob, MyAge, TodoProgress, Item = 8 }
public enum UIMainButtonOrder { Setting, SkipDay }
public enum UIMainImageOrders { Day, Qurter, ActivePoint, Job, Reputation }
//public enum UIMainImageOrders { Day, Qurter, Proficiency, Job, Reputation }
//public enum UISettingButtonOrder { SettingClose, Pause, Save, Load, Sound, Resume, LoadClose, SoundClose, Back, BackOk, BackCancel }
public enum UISettingButtonOrder { SettingClose, Pause, Save, Load, Sound, Back, Resume, LoadClose, SoundClose, BackOk, BackCancel }
public enum UISettingPanelOrder { Pause, Save, Load, Sound, Back }
public enum UISoundOrder { Background, Effect }
public enum QuestState { Start, Progress, End, Stand, Story }
public enum QuestData { QuestNumber, QuestObjectNumber, ClearCount, QuestContent }