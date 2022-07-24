using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UndertaleModLib;
using System.Collections;
using UndertaleModLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UndertaleModLib.Util;
using System.Drawing;

namespace HATE
{
    static class Shuffle
    {
        public const int WordSize = 4;
        public static List<string> DRChoicerControlChars = new List<string>{
            "\\\\C1",
            "\\\\C2",
            "\\\\C3",
            "\\\\C4",
            "\\C1",
            "\\C2",
            "\\C3",
            "\\C4",
            "\\C"
        };
        public static List<string> FormatChars = new List<string>{
            "/%%.", // SCR_TEXT_1935 in UNDERTALE
            "/%%\"", // item_desc_23_0 in UNDERTALE
            "/%%",
            "/^",
            "/%",
            "/",
            "%%",
            "%",
            "-",
            "/*"
        };
        // hack
        public static List<string> ForceShuffleReferenceChars = new List<string>{
            "#", "&", "^", /*"||",*/
            "\\[1]", "\\[2]", "\\[C]", "\\[G]", "\\[I]",
            "\\*Z", "\\*X", "\\*C", "\\*D",
            "\\X", "\\W", "\n",
            "* ",
            "...", "~", "(", ")", "?", "'", " ", ";", ":",
            "Greetings.", "Understood.", "Chapter", "English",
            // Kanji list generated from fnt_ja_main in DELTARUNE Chapter 1&2
            "―", "“", "”", "…", "※", "←", "↑", "→", "↓", "≠", "○", "★", "♪", "♯", "　", "、", "。", "々", "「", "」", "『", "』", "ぁ", "あ", "ぃ", "い", "ぅ", "う", "ぇ", "え", "ぉ", "お", "か", "が", "き", "ぎ", "く", "ぐ", "け", "げ",
            "こ", "ご", "さ", "ざ", "し", "じ", "す", "ず", "せ", "ぜ", "そ", "ぞ", "た", "だ", "ち", "ぢ", "っ", "つ", "づ", "て", "で", "と", "ど", "な", "に", "ぬ", "ね", "の", "は", "ば", "ぱ", "ひ", "び", "ぴ", "ふ", "ぶ", "ぷ", "へ", "べ", "ぺ",
            "ほ", "ぼ", "ぽ", "ま", "み", "む", "め", "も", "ゃ", "や", "ゅ", "ゆ", "ょ", "よ", "ら", "り", "る", "れ", "ろ", "ゎ", "わ", "ゐ", "ゑ", "を", "ん", "゛", "゜", "ァ", "ア", "ィ", "イ", "ゥ", "ウ", "ェ", "エ", "ォ", "オ", "カ", "ガ", "キ",
            "ギ", "ク", "グ", "ケ", "ゲ", "コ", "ゴ", "サ", "ザ", "シ", "ジ", "ス", "ズ", "セ", "ゼ", "ソ", "ゾ", "タ", "ダ", "チ", "ヂ", "ッ", "ツ", "ヅ", "テ", "デ", "ト", "ド", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "バ", "パ", "ヒ", "ビ", "ピ", "フ",
            "ブ", "プ", "ヘ", "ベ", "ペ", "ホ", "ボ", "ポ", "マ", "ミ", "ム", "メ", "モ", "ャ", "ヤ", "ュ", "ユ", "ョ", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ヮ", "ワ", "ヰ", "ヱ", "ヲ", "ン", "ヴ", "ヵ", "ヶ", "・", "ー", "一", "丁", "万", "丈", "三",
            "上", "下", "不", "与", "世", "両", "並", "中", "丸", "主", "久", "乗", "乙", "乱", "乳", "了", "予", "争", "事", "二", "互", "井", "些", "亡", "交", "享", "人", "仁", "今", "介", "仕", "他", "付", "代", "令", "以", "仮", "仰", "仲", "件",
            "任", "企", "休", "会", "伝", "伸", "似", "位", "低", "住", "体", "何", "余", "作", "使", "例", "供", "依", "価", "侮", "侵", "便", "係", "保", "信", "修", "個", "倍", "倒", "候", "借", "値", "偉", "停", "健", "側", "偶", "偽", "傑", "備",
            "催", "傭", "傷", "傾", "働", "像", "僕", "僧", "儀", "優", "元", "兄", "充", "先", "光", "克", "免", "兎", "入", "全", "公", "六", "共", "兵", "具", "典", "兼", "内", "円", "再", "冒", "写", "冠", "冬", "冶", "冷", "凍", "凝", "凡", "処",
            "凶", "出", "刀", "刃", "分", "切", "刊", "刑", "列", "初", "判", "別", "利", "刮", "到", "制", "刺", "刻", "則", "削", "前", "剣", "剤", "副", "割", "創", "劇", "力", "功", "加", "劣", "助", "努", "労", "効", "勇", "勉", "動", "勘", "務",
            "勝", "募", "勢", "包", "化", "北", "匹", "区", "医", "十", "千", "半", "卒", "協", "南", "単", "博", "占", "印", "危", "即", "却", "卵", "卿", "厄", "厚", "原", "厨", "厳", "去", "参", "友", "双", "反", "収", "取", "受", "口", "古", "句",
            "叫", "召", "可", "台", "史", "右", "号", "司", "各", "合", "同", "名", "吐", "向", "君", "否", "含", "吸", "吹", "告", "周", "呪", "味", "呵", "呼", "命", "咆", "和", "咲", "品", "員", "哮", "唯", "唱", "商", "問", "喚", "喜", "喧", "営",
            "嗚", "器", "噴", "囚", "四", "回", "団", "困", "囲", "図", "固", "国", "園", "土", "圧", "在", "地", "均", "坊", "垂", "型", "垢", "埃", "埋", "城", "域", "執", "基", "堪", "報", "場", "塊", "境", "墓", "増", "壁", "壊", "壌", "士", "壮",
            "声", "売", "変", "夏", "外", "多", "夜", "夢", "大", "天", "夫", "央", "失", "奇", "奈", "奉", "奏", "奥", "奨", "奪", "女", "好", "如", "妄", "妙", "妥", "姉", "始", "姫", "姿", "威", "娘", "婚", "婦", "嫌", "嫡", "嬢", "子", "字", "存",
            "孤", "学", "宅", "守", "安", "完", "官", "定", "宝", "実", "客", "宣", "室", "宮", "害", "家", "容", "宿", "寂", "寄", "密", "富", "寒", "寛", "寝", "寮", "対", "封", "専", "射", "尊", "導", "小", "少", "就", "尽", "局", "居", "屈", "届",
            "屋", "展", "属", "層", "履", "山", "岩", "峙", "島", "崇", "崩", "嵐", "川", "巣", "工", "左", "巨", "差", "己", "巻", "市", "布", "希", "帝", "師", "席", "帯", "帰", "帳", "常", "帽", "幅", "幕", "干", "平", "年", "幸", "幼", "幾", "広",
            "床", "序", "底", "店", "度", "座", "庫", "庭", "康", "廃", "延", "廷", "建", "弊", "式", "引", "弦", "弱", "張", "強", "弾", "当", "形", "彩", "彫", "彰", "影", "役", "彼", "征", "待", "律", "後", "徒", "従", "得", "御", "復", "徳", "心",
            "必", "忍", "志", "忘", "忙", "応", "忠", "快", "念", "怒", "怖", "思", "急", "性", "怪", "恋", "恍", "恐", "恥", "恩", "息", "恵", "悔", "悟", "悩", "悪", "悲", "情", "惑", "惚", "惜", "惨", "想", "愉", "意", "愚", "愛", "感", "慈", "態",
            "慣", "慮", "憎", "憶", "懐", "懲", "成", "我", "戦", "房", "所", "扉", "手", "才", "打", "払", "扱", "承", "技", "把", "抑", "投", "抗", "折", "抜", "択", "披", "抱", "抵", "抹", "押", "担", "拍", "拐", "拒", "招", "拝", "拠", "拡", "拷",
            "拾", "持", "指", "挑", "挙", "挫", "振", "捕", "捜", "捨", "掃", "授", "排", "掘", "採", "探", "接", "控", "推", "掲", "描", "提", "換", "握", "揮", "援", "揺", "損", "摂", "摩", "撃", "撮", "擁", "操", "支", "改", "攻", "放", "政", "故",
            "敏", "救", "敗", "教", "敢", "散", "敬", "数", "整", "敵", "敷", "文", "斗", "料", "斧", "断", "新", "方", "旅", "旋", "族", "既", "日", "旧", "早", "昆", "昇", "明", "易", "昔", "星", "映", "春", "昧", "昨", "昼", "時", "晩", "普", "景",
            "晴", "晶", "暇", "暖", "暗", "暮", "暴", "曜", "曲", "更", "書", "替", "最", "月", "有", "服", "朗", "望", "朝", "期", "木", "未", "末", "本", "札", "杉", "材", "束", "条", "来", "杯", "東", "板", "析", "枕", "枚", "果", "枝", "枠", "枯",
            "柄", "某", "染", "柔", "柱", "栄", "校", "株", "核", "根", "格", "案", "械", "棄", "棒", "棚", "森", "棲", "検", "業", "極", "楽", "構", "様", "標", "権", "横", "橋", "機", "欄", "欠", "次", "欲", "歌", "歓", "止", "正", "此", "武", "歩",
            "歯", "歳", "歴", "死", "殊", "残", "殴", "段", "殺", "殿", "母", "毎", "毒", "比", "毛", "氏", "民", "気", "水", "氷", "永", "汁", "求", "汗", "汚", "池", "決", "沈", "沌", "没", "油", "治", "況", "泉", "泊", "法", "泡", "波", "泣", "注",
            "泰", "泳", "洋", "洗", "洞", "活", "派", "流", "浄", "浮", "浴", "海", "浸", "消", "涙", "液", "淡", "深", "淵", "混", "添", "清", "済", "渋", "減", "渡", "温", "測", "湖", "湯", "湿", "満", "源", "準", "溶", "滅", "滝", "滞", "演", "漢",
            "潔", "激", "濃", "濯", "瀾", "火", "灯", "災", "炎", "炭", "点", "為", "焉", "無", "焦", "然", "焼", "煙", "照", "熟", "熱", "燃", "爆", "爛", "父", "片", "版", "牛", "牢", "物", "特", "犬", "犯", "状", "狙", "独", "狭", "猫", "献", "獄",
            "獣", "獲", "玄", "率", "玉", "王", "現", "球", "理", "環", "甘", "生", "産", "用", "由", "申", "男", "町", "画", "界", "略", "番", "異", "疑", "疲", "病", "症", "痛", "療", "発", "登", "白", "百", "的", "皆", "皮", "皿", "盗", "盛", "監",
            "盤", "目", "盲", "直", "相", "盾", "省", "看", "真", "眠", "眷", "眺", "着", "瞬", "瞳", "矢", "知", "短", "石", "砂", "砕", "砲", "破", "確", "磨", "礎", "示", "礼", "社", "祈", "祝", "神", "祭", "禁", "福", "秀", "私", "科", "秒", "秘", 
            "秩", "称", "移", "程", "税", "種", "稼", "稿", "積", "穏", "穴", "究", "空", "突", "窓", "窟", "立", "竜", "童", "端", "競", "笑", "符", "第", "筆", "等", "筋", "筐", "答", "算", "管", "箱", "節", "範", "築", "簡", "粉", "精", "糖", "糸",
            "系", "約", "納", "純", "紙", "級", "紛", "素", "索", "細", "紳", "紹", "終", "組", "経", "結", "絡", "絢", "給", "統", "絵", "絶", "継", "続", "維", "綿", "緊", "緑", "線", "編", "練", "縁", "縄", "縛", "縦", "縮", "績", "繁", "缶", "罪",
            "置", "罰", "美", "群", "羨", "義", "羽", "習", "翻", "翼", "老", "考", "者", "耐", "耳", "聖", "聞", "聴", "職", "肉", "肌", "肝", "肢", "肩", "育", "胃", "胆", "背", "胞", "胴", "胸", "能", "脈", "脚", "脱", "脳", "腐", "腕", "腰", "腸",
            "腹", "膨", "臆", "臣", "自", "臭", "至", "致", "興", "舌", "舐", "舞", "般", "船", "良", "色", "芯", "花", "芸", "若", "苦", "英", "茂", "茶", "草", "荒", "荘", "荷", "菓", "菜", "華", "落", "葉", "葬", "蓄", "蔵", "薄", "薬", "虐", "虚",
            "虫", "蛍", "蛮", "蝶", "融", "血", "行", "術", "街", "衝", "衡", "表", "袋", "被", "裁", "裂", "装", "裏", "裕", "補", "製", "複", "褒", "襲", "西", "要", "覆", "覇", "見", "規", "視", "覚", "覧", "親", "観", "角", "解", "触", "言", "計",
            "討", "訓", "記", "訪", "設", "許", "訳", "訶", "証", "評", "試", "詩", "詰", "話", "誇", "誉", "認", "誓", "誕", "誘", "語", "誠", "誤", "説", "読", "誰", "課", "調", "談", "請", "論", "諸", "諾", "謁", "講", "謝", "謡", "謳", "識", "譚",
            "警", "議", "護", "豊", "象", "豪", "貌", "貝", "負", "財", "貢", "貨", "販", "貫", "責", "貯", "貴", "買", "貸", "費", "賃", "資", "賊", "賛", "賜", "賞", "賢", "質", "購", "贈", "赤", "走", "起", "超", "越", "趣", "足", "距", "跡", "路",
            "踏", "蹴", "身", "車", "軍", "軒", "転", "軽", "較", "輝", "輩", "輪", "辛", "辞", "辱", "辺", "辻", "込", "迅", "迎", "近", "返", "迫", "迷", "追", "退", "送", "逃", "逆", "透", "逐", "途", "這", "通", "逞", "速", "造", "連", "逮", "週",
            "進", "遂", "遅", "遇", "遊", "運", "過", "道", "達", "違", "遠", "適", "遭", "選", "避", "邪", "邸", "郎", "部", "都", "配", "酵", "酷", "酸", "醒", "重", "野", "量", "金", "釜", "針", "釣", "鉄", "銀", "銃", "鋸", "録", "鍛", "鎖", "鎧",
            "鏡", "鑑", "長", "門", "閉", "開", "閑", "間", "関", "闇", "闘", "防", "阻", "降", "限", "陛", "陣", "除", "陰", "陶", "険", "陽", "隊", "階", "随", "際", "障", "隠", "雄", "雅", "集", "雇", "雌", "雑", "離", "難", "雨", "雪", "雰", "雲",
            "雷", "電", "需", "霜", "霧", "露", "青", "静", "非", "面", "革", "音", "響", "頃", "項", "順", "須", "預", "領", "頭", "頼", "題", "額", "顔", "願", "風", "飛", "食", "飲", "飼", "飽", "飾", "養", "館", "首", "香", "駁", "駆", "駒", "騎",
            "騒", "験", "驚", "骨", "高", "髪", "鬼", "魂", "魅", "魔", "鮮", "鳥", "鳴", "麗", "麦", "黄", "黒", "黙", "鼓", "鼠", "鼻", "齢",
            "！", "％", "＆", "（", "）", "＊", "＋", "－", "．", "／", "０", "１", "２", "３", "４", "５", "６", "７", "８", "９", "：", "＝", "？", "Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ", "Ｇ", "Ｈ", "Ｉ", "Ｊ", "Ｋ", "Ｌ", "Ｍ", "Ｎ", "Ｏ", "Ｐ", "Ｑ", "Ｒ", "Ｓ", "Ｔ", "Ｕ", "Ｖ", "Ｗ", "Ｘ", "Ｙ", "Ｚ",
            "ａ", "ｂ", "ｃ", "ｄ", "ｅ", "ｆ", "ｇ", "ｈ", "ｉ", "ｊ", "ｋ", "ｌ", "ｍ", "ｎ", "ｏ", "ｐ", "ｑ", "ｒ", "ｓ", "ｔ", "ｕ", "ｖ", "ｗ", "ｘ", "ｙ", "ｚ",
            "～", "･", "ｦ", "ｧ", "ｨ", "ｩ", "ｬ", "ｭ", "ｮ", "ｯ", "ｰ", "ｱ", "ｲ", "ｳ", "ｴ", "ｵ", "ｶ", "ｷ", "ｸ", "ｹ", "ｺ", "ｻ", "ｼ", "ｽ", "ｾ", "ｿ", "ﾀ", "ﾁ", "ﾂ", "ﾃ", "ﾄ", "ﾅ", "ﾆ", "ﾈ", "ﾉ", "ﾊ", "ﾋ", "ﾌ", "ﾍ", "ﾎ", "ﾏ", "ﾐ", "ﾑ", "ﾒ", "ﾓ", "ﾔ", "ﾖ", "ﾗ", "ﾘ", "ﾙ", "ﾚ", "ﾛ", "ﾜ", "ﾝ", "ﾞ", "ﾟ",
        };
        public static List<string> CategorizableForcedSubstring = new List<string>{ "#", "&" };
        public static string[] BannedStrings = {
            "_", "@@",
            "Z}", "`}", "0000000000000000", "~~~",
            "DO NOT EDIT IN GAMEMAKER",
            "gml_", "scr_", "SCR_", "room_", "obj_", "blt_", "spr_",
            "audiogroup_",
            "abc_1", "string_",
            "battle_", "item_", "recover_hp",
            "music/", ".ogg", "external/", "bg_", "Compatibility_", "path_",
            "attention_hackerz_no_2", "snd_", "__",
            "castroll_", "credits_", "mettnews_",
            "shop1_", "shop2_", "shop3_", "shop4_", "shop5_",
            "mett_opera", "item_name_", "item_use_",
            "the_end", "mett_ratings_", "mettquiz_", "trophy_install",
            "hardmode_", "elevator_", "flowey_", "item_drop_", "labtv_",
            "damage_", "joyconfig_", "settings_", "title_", "soundtest_",
            "roomname_", "title_press_", "instructions_",
            "save_menu_", "stat_menu_", "field_", "weekday_", "menu_", "snd_",
            "phonename_", "image_", "papyrus_", "audio_", "steam_", "sprite_",
            "fnt_",
            "name_entry_",
            "file0", "donate_", "system_information_", "ord(",
            "monstername_", "undertale.ini", "dr.ini", "keyconfig_", ".ini", "config.ini", "credits.txt",
            "true_config.ini", ".json",
            "Bscotch", "testlines.txt", "s_key", "_key", "_label", "shop_", "basicmonster_",
            "undertale.exe", "flowey.exe", "undertale.EXE", "Undertale.exe",
            "UNDERTALE.exe", "file1", "file2", "file3", "file4", "file5",
            "file6", "file7", "file8", "file9", "Action1", "Action2", "Action3",
            "000010000", "000001000", "000100000", "000000010", "000000001",
            "Met1", "alter2", "FloweyExplain1", "EndMet",
            "MeetLv1", "MeetLv2", "MeetLv", "KillYou", "Gameover", ".sav", "graphic_", "GMSpriteFramesTrack",
            "in_Position", "in_Colour", "in_TextureCoord", "_shot.png", ".gif", "special_name_check_",
            "_ch1.json", "u_pixelSize", "u_Uvs", "u_paletteId", "u_palTexture",
            "DEVICE_", "global.flag[",
            "zurasuon", "zurasuoff", "instancecreate",
            "globalvar", "autowalk", "autofacing", "depthobject",
            "free_all", "initplay", "initloop",
            "pitchtime", "loopsfxpitch", "loopsfxpitchtime", "loopsfxstop",
            "loopsfxvolume", "panspeed", "panobj", "pannable", "jumpinplace",
            "addxy", "arg_objectxy", "actortoobject", "actortokris", "actortocaterpillar",
            "saveload", "waitcustom", "waitdialoguer", "waitbox",
            "terminatekillactors", "krislight", "krisdark", "susielight", "susielighteyes", "susiedark",
            "susiedarkeyes", "ralseihat", "ralseinohat", "noellelight", "noelledark", "berdlylight", "berdlydark",
            "susieunhappy", "berdlyunhappy", "ralseiunhappy", "queenchair",
            "temp_save_failed", "filech",
            "enter_button_assign",
            "mainbig", "comicsans", "tinynoelle", "dotumche", "noelle_cropped", "leftmid", "rightmid", "bottommid",
            "enemytalk", "enemyattack",
            "soundplay", "walkdirect",
            "showdialog", "savepadindex", "slottitle",
            "saveslotsize", "\\n",
        };
        public static string[] BannedStringsEX = {
            "mus/", "lang/", "0123456789+-%", "0123456789-+",
            // DEVICE_NAMER
            "(B)BACK", "(E)END", "(1)ひらがな", "(2)カタカナ", "(3)アルファベット", "(B)さくじょ", "(E)けってい"
        };
        public static string[] FriskSpriteHandles = {
            // UNDERTALE
            "spr_maincharal", "spr_maincharau", "spr_maincharar", "spr_maincharad",
            "spr_maincharau_stark", "spr_maincharar_stark", "spr_maincharal_stark",
            "spr_maincharad_pranked", "spr_maincharal_pranked",
            "spr_maincharad_umbrellafall", "spr_maincharau_umbrellafall", "spr_maincharar_umbrellafall", "spr_maincharal_umbrellafall",
            "spr_maincharad_umbrella", "spr_maincharau_umbrella", "spr_maincharar_umbrella", "spr_maincharal_umbrella",
            "spr_charad", "spr_charad_fall", "spr_charar", "spr_charar_fall", "spr_charal", "spr_charal_fall", "spr_charau", "spr_charau_fall",
            "spr_maincharar_shadow", "spr_maincharal_shadow", "spr_maincharau_shadow", "spr_maincharad_shadow",
            "spr_maincharal_tomato", "spr_maincharal_burnt", "spr_maincharal_water",
            "spr_maincharar_water", "spr_maincharau_water", "spr_maincharad_water", "spr_mainchara_pourwater",
            "spr_maincharad_b", "spr_maincharau_b", "spr_maincharar_b", "spr_maincharal_b",
            "spr_doorA", "spr_doorB", "spr_doorC", "spr_doorD", "spr_doorX",
            // DELTARUNE
            "spr_krisr", "spr_krisl", "spr_krisd", "spr_krisu", "spr_kris_fall", "spr_krisr_sit",
            "spr_krisd_dark", "spr_krisr_dark", "spr_krisu_dark", "spr_krisl_dark",
            "spr_krisd_slide", "spr_krisd_slide_light",
            "spr_krisd_heart", "spr_krisd_slide_heart", "spr_krisu_heart", "spr_krisl_heart", "spr_krisr_heart",
            "spr_kris_fallen_dark", "spr_krisu_run", "spr_kris_fall_d_white", "spr_kris_fall_turnaround",
            "spr_kris_fall_d_lw", "spr_kris_fall_d_dw", "spr_kris_fall_smear", "spr_kris_dw_landed",
            "spr_kris_fall_ball", "spr_kris_jump_ball", "spr_kris_dw_land_example_dark", "spr_kris_fall_example_dark",
            "spr_krisu_fall_lw", "spr_kris_pose", "spr_kris_dance",
            "spr_kris_sword_jump", "spr_kris_sword_jump_down", "spr_kris_sword_jump_settle", "spr_kris_sword_jump_up",
            "spr_kris_coaster", "spr_kris_coaster_hurt_front", "spr_kris_coaster_hurt_back",
            "spr_kris_coaster_front", "spr_kris_coaster_empty", "spr_kris_coaster_back",
            "spr_kris_hug_left", "spr_kris_peace", "spr_kris_rude_gesture",
            "spr_kris_sit_wind", "spr_kris_hug", "spr_krisb_pirouette", "spr_krisb_bow",
            "spr_krisb_victory", "spr_krisb_defeat", "spr_krisb_attackready",
            "spr_krisb_act", "spr_krisb_actready", "spr_krisb_itemready", "spr_krisb_item",
            "spr_krisb_attack", "spr_krisb_hurt", "spr_krisb_intro", "spr_krisb_idle", "spr_krisb_defend",
            "spr_krisb_virokun", "spr_krisb_virokun_doctor", "spr_krisb_virokun_nurse", "spr_krisb_wan",
            "spr_krisb_wan_tail", "spr_krisb_wiggle",
            "spr_krisb_ready_throw_torso", "spr_krisb_ready_throw_full", "spr_krisb_throw",
            "spr_krisd_bright", "spr_krisl_bright", "spr_krisr_bright", "spr_krisu_bright",
            "spr_kris_fell",
            "spr_teacup_kris", "spr_teacup_kris_tea", "spr_teacup_kris_tea2", "spr_kris_tea",
            "spr_kris_hug_ch1",
            "spr_krisb_pirouette_ch1", "spr_krisb_bow_ch1", "spr_krisb_victory_ch1",
            "spr_krisb_defeat_ch1", "spr_krisb_attackready_ch1", "spr_krisb_act_ch1",
            "spr_krisb_actready_ch1", "spr_krisb_itemready_ch1", "spr_krisb_item_ch1",
            "spr_krisb_attack_ch1", "spr_krisb_attack_old_ch1", "spr_krisb_hurt_ch1",
            "spr_krisb_intro_ch1", "spr_krisb_idle_ch1", "spr_krisb_defend_ch1",
            "spr_kris_drop_ch1", "spr_kris_fell_ch1",
            "spr_krisr_kneel_ch1", "spr_krisd_bright_ch1", "spr_krisl_bright_ch1",
            "spr_krisr_bright_ch1", "spr_krisu_bright_ch1", "spr_krisd_heart_ch1",
            "spr_krisd_slide_heart_ch1", "spr_krisu_heart_ch1", "spr_krisl_heart_ch1",
            "spr_krisr_heart_ch1", "spr_kris_fallen_dark_ch1",
            "spr_krisd_dark_ch1", "spr_krisr_dark_ch1", "spr_krisu_dark_ch1", "spr_krisl_dark_ch1",
            "spr_krisd_slide_ch1", "spr_krisd_slide_light_ch1",
            "spr_krisr_ch1", "spr_krisl_ch1", "spr_krisd_ch1", "spr_krisu_ch1",
            "spr_krisr_sit_ch1", "spr_kris_fall_ch1",
            "spr_doorAny", "spr_doorE", "spr_doorF", "spr_doorW",
            "spr_doorE_ch1", "spr_doorF_ch1", "spr_doorW_ch1"
        };

