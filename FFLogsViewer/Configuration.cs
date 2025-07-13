using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using FFLogsViewer.Model;
using Newtonsoft.Json;

namespace FFLogsViewer;

[Serializable]
public class Configuration : IPluginConfiguration
{
    [JsonIgnore]
    public const int CurrentConfigVersion = 1;
    public int Version { get; set; } = CurrentConfigVersion;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public bool ContextMenu { get; set; } = true;
    public bool ContextMenuStreamer { get; set; }
    public bool ContextMenuPartyView { get; set; } = true;
    public bool ContextMenuAlwaysPartyView { get; set; }
    public bool OpenInBrowser { get; set; }
    public bool ShowTomestoneOption { get; set; } = false;
    public string ContextMenuButtonName { get; set; } = "Search FF Logs";
    public bool IsDefaultViewParty { get; set; }
    public bool HideInCombat { get; set; }
    public bool IsDefaultLayout { get; set; } = true;
    public bool IsHistoricalDefault { get; set; } = true;
    public bool IsEncounterLayout { get; set; } = true;
    public bool IsCachingEnabled { get; set; } = true;
    public bool IsAllJobsDefault { get; set; } = true;
    public int NbOfDecimalDigits { get; set; }
    public StatType? DefaultStatTypePartyView { get; set; }
    public LayoutEntry? DefaultEncounterPartyView { get; set; }
    public List<LayoutEntry> Layout { get; set; } = [];
    public List<Stat> Stats { get; set; } = [];
    public Metric Metric { get; set; } = new() { Name = "rDPS", InternalName = "rdps" };
    public Style Style { get; set; } = new();
    public OpenWith OpenWith { get; set; } = new();
    public bool IsUpdateDismissed2213 { get; set; }

    public void Save()
    {
        Service.Interface.SavePluginConfig(this);
    }

    public void Initialize()
    {
        if (this.IsDefaultLayout || this.Layout.Count == 0)
        {
            this.SetDefaultLayout();
        }

        if (this.Stats.Count == 0)
        {
            this.Stats.AddRange(GetDefaultStats());
        }

        this.Upgrade();
    }

    public void Upgrade()
    {
        // all stars stats
        if (this.Version == 0)
        {
            var defaultStats = GetDefaultStats();
            if (this.Stats.Count < defaultStats.Count)
            {
                for (var i = this.Stats.Count; i < defaultStats.Count; i++)
                {
                    this.Stats.Add(defaultStats[i]);
                }
            }

            this.Version++;
            this.Save();
        }
    }

    public void SetDefaultLayout()
    {
        this.Layout = GetDefaultLayout();
        this.IsDefaultLayout = true;
    }

