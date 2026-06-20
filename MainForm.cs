using System;
using System.Windows.Forms;
using GachaAnalyzer.Core;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;  // 如果之前没有，需要加上

namespace GachaAnalyzer;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        // 加载所有界面文字（多语言）
        ApplyResources();

        btnCalc.Click += BtnCalc_Click;
    }

    /// <summary>
    /// 从资源文件读取并刷新所有界面控件的文本
    /// </summary>
    private void ApplyResources()
    {
        // 窗体标题
        this.Text = Localization.GetString("FormTitle");

        // 标签
        lblPulls.Text = Localization.GetString("LabelPulls");
        lblC3.Text = Localization.GetString("LabelC3");
        lblPool.Text = Localization.GetString("LabelPool");

        // 复选框
        chkPityExchange.Text = Localization.GetString("CheckPity");

        // 按钮
        btnCalc.Text = Localization.GetString("ButtonCalc");

        // 结果分组框
        grpResult.Text = Localization.GetString("GroupResult");

        // 下拉框选项（先清空，再按当前语言添加）
        cmbPool.Items.Clear();
        cmbPool.Items.Add(Localization.GetString("ComboNormal"));
        cmbPool.Items.Add(Localization.GetString("ComboFES"));
        cmbPool.Items.Add(Localization.GetString("ComboPickup"));
        cmbPool.SelectedIndex = 0;

        // 清空结果显示（使用资源前缀）
        ClearResults();
    }

    private void ClearResults()
    {
        lblZ.Text = Localization.GetString("LabelZ") + " --";
        lblGrade.Text = Localization.GetString("LabelGrade") + " --";
        lblPercentile.Text = Localization.GetString("LabelPercentile") + " --";
        lblCI.Text = Localization.GetString("LabelCI") + " [-- , --]";
        lblSignificance.Text = Localization.GetString("LabelSignificance") + " --";
    }

    private void BtnCalc_Click(object? sender, EventArgs e)
    {
        ClearResults();

        // 验证总抽数
        if (!int.TryParse(txtPulls.Text, out int n) || n <= 0)
        {
            ShowError("ErrorInvalidPulls");
            return;
        }

        // 验证出彩数
        if (!int.TryParse(txtC3.Text, out int c3) || c3 < 0)
        {
            ShowError("ErrorInvalidC3");
            return;
        }

        // 从下拉框当前选中项提取概率（统一用正则）
        string poolStr = cmbPool.SelectedItem as string ?? "";
        double p;

        var m = Regex.Match(poolStr, @"([0-9]+(?:[.,][0-9]+)?)\s*%");
        if (m.Success)
        {
            var numStr = m.Groups[1].Value.Replace(',', '.');
            if (!double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double pct))
            {
                ShowError("ErrorPoolParse");
                return;
            }
            p = pct / 100.0;
        }
        else
        {
            // 如果正则意外失败（理论上不会），给出友好提示
            ShowError("ErrorPoolSelect");
            return;
        }

        bool usedExchange = chkPityExchange.Checked;

        if (usedExchange && c3 < 1)
        {
            ShowError("ErrorPityWithZero");
            return;
        }

        // 调用核心计算
        var result = LuckCalculator.Analyze(n, c3, p, usedExchange);

        // 使用资源文件中的格式化字符串（带占位符）
        lblZ.Text = string.Format(Localization.GetString("FormatZ"), result.Z);
        lblGrade.Text = string.Format(Localization.GetString("FormatGrade"), result.Grade);
        lblPercentile.Text = string.Format(Localization.GetString("FormatPercentile"), result.Percentile * 100);
        lblCI.Text = string.Format(Localization.GetString("FormatCI"), result.LowerBound95, result.UpperBound95);
        lblSignificance.Text = string.Format(Localization.GetString("FormatSignificance"), result.SignificanceDescription);
    }

    /// <summary>
    /// 显示错误消息（从资源读取内容）
    /// </summary>
    /// <param name="key">资源键名</param>
    private void ShowError(string key)
    {
        string msg = Localization.GetString(key);
        string title = Localization.GetString("ErrorTitle") ?? "提示"; // 可选的错误标题
        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}