        public static bool ShuffleChunk(UndertaleChunk chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream,
            bool friskmode, Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> shufflefunc)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            try
            {
                if (!shufflefunc(chunk, data, random, shufflechance, logstream, friskmode))
                {
                    MsgBoxHelpers.ShowError($"Error occured while modifying chuck {chunk.Name}.");
                    logstream.WriteLine($"Error occured while modifying chuck {chunk.Name}.");
                    return false;
                }
            }
            catch (Exception e)
            {
                logstream.Write($"Caught exception during modification of chuck {chunk.Name}. -> {e}");
                throw;
            }
            return true;
        }
        public static bool ShuffleChunk(UndertaleChunk chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool friskmode)
        {
            return ShuffleChunk(chunk, data, random, shufflechance, logstream, friskmode, SimpleShuffle);
        }

        enum ComplexShuffleStep : byte { Shuffling, SecondLog }

        public static Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> ComplexShuffle(
            Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> shuffler)
        {
            return (UndertaleChunk chunk, UndertaleData data, Random random, float chance, StreamWriter logstream, bool friskmode) =>
            {
                ComplexShuffleStep step = ComplexShuffleStep.Shuffling;
                try
                {
                    shuffler(chunk, data, random, chance, logstream, friskmode);
                    step = ComplexShuffleStep.SecondLog;
                    logstream.WriteLine($"Shuffled chunk {chunk.Name}.");
                }
                catch (Exception ex)
                {
                    logstream.WriteLine($"Caught exception [{ex}] while modifying chunk, during step {step}.");
                    throw;
                }

                return true;
            };
        }