    private static List<LayoutEntry> GetDefaultLayout()
    {
        return
        [
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "中量级", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "7.2", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "热舞绿光", Expansion = "Dawntrail", Zone = "ACC Cruiserweight", ZoneId = 68, Encounter = "Dancing Green", EncounterId = 97, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "狂热糖潮", Expansion = "Dawntrail", Zone = "ACC Cruiserweight", ZoneId = 68, Encounter = "Sugar Riot", EncounterId = 98, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "野蛮恨心", Expansion = "Dawntrail", Zone = "ACC Cruiserweight", ZoneId = 68, Encounter = "Brute Abombinator", EncounterId = 99, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "剑嚎", Expansion = "Dawntrail", Zone = "ACC Cruiserweight", ZoneId = 68, Encounter = "Howling Blade", EncounterId = 100, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "轻量级", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "7.2", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "黑猫", Expansion = "Dawntrail", Zone = "ACC Light-heavyweight", ZoneId = 62, Encounter = "Black Cat", EncounterId = 93, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "蜂蜂小甜心", Expansion = "Dawntrail", Zone = "ACC Light-heavyweight", ZoneId = 62, Encounter = "Honey B. Lovely", EncounterId = 94, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "野蛮爆弹狂人", Expansion = "Dawntrail", Zone = "ACC Light-heavyweight", ZoneId = 62, Encounter = "Brute Bomber", EncounterId = 95, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "狡雷", Expansion = "Dawntrail", Zone = "ACC Light-heavyweight", ZoneId = 62, Encounter = "Wicked Thunder", EncounterId = 96, Difficulty = "Normal", DifficultyId = 101, SwapId = "7.2", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "绝本 (DT)", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝巴哈", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Unending Coil of Bahamut", EncounterId = 1073, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝神兵", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Weapon's Refrain", EncounterId = 1074, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝亚", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Epic of Alexander", EncounterId = 1075, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝龙诗", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "Dragonsong's Reprise", EncounterId = 1076, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝欧", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Omega Protocol", EncounterId = 1077, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝伊甸", Expansion = "Dawntrail", Zone = "Futures Rewritten", ZoneId = 65, Encounter = "Futures Rewritten", EncounterId = 1079, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "绝本 (EW)", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝巴哈", Expansion = "Endwalker", Zone = "Ultimates (Legacy)", ZoneId = 43, Encounter = "The Unending Coil of Bahamut", EncounterId = 1060, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝神兵", Expansion = "Endwalker", Zone = "Ultimates (Legacy)", ZoneId = 43, Encounter = "The Weapon's Refrain", EncounterId = 1061, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝亚", Expansion = "Endwalker", Zone = "Ultimates (Legacy)", ZoneId = 43, Encounter = "The Epic of Alexander", EncounterId = 1062, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝龙诗", Expansion = "Endwalker", Zone = "Dragonsong's Reprise", ZoneId = 45, Encounter = "Dragonsong's Reprise", EncounterId = 1065, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "绝欧", Expansion = "Endwalker", Zone = "The Omega Protocol", ZoneId = 53, Encounter = "The Omega Protocol", EncounterId = 1068, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 1 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "极神", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-" },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "艳翼蛇鸟", Expansion = "Dawntrail", Zone = "Trials I (Extreme)", ZoneId = 58, Encounter = "Valigarmanda", EncounterId = 1071, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "佐拉加", Expansion = "Dawntrail", Zone = "Trials I (Extreme)", ZoneId = 58, Encounter = "Zoraal Ja", EncounterId = 1072, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "永恒女王", Expansion = "Dawntrail", Zone = "Trials I (Extreme)", ZoneId = 58, Encounter = "Queen Eternal", EncounterId = 1078, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "泽莲尼娅", Expansion = "Dawntrail", Zone = "Trials II (Extreme)", ZoneId = 67, Encounter = "Zelenia", EncounterId = 1080, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "诛灭战", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-" },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "暗黑之云", Expansion = "Dawntrail", Zone = "Alliance Raids (Chaotic)", ZoneId = 66, Encounter = "Cloud of Darkness", EncounterId = 2061, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "幻巧战", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-" },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "朱雀", Expansion = "Dawntrail", Zone = "Trials (Unreal)", ZoneId = 64, Encounter = "Suzaku", EncounterId = 3010, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "白虎", Expansion = "Dawntrail", Zone = "Trials (Unreal)", ZoneId = 64, Encounter = "Byakko", EncounterId = 3009, Difficulty = "Normal", DifficultyId = 100 },
        ];
    }

    private static List<Stat> GetDefaultStats()
    {
        return
        [
            new Stat { Name = "Best", Type = StatType.Best, IsEnabled = true },
            new Stat { Alias = "Med.", Name = "Median", Type = StatType.Median, IsEnabled = true },
            new Stat { Name = "Kills", Type = StatType.Kills, IsEnabled = true },
            new Stat { Name = "Fastest", Type = StatType.Fastest, IsEnabled = false },
            new Stat { Alias = "/metric/", Name = "Best Metric", Type = StatType.BestAmount, IsEnabled = false },
            new Stat { Name = "Job", Type = StatType.Job, IsEnabled = true },
            new Stat { Name = "Best Job", Type = StatType.BestJob, IsEnabled = false },
            new Stat { Alias = "ASP", Name = "All Stars Points", Type = StatType.AllStarsPoints, IsEnabled = false },
            new Stat { Alias = "ASP R", Name = "All Stars Rank", Type = StatType.AllStarsRank, IsEnabled = false },
            new Stat { Alias = "ASP R%", Name = "All Stars Rank %", Type = StatType.AllStarsRankPercent, IsEnabled = false },
        ];
    }
}
