using System;
using System.Windows.Forms;
using GachaAnalyzer.Core;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GachaAnalyzer;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        btnCalc.Click += BtnCalc_Click;
    }

    private void BtnCalc_Click(object? sender, EventArgs e)
    {
        ClearResults();

        if (!int.TryParse(txtPulls.Text, out int n) || n <= 0)
        {
            ShowError("请输入有效的总抽数");
            return;
        }

        if (!int.TryParse(txtC3.Text, out int c3) || c3 < 0)
        {
            ShowError("请输入有效的出彩数");
            return;
        }

        string poolStr = cmbPool.SelectedItem as string ?? "";
        double p;

        var m = Regex.Match(poolStr, @"([0-9]+(?:[.,][0-9]+)?)\s*%");
        if (m.Success)
        {
            var numStr = m.Groups[1].Value.Replace(',', '.'); // 兼容逗号小数分隔符
            if (!double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double pct))
            {
                ShowError("无法解析卡池概率");
                return;
            }
            p = pct / 100.0;
        }
        else
        {
            var map = new Dictionary<string, double>
            {
                { "常规池", 0.03 },
                { "庆典池", 0.06 }
            };
            if (!map.TryGetValue(poolStr, out p))
            {
                ShowError("请选择一个带概率的卡池或联系开发者");
                return;
            }
        }
        bool usedExchange = chkPityExchange.Checked;

        if (usedExchange && c3 < 1)
        {
            ShowError("如果井了，出彩数至少为1（含井）");
            return;
        }

        var result = LuckCalculator.Analyze(n, c3, p, usedExchange);

        lblZ.Text = $"Z值: {result.Z:F3}";
        lblGrade.Text = $"评级: {result.Grade}";
        lblPercentile.Text = $"超过: {result.Percentile * 100:F1}% 的sensei";
        lblCI.Text = $"95% 区间: [{result.LowerBound95:F2}, {result.UpperBound95:F2}]";
        lblSignificance.Text = $"显著性: {result.SignificanceDescription}";
    }

    private void ClearResults()
    {
        lblZ.Text = "Z值: --";
        lblGrade.Text = "评级: --";
        lblPercentile.Text = "超过: --% 的sensei";
        lblCI.Text = "95% CI: [-- , --]";
        lblSignificance.Text = "显著性: --";
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}