        public static bool SimpleShuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool _friskMode)
        {
            var chunk = (_chunk as IUndertaleListChunk)?.GetList();
            List<int> ints = new List<int>();
            for (int i = 0; i < chunk.Count; i++)
            {
                ints.Add(i);
            }
            ints.SelectSome(shuffleChance, random);
            chunk.ShuffleOnlySelected(ints, GetSubfunction(chunk, _chunk), random);
            return true;
        }

        public static bool ShuffleAudio2_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (_chunk as UndertaleChunkSTRG)?.List;
            IList<UndertaleString> strgClone = new List<UndertaleString>(_pointerlist);
            foreach (var func in data.Sounds)
            {
                strgClone.Remove(func.Name);
                strgClone.Remove(func.Type);
                strgClone.Remove(func.File);
            }

            IList<int> stringList = new List<int>();

            for (int _i = 0; _i < strgClone.Count; _i++)
            {
                var i = _pointerlist.IndexOf(strgClone[_i]);
                string s = strgClone[_i].Content;
                if ((s.EndsWith(".ogg") || s.EndsWith(".wav") || s.EndsWith(".mp3"))
                    && !s.StartsWith("music/") /* UNDERTALE */)
                    stringList.Add(i);
            }

            stringList.SelectSome(shufflechance, random);
            logstream.WriteLine($"Added {stringList.Count} string pointers to music file references list.");

            _pointerlist.ShuffleOnlySelected(stringList, GetSubfunction(_pointerlist, _chunk), random);

            return true;
        }

        public static bool ShuffleBG2_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> chunk = (_chunk as UndertaleChunkSPRT)?.List;
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = chunk[i];

                if (pointer.Name.Content.Trim().StartsWith("bg_"))
                    sprites.Add(i);
            }

            sprites.SelectSome(shufflechance, random);
            chunk.ShuffleOnlySelected(sprites, GetSubfunction(chunk, _chunk), random);
            logstream.WriteLine($"Shuffled {sprites.Count} assumed backgrounds out of {chunk.Count} sprites.");

            return true;
        }
        public static bool ShuffleGFX_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> chunk = (_chunk as UndertaleChunkSPRT)?.List;
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = chunk[i];

                if (!pointer.Name.Content.Trim().StartsWith("bg_") &&
                    (!FriskSpriteHandles.Contains(pointer.Name.Content.Trim()) || _friskMode))
                    sprites.Add(i);
            }

            sprites.SelectSome(shufflechance, random);
            chunk.ShuffleOnlySelected(sprites, GetSubfunction(chunk, _chunk), random);
            logstream.WriteLine($"Shuffled {sprites.Count} out of {chunk.Count} sprite pointers.");

            return true;
        }

        public static bool HitboxFix_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> pointerlist = (_chunk as UndertaleChunkSPRT)?.List;
            TextureWorker worker = new TextureWorker();
            foreach (UndertaleSprite sprite in pointerlist)
            {
                // based on ImportGraphics.csx
                sprite.CollisionMasks.Clear();
                var texPageItem = sprite.Textures[0].Texture;
                // for Undertale BNP
                if (texPageItem == null)
                {
                    logstream.WriteLine($"Texture 0 of sprite {sprite.Name.Content} is null.");
                    continue;
                }
                // spr_hotlandmissle
                if (texPageItem.BoundingWidth != sprite.Width || texPageItem.BoundingHeight != sprite.Height)
                {
                    texPageItem.BoundingWidth = (ushort)sprite.Width;
                    texPageItem.BoundingHeight = (ushort)sprite.Height;
                }
                if (texPageItem.BoundingWidth < texPageItem.TargetWidth || texPageItem.BoundingHeight < texPageItem.TargetHeight)
                {
                    texPageItem.BoundingWidth = texPageItem.TargetWidth;
                    texPageItem.BoundingHeight = texPageItem.TargetHeight;
                    sprite.Width = texPageItem.BoundingWidth;
                    sprite.Height = texPageItem.BoundingHeight;
                }
                Bitmap tex;
                try
                {
                    tex = worker.GetTextureFor(texPageItem, sprite.Name.Content, true);
                }
                catch (InvalidDataException ex)
                {
                    logstream.WriteLine($"Caught InvalidDataException while modifying the hitbox of {sprite.Name.Content}. {ex.Message}");
                    continue;
                }
                int width = (((int)sprite.Width + 7) / 8) * 8;
                BitArray maskingBitArray = new BitArray(width * (int)sprite.Height);
                try
                {
                    for (int y = 0; y < sprite.Height; y++)
                    {
                        for (int x = 0; x < sprite.Width; x++)
                        {
                            Color pixelColor = tex.GetPixel(x, y);
                            maskingBitArray[y * width + x] = (pixelColor.A > 0);
                        }
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException($"{ex.Message.TrimEnd()} ({sprite.Name.Content})");
                }
                BitArray tempBitArray = new BitArray(maskingBitArray.Length);
                for (int i = 0; i < maskingBitArray.Length; i += 8)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempBitArray[j + i] = maskingBitArray[-(j - 7) + i];
                    }
                }
                UndertaleSprite.MaskEntry newEntry = new UndertaleSprite.MaskEntry();
                int numBytes;
                numBytes = maskingBitArray.Length / 8;
                byte[] bytes = new byte[numBytes];
                tempBitArray.CopyTo(bytes, 0);
                newEntry.Data = bytes;
                sprite.CollisionMasks.Add(newEntry);
            }
            logstream.WriteLine($"Wrote {pointerlist.Count} collision boxes.");

            return true;
        }

        public static bool ShuffleText_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (_chunk as UndertaleChunkSTRG)?.List;

            IList<UndertaleString> pl_test = CleanseStringList(_pointerlist, data);

            Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();
            foreach (string chr in DRChoicerControlChars)
            {
                stringDict[chr] = new List<int>();
                stringDict[chr + "_ja"] = new List<int>();
            }
            foreach (string chr in FormatChars)
            {
                stringDict[chr] = new List<int>();
                stringDict[chr + "_ja"] = new List<int>();
            }
            foreach (string chr in CategorizableForcedSubstring)
            {
                stringDict[chr] = new List<int>();
                stringDict[chr + "_ja"] = new List<int>();
            }
            stringDict["_FORCE"] = new List<int>();
            stringDict["_FORCE_ja"] = new List<int>();

            for (int _i = 0; _i < pl_test.Count; _i++)
            {
                var i = _pointerlist.IndexOf(pl_test[_i]);

                var s = _pointerlist[i];
                var convertedString = s.Content;

                if (convertedString.Length < 3 || IsStringBanned(convertedString))
                    continue;

                string ch = "";
                foreach (string chr in DRChoicerControlChars)
                {
                    if (convertedString.Contains(chr))
                    {
                        ch = chr;
                        break;
                    }
                }
                if (ch != "")
                {
                    foreach (char ix in convertedString)
                        if (ix > 127)
                        {
                            ch += "_ja";
                            break;
                        }
                    stringDict[ch].Add(i);
                }
                else
                {
                    string ending = "";
                    foreach (string ed in FormatChars)
                    {
                        if (convertedString.EndsWith(ed))
                        {
                            ending = ed;
                            if (ending.StartsWith("/%%"))
                                ending = "/%%";
                            break;
                        }
                    }
                    if (ending == "")
                    {
                        string chu = "";
                        foreach (string chr in CategorizableForcedSubstring)
                        {
                            if (convertedString.Contains(chr))
                            {
                                chu = chr;
                                break;
                            }
                        }
                        if (chu != "")
                            ending = chu;
                        else if (ForceShuffleReferenceChars.Any(convertedString.Contains))
                            ending = "_FORCE";
                        else
                            continue;
                    }
                    foreach (char ix in convertedString)
                        if (ix > 127)
                        {
                            ending += "_ja";
                            break;
                        }

                    stringDict[ending].Add(i);
                }

                if (data.IsGameMaker2())
                {
                    // Do string_hash_to_newline for good measure
                    s.Content = s.Content.Replace("#", "\n");
                    convertedString = s.Content;
                }
            }

            foreach (string ending in stringDict.Keys)
            {
                stringDict[ending].SelectSome(shufflechance, random);
                logstream.WriteLine($"Added {stringDict[ending].Count} string pointers of ending {ending} to dialogue string List.");

                _pointerlist.ShuffleOnlySelected(stringDict[ending], GetSubfunction(_pointerlist, _chunk), random);
            }

            return true;
        }

        private static IList<UndertaleString> CleanseStringList(IList<UndertaleString> pointerlist, UndertaleData data)
        {
            var pl_test = new List<UndertaleString>(pointerlist);
            pl_test.Remove(data.GeneralInfo.FileName);
            pl_test.Remove(data.GeneralInfo.Name);
            pl_test.Remove(data.GeneralInfo.DisplayName);
            pl_test.Remove(data.GeneralInfo.Config);
            foreach (var func in data.Options.Constants)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Value);
            }
            foreach (var func in data.Language.EntryIDs)
            {
                pl_test.Remove(func);
            }
            foreach (var func in data.Language.Languages)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Region);
                foreach (var j in func.Entries)
                {
                    pl_test.Remove(j);
                }
            }
            foreach (var func in data.Variables)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.Functions)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.CodeLocals)
            {
                pl_test.Remove(func.Name);
                foreach (var i in func.Locals)
                    pl_test.Remove(i.Name);
            }
            foreach (var func in data.Code)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.Rooms)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Caption);
                if (data.IsGameMaker2())
                {
                    foreach (var layer in func.Layers)
                    {
                        pl_test.Remove(layer.LayerName);
                        if (layer.EffectType != null)
                            pl_test.Remove(layer.EffectType);
                        if (layer.EffectData != null)
                        {
                            pl_test.Remove(layer.EffectData.EffectType);
                            foreach (var prop in layer.EffectData.Properties)
                            {
                                pl_test.Remove(prop.Name);
                                pl_test.Remove(prop.Value);
                            }
                        }
                        if (layer.AssetsData != null)
                        {
                            if (layer.AssetsData.Sprites != null)
                                foreach (var asset in layer.AssetsData.Sprites)
                                    pl_test.Remove(asset.Name);
                            if (layer.AssetsData.Sequences != null)
                                foreach (var asset in layer.AssetsData.Sequences)
                                    pl_test.Remove(asset.Name);
                        }
                    }
                }
            }
            foreach (var func in data.GameObjects)
            {
                pl_test.Remove(func.Name);
                foreach (var i in func.Events)
                    foreach (var ii in i)
                        foreach (var j in ii.Actions)
                            pl_test.Remove(j.ActionName);
            }
            if (data.Shaders is not null)
                foreach (var func in data.Shaders)
                {
                    pl_test.Remove(func.Name);
                    pl_test.Remove(func.GLSL_ES_Vertex);
                    pl_test.Remove(func.GLSL_ES_Fragment);
                    pl_test.Remove(func.GLSL_Vertex);
                    pl_test.Remove(func.GLSL_Fragment);
                    pl_test.Remove(func.HLSL9_Vertex);
                    pl_test.Remove(func.HLSL9_Fragment);
                    foreach (UndertaleShader.VertexShaderAttribute attr in func.VertexShaderAttributes)
                        pl_test.Remove(attr.Name);
                }
            if (data.Timelines is not null)
                foreach (var func in data.Timelines)
                {
                    pl_test.Remove(func.Name);
                }
            if (data.AnimationCurves is not null)
                foreach (var func in data.AnimationCurves)
                {
                    pl_test.Remove(func.Name);
                    foreach (var ch in func.Channels)
                        pl_test.Remove(ch.Name);
                }
            if (data.Sequences is not null)
                foreach (var func in data.Sequences)
                {
                    pl_test.Remove(func.Name);

                    foreach (KeyValuePair<int, UndertaleString> kvp in func.FunctionIDs)
                        pl_test.Remove(kvp.Value);
                    foreach (UndertaleSequence.Keyframe<UndertaleSequence.Moment> moment in func.Moments)
                        foreach (KeyValuePair<int, UndertaleSequence.Moment> kvp in moment.Channels)
                            pl_test.Remove(kvp.Value.Event);
                    foreach (UndertaleSequence.Keyframe<UndertaleSequence.BroadcastMessage> bm in func.BroadcastMessages)
                        foreach (KeyValuePair<int, UndertaleSequence.BroadcastMessage> kvp in bm.Channels)
                            foreach (UndertaleString msg in kvp.Value.Messages)
                                pl_test.Remove(msg);

                    foreach (var track in func.Tracks)
                    {
                        void loop(UndertaleSequence.Track track)
                        {
                            pl_test.Remove(track.Name);
                            pl_test.Remove(track.ModelName);
                            pl_test.Remove(track.GMAnimCurveString);
                            if (track.ModelName.Content == "GMStringTrack")
                                foreach (var i in (track.Keyframes as UndertaleSequence.StringKeyframes).List)
                                    foreach (KeyValuePair<int, UndertaleSequence.StringData> kvp in i.Channels)
                                        pl_test.Remove(kvp.Value.Value);
                            if (track.Tracks != null)
                                foreach (var subtrack in track.Tracks)
                                    loop(subtrack);
                        }
                        loop(track);
                    }
                }
            foreach (var func in data.Fonts)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.DisplayName);
            }
            foreach (var func in data.Extensions)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.FolderName);
                pl_test.Remove(func.ClassName);
                foreach (var file in func.Files)
                {
                    pl_test.Remove(file.Filename);
                    pl_test.Remove(file.InitScript);
                    pl_test.Remove(file.CleanupScript);
                    foreach (var function in file.Functions)
                    {
                        pl_test.Remove(function.Name);
                        pl_test.Remove(function.ExtName);
                    }
                }
                foreach (var function in func.Options)
                {
                    pl_test.Remove(function.Name);
                    pl_test.Remove(function.Value);
                }
            }
            foreach (var func in data.Scripts)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.Sprites)
            {
                pl_test.Remove(func.Name);
            }
            if (data.Backgrounds is not null)
                foreach (var func in data.Backgrounds)
                {
                    pl_test.Remove(func.Name);
                }
            foreach (var func in data.Paths)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.Sounds)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Type);
                pl_test.Remove(func.File);
            }
            if (data.AudioGroups is not null)
                foreach (var func in data.AudioGroups)
                {
                    pl_test.Remove(func.Name);
                }
            if (data.TextureGroupInfo is not null)
                foreach (var func in data.TextureGroupInfo)
                {
                    pl_test.Remove(func.Name);
                }
            if (data.EmbeddedImages is not null)
                foreach (var func in data.EmbeddedImages)
                {
                    pl_test.Remove(func.Name);
                }
            var feds = (data.FORM.Chunks.GetValueOrDefault("FEDS") as UndertaleChunkFEDS)?.List;
            if (feds is not null)
                foreach (var func in feds)
                {
                    pl_test.Remove(func.Name);
                    pl_test.Remove(func.Value);
                }
            return pl_test;
        }

        private static void SwapUndertaleSprite(UndertaleSprite kv, UndertaleSprite nv)
        {
            var Name_value = kv.Name;
            kv.Name = nv.Name;
            nv.Name = Name_value;
            var Width_value = kv.Width;
            kv.Width = nv.Width;
            nv.Width = Width_value;
            var Height_value = kv.Height;
            kv.Height = nv.Height;
            nv.Height = Height_value;
            var MarginLeft_value = kv.MarginLeft;
            kv.MarginLeft = nv.MarginLeft;
            nv.MarginLeft = MarginLeft_value;
            var MarginRight_value = kv.MarginRight;
            kv.MarginRight = nv.MarginRight;
            nv.MarginRight = MarginRight_value;
            var MarginTop_value = kv.MarginTop;
            kv.MarginTop = nv.MarginTop;
            nv.MarginTop = MarginTop_value;
            var MarginBottom_value = kv.MarginBottom;
            kv.MarginBottom = nv.MarginBottom;
            nv.MarginBottom = MarginBottom_value;
            var V2Sequence_value = kv.V2Sequence;
            kv.V2Sequence = nv.V2Sequence;
            nv.V2Sequence = V2Sequence_value;
            var OriginX_value = kv.OriginX;
            kv.OriginX = nv.OriginX;
            nv.OriginX = OriginX_value;
            var OriginY_value = kv.OriginY;
            kv.OriginY = nv.OriginY;
            nv.OriginY = OriginY_value;
            var GMS2PlaybackSpeed_value = kv.GMS2PlaybackSpeed;
            kv.GMS2PlaybackSpeed = nv.GMS2PlaybackSpeed;
            nv.GMS2PlaybackSpeed = GMS2PlaybackSpeed_value;
            var GMS2PlaybackSpeedType_value = kv.GMS2PlaybackSpeedType;
            kv.GMS2PlaybackSpeedType = nv.GMS2PlaybackSpeedType;
            nv.GMS2PlaybackSpeedType = GMS2PlaybackSpeedType_value;
            var SSpriteType_value = kv.SSpriteType;
            kv.SSpriteType = nv.SSpriteType;
            nv.SSpriteType = SSpriteType_value;
            var SpineJSON_value = kv.SpineJSON;
            kv.SpineJSON = nv.SpineJSON;
            nv.SpineJSON = SpineJSON_value;
            var SpineAtlas_value = kv.SpineAtlas;
            kv.SpineAtlas = nv.SpineAtlas;
            nv.SpineAtlas = SpineAtlas_value;
            var SpineTextures_value = kv.SpineTextures;
            kv.SpineTextures = nv.SpineTextures;
            nv.SpineTextures = SpineTextures_value;
            var YYSWF_value = kv.YYSWF;
            kv.YYSWF = nv.YYSWF;
            nv.YYSWF = YYSWF_value;
            var V3NineSlice_value = kv.V3NineSlice;
            kv.V3NineSlice = nv.V3NineSlice;
            nv.V3NineSlice = V3NineSlice_value;
            List<UndertaleSprite.TextureEntry> Textures_value
                = new List<UndertaleSprite.TextureEntry>(kv.Textures);
            List<UndertaleSprite.TextureEntry> Textures_valueb
                = new List<UndertaleSprite.TextureEntry>(nv.Textures);
            kv.Textures.Clear();
            nv.Textures.Clear();
            foreach (var t in Textures_valueb)
            {
                var te = new UndertaleSprite.TextureEntry();
                te.Texture = t.Texture;
                kv.Textures.Add(te);
            }
            foreach (var t in Textures_value)
            {
                var te = new UndertaleSprite.TextureEntry();
                te.Texture = t.Texture;
                nv.Textures.Add(te);
            }
            List<UndertaleSprite.MaskEntry> CollisionMasks_value
                = new List<UndertaleSprite.MaskEntry>(kv.CollisionMasks);
            List<UndertaleSprite.MaskEntry> CollisionMasks_valueb
                = new List<UndertaleSprite.MaskEntry>(nv.CollisionMasks);
            kv.CollisionMasks.Clear();
            nv.CollisionMasks.Clear();
            foreach (var t in CollisionMasks_valueb)
                kv.CollisionMasks.Add(t);
            foreach (var t in CollisionMasks_value)
                nv.CollisionMasks.Add(t);
            var Transparent_value = kv.Transparent;
            kv.Transparent = nv.Transparent;
            nv.Transparent = Transparent_value;
            var Smooth_value = kv.Smooth;
            kv.Smooth = nv.Smooth;
            nv.Smooth = Smooth_value;
            var BBoxMode_value = kv.BBoxMode;
            kv.BBoxMode = nv.BBoxMode;
            nv.BBoxMode = BBoxMode_value;
            var SepMasks_value = kv.SepMasks;
            kv.SepMasks = nv.SepMasks;
            nv.SepMasks = SepMasks_value;
        }
        private static void SwapUndertaleBackground(UndertaleBackground kv, UndertaleBackground nv)
        {
            var Name_value = kv.Name;
            kv.Name = nv.Name;
            nv.Name = Name_value;
            var Transparent_value = kv.Transparent;
            kv.Transparent = nv.Transparent;
            nv.Transparent = Transparent_value;
            var Smooth_value = kv.Smooth;
            kv.Smooth = nv.Smooth;
            nv.Smooth = Smooth_value;
            var Texture_value = kv.Texture;
            kv.Texture = nv.Texture;
            nv.Texture = Texture_value;
            var GMS2TileWidth_value = kv.GMS2TileWidth;
            kv.GMS2TileWidth = nv.GMS2TileWidth;
            nv.GMS2TileWidth = GMS2TileWidth_value;
            var GMS2TileHeight_value = kv.GMS2TileHeight;
            kv.GMS2TileHeight = nv.GMS2TileHeight;
            nv.GMS2TileHeight = GMS2TileHeight_value;
            var GMS2OutputBorderX_value = kv.GMS2OutputBorderX;
            kv.GMS2OutputBorderX = nv.GMS2OutputBorderX;
            nv.GMS2OutputBorderX = GMS2OutputBorderX_value;
            var GMS2OutputBorderY_value = kv.GMS2OutputBorderY;
            kv.GMS2OutputBorderY = nv.GMS2OutputBorderY;
            nv.GMS2OutputBorderY = GMS2OutputBorderY_value;
            var GMS2TileColumns_value = kv.GMS2TileColumns;
            kv.GMS2TileColumns = nv.GMS2TileColumns;
            nv.GMS2TileColumns = GMS2TileColumns_value;
            var GMS2FrameLength_value = kv.GMS2FrameLength;
            kv.GMS2FrameLength = nv.GMS2FrameLength;
            nv.GMS2FrameLength = GMS2FrameLength_value;
        }
        public static Action<int, int> GetSubfunction<T>(IList<T> list, UndertaleChunk chunk)
        {
            return (n, k) =>
            {
                if (list[n] is UndertaleString)
                {
                    var kv = list[k] as UndertaleString;
                    var nv = list[n] as UndertaleString;
                    var _value = kv.Content;
                    kv.Content = nv.Content;
                    nv.Content = _value;
                    return;
                }
                if (list[n] is UndertaleSprite)
                {
                    var kv = list[k] as UndertaleSprite;
                    var nv = list[n] as UndertaleSprite;
                    SwapUndertaleSprite(kv, nv);
                    return;
                }
                if (list[n] is UndertaleBackground)
                {
                    var kv = list[k] as UndertaleBackground;
                    var nv = list[n] as UndertaleBackground;
                    SwapUndertaleBackground(kv, nv);
                    return;
                }
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            };
        }
        public static Action<int, int> GetSubfunction(IList list, UndertaleChunk chunk)
        {
            return (n, k) =>
            {
                if (list[n] is UndertaleString)
                {
                    var kv = list[k] as UndertaleString;
                    var nv = list[n] as UndertaleString;
                    var _value = kv.Content;
                    kv.Content = nv.Content;
                    nv.Content = _value;
                    return;
                }
                if (list[n] is UndertaleSprite)
                {
                    var kv = list[k] as UndertaleSprite;
                    var nv = list[n] as UndertaleSprite;
                    SwapUndertaleSprite(kv, nv);
                    return;
                }
                if (list[n] is UndertaleBackground)
                {
                    var kv = list[k] as UndertaleBackground;
                    var nv = list[n] as UndertaleBackground;
                    SwapUndertaleBackground(kv, nv);
                    return;
                }
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            };
        }

        public static Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> SimpleShuffle = ComplexShuffle(SimpleShuffler);

        public static bool JSONStringShuffle(string resource_file, string target_file, UndertaleData data, Random random, float shufflechance, StreamWriter logstream)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            JObject langObject;

            using (StreamReader stream = File.OpenText(resource_file))
            {
                logstream.WriteLine($"Opened {resource_file}.");
                using (JsonTextReader jsonReader = new JsonTextReader(stream))
                {
                    var serializer = new JsonSerializer();
                    langObject = serializer.Deserialize<JObject>(jsonReader);
                }
            }
            logstream.WriteLine($"Closed {resource_file}.");

            logstream.WriteLine($"Gathered {langObject.Count} JSON String Entries. ");

            string[] bannedStrings = { "_", "||" };
            string[] bannedKeys = { "date" };

            List<KeyValuePair<string, string>> good_strings = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> final_list = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, JToken> entry in langObject)
            {
                string value = (string)entry.Value;
                if (data.IsGameMaker2())
                {
                    // Do string_hash_to_newline for good measure
                    value = value.Replace("#", "\n");
                }
                KeyValuePair<string, string> nEntry = new KeyValuePair<string, string>(entry.Key, value);
                if (!bannedKeys.Contains(entry.Key) && value.Length >= 3 && !IsStringBanned(value))
                {
                    good_strings.Add(nEntry);
                }
                else
                {
                    final_list.Add(nEntry);
                }
            }

            logstream.WriteLine($"Kept {good_strings.Count} good JSON string entries.");
            logstream.WriteLine($"Fastforwarded {final_list.Count} JSON string entries to the final phase.");

            Dictionary<string, List<KeyValuePair<string, string>>> stringDict
                = new Dictionary<string, List<KeyValuePair<string, string>>>();

            foreach (KeyValuePair<string, string> s in good_strings)
            {
                string ch = "";
                foreach (string chr in DRChoicerControlChars)
                {
                    if (s.Value.Contains(chr))
                    {
                        ch = chr;
                        break;
                    }
                }
                if (ch != "")
                {
                    if (!stringDict.ContainsKey(ch))
                        stringDict[ch] = new List<KeyValuePair<string, string>>();
                    stringDict[ch].Add(s);
                }
                else
                {
                    string ending = "";
                    foreach (string ed in FormatChars)
                    {
                        if (s.Value.EndsWith(ed))
                        {
                            ending = ed;
                            break;
                        }
                    }
                    if (!stringDict.ContainsKey(ending))
                        stringDict[ending] = new List<KeyValuePair<string, string>>();

                    stringDict[ending].Add(s);
                }
            }

            foreach (string ending in stringDict.Keys)
            {
                logstream.WriteLine($"Added {stringDict[ending].Count} JSON string entries of ending <{ending}> to dialogue string List.");

                List<int> ints = new List<int>();
                for (int i = 0; i < stringDict[ending].Count; i++)
                    ints.Add(i);
                ints.SelectSome(shufflechance, random);
                stringDict[ending].ShuffleOnlySelected<KeyValuePair<string, string>>(ints, (n, k) => {
                    KeyValuePair<string, string> tmp = stringDict[ending][n];
                    stringDict[ending][n] = stringDict[ending][k];
                    stringDict[ending][k] = tmp;
                }, random);

                final_list = final_list.Concat(stringDict[ending]).ToList();
            }

            using (FileStream out_writer = File.OpenWrite(target_file))
            {
                using (StreamWriter out_stream = new StreamWriter(out_writer))
                {
                    logstream.WriteLine($"Opened {target_file}.");
                    using (JsonTextWriter jsonReader = new JsonTextWriter(out_stream))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonReader, final_list);
                    }
                }
            }

            logstream.WriteLine($"Closed {target_file}.");

            return true;
        }

        public static bool IsStringBanned(string str)
        {
            bool bannedEX = BannedStringsEX.Any(str.Contains);
            return ((BannedStrings.Any(str.Contains) || bannedEX)
                && !(
                (ForceShuffleReferenceChars.Any(str.Contains) && !bannedEX)
                || DRChoicerControlChars.Any(str.Contains)
                )
            );
        }
    }
}