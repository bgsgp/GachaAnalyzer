using System.Globalization;
using System.Resources;

namespace GachaAnalyzer;

public static class Localization
{
    // 注意：GachaAnalyzer.Language.Resources 对应你的 Language 文件夹和 Resources.resx 文件
    private static readonly ResourceManager _rm =
        new ResourceManager("GachaAnalyzer.Language.Resources", typeof(Localization).Assembly);

    public static string GetString(string key)
    {
        // 根据当前 UI 文化（CurrentUICulture）自动选择语言
        return _rm.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }
}