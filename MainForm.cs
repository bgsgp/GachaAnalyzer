using System;
using System.Windows.Forms;
using GachaAnalyzer.Core;

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

        // 抽数验证
        if (!int.TryParse(txtPulls.Text, out int n) || n <= 0)
        {
            ShowError("请输入有效的正整数总抽数");
            return;
        }

        // 出彩数验证
        if (!int.TryParse(txtC3.Text, out int c3) || c3 < 0)
        {
            ShowError("请输入有效的非负整数出彩数");
            return;
        }

        if (cmbPool.SelectedItem is not string poolStr)
        {
            ShowError("请选择一个卡池");
            return;
        }

        // 解析概率（从文本中提取数字）
        double p = double.Parse(poolStr.Split(' ')[0].TrimEnd('%')) / 100.0;
        bool usedExchange = chkPityExchange.Checked;

        // 天井逻辑校验：使用了兑换则至少有一个3星
        if (usedExchange && c3 < 1)
        {
            ShowError("若使用了天井兑换，出彩数至少为1（含兑换的那个3星）");
            return;
        }

        var result = LuckCalculator.Analyze(n, c3, p, usedExchange);

        lblZ.Text = $"Z值: {result.Z:F3}";
        lblGrade.Text = $"评级: {result.Grade}";
        lblPercentile.Text = $"超过: {result.Percentile * 100:F1}% 的老师";
        lblCI.Text = $"95% 区间: [{result.LowerBound95:F2}, {result.UpperBound95:F2}]";
        lblSignificance.Text = $"显著性: {result.SignificanceDescription}";
    }

    private void ClearResults()
    {
        lblZ.Text = "Z值: --";
        lblGrade.Text = "评级: --";
        lblPercentile.Text = "超过: --% 的老师";
        lblCI.Text = "95% CI: [-- , --]";
        lblSignificance.Text = "显著性: --";
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}