
namespace GachaAnalyzer.Core;

/// <summary>
/// 抽卡分析结果
/// </summary>
/// <param name="Z">运气Z值（正值偏欧，负值偏非）</param>
/// <param name="Mean">期望出彩数（按卡池概率的理论值）</param>
/// <param name="Std">标准差</param>
/// <param name="EffectiveC3">用于计算的有效出彩数（扣除保底兑换后）</param>
/// <param name="Grade">欧非等级评定</param>
/// <param name="Percentile">超越了百分之多少的sensei（正态累积概率）</param>
/// <param name="LowerBound95">出彩数的95%置信区间下界</param>
/// <param name="UpperBound95">出彩数的95%置信区间上界</param>
/// <param name="IsSignificant">是否显著偏离期望（|Z|≥1.96）</param>
/// <param name="SignificanceDescription">显著性中文描述</param>
public record AnalysisResult(
    double Z,
    double Mean,
    double Std,
    int EffectiveC3,
    string Grade,
    double Percentile,
    double LowerBound95,
    double UpperBound95,
    bool IsSignificant,
    string SignificanceDescription
);


public static class LuckCalculator
{
    /// <summary>
    /// 蔚蓝档案抽卡分析
    /// </summary>
    /// <param name="n">总抽数（招募次数）</param>
    /// <param name="c3">获得的3星角色总数（出彩数）</param>
    /// <param name="p">卡池单抽3星概率（常规池0.03，FES庆典池0.06）</param>
    /// <param name="usedPityExchange">是否井了？</param>
    public static AnalysisResult Analyze(int n, int c3, double p, bool usedPityExchange)
    {
        int effectiveC3 = usedPityExchange ? c3 - 1 : c3;

        double mean = n * p;
        double variance = n * p * (1 - p);
        double std = Math.Sqrt(variance);

        double z = std > 1e-12 ? (effectiveC3 - mean) / std : 0.0;

        double lower95 = mean - 1.96 * std;
        double upper95 = mean + 1.96 * std;

        bool isSignificant = Math.Abs(z) >= 1.96;
        string sigDesc = isSignificant
            ? (z > 0 ? "显著偏欧 🥇" : "显著偏非 🥀")
            : "无显著偏差";

        string grade = GradeFromZ(z);
        double percentile = NormalCdf(z);

        return new AnalysisResult(
            Z: z,
            Mean: mean,
            Std: std,
            EffectiveC3: effectiveC3,
            Grade: grade,
            Percentile: percentile,
            LowerBound95: lower95,
            UpperBound95: upper95,
            IsSignificant: isSignificant,
            SignificanceDescription: sigDesc
        );
    }

    private static string GradeFromZ(double z) => z switch
    {
        < -1.5 => "❌ 非酋",
        < -0.5 => "⚠️ 偏非",
        < 0.5 => "🎯 正常",
        < 1.5 => "🍀 欧皇",
        _ => "🌟 极欧"
    };

    private static double NormalCdf(double x)
    {
        return 0.5 * (1.0 + Erf(x / Math.Sqrt(2)));
    }

    private static double Erf(double x)
    {
        double t = 1.0 / (1.0 + 0.5 * Math.Abs(x));
        double tau = t * Math.Exp(
            -x * x - 1.26551223 +
            t * (1.00002368 +
            t * (0.37409196 +
            t * (0.09678418 +
            t * (-0.18628806 +
            t * (0.27886807 +
            t * (-1.13520398 +
            t * (1.48851587 +
            t * (-0.82215223 +
            t * 0.17087277)))))))));

        return x >= 0 ? 1.0 - tau : tau - 1.0;
    }
}