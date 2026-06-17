using System.Drawing;
using System.Windows.Forms;

namespace GachaAnalyzer;

partial class MainForm
{
    private TableLayoutPanel tableLayout;
    private Label lblTitle;
    private Label lblPulls, lblC3, lblPool;
    private TextBox txtPulls, txtC3;
    private ComboBox cmbPool;
    private CheckBox chkPityExchange;
    private Button btnCalc;
    private GroupBox grpResult;
    private Label lblZ, lblGrade, lblPercentile, lblCI, lblSignificance;

    private void InitializeComponent()
    {
        this.Text = "蔚蓝档案 · 抽卡运气分析器";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new Size(440, 460);
        this.Font = new Font("Microsoft YaHei UI", 9F);

        tableLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 9,
            Padding = new Padding(15),
            AutoSize = true
        };
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        // 标题
        lblTitle = new Label
        {
            Text = "🎰 蔚蓝档案 抽卡运气分析",
            Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        tableLayout.Controls.Add(lblTitle, 0, 0);
        tableLayout.SetColumnSpan(lblTitle, 2);

        // 抽数
        lblPulls = new Label { Text = "总抽数:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight };
        txtPulls = new TextBox { Dock = DockStyle.Left, Width = 130 };
        tableLayout.Controls.Add(lblPulls, 0, 1);
        tableLayout.Controls.Add(txtPulls, 1, 1);

        // 出彩数（3星）
        lblC3 = new Label { Text = "出彩数(3星):", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight };
        txtC3 = new TextBox { Dock = DockStyle.Left, Width = 130 };
        tableLayout.Controls.Add(lblC3, 0, 2);
        tableLayout.Controls.Add(txtC3, 1, 2);

        // 卡池类型（概率）
        lblPool = new Label { Text = "卡池:", Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleRight };
        cmbPool = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Dock = DockStyle.Left,
            Width = 130
        };
        cmbPool.Items.AddRange(new object[]
        {
            "常规池 (3%)",
            "FES庆典池 (6%)",
            "自定义 0.7%",
            "自定义 1%",
            "自定义 2%"
        });
        cmbPool.SelectedIndex = 0; // 默认常规池
        tableLayout.Controls.Add(lblPool, 0, 3);
        tableLayout.Controls.Add(cmbPool, 1, 3);

        // 天井兑换选项
        chkPityExchange = new CheckBox
        {
            Text = "使用了200抽天井兑换当期UP（扣除一个保底3星）",
            Dock = DockStyle.Left,
            AutoSize = true
        };
        tableLayout.Controls.Add(chkPityExchange, 1, 4);
        tableLayout.SetColumnSpan(chkPityExchange, 1);

        // 计算按钮
        btnCalc = new Button
        {
            Text = "🔮 分析运气",
            Dock = DockStyle.Fill,
            Height = 38,
            Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
            BackColor = Color.LightSteelBlue,
            FlatStyle = FlatStyle.Flat
        };
        btnCalc.FlatAppearance.BorderSize = 0;
        tableLayout.Controls.Add(btnCalc, 0, 5);
        tableLayout.SetColumnSpan(btnCalc, 2);

        // 结果分组
        grpResult = new GroupBox
        {
            Text = "📊 分析结果",
            Dock = DockStyle.Fill,
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold)
        };
        var resultLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            AutoSize = true
        };
        grpResult.Controls.Add(resultLayout);

        lblZ = new Label { Text = "Z值: --", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        lblGrade = new Label { Text = "评级: --", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        lblPercentile = new Label { Text = "超过: --% 的老师", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        lblCI = new Label { Text = "95% CI: [-- , --]", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
        lblSignificance = new Label { Text = "显著性: --", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };

        resultLayout.Controls.Add(lblZ, 0, 0);
        resultLayout.Controls.Add(lblGrade, 0, 1);
        resultLayout.Controls.Add(lblPercentile, 0, 2);
        resultLayout.Controls.Add(lblCI, 0, 3);
        resultLayout.Controls.Add(lblSignificance, 0, 4);

        tableLayout.Controls.Add(grpResult, 0, 6);
        tableLayout.SetColumnSpan(grpResult, 2);
        tableLayout.SetRowSpan(grpResult, 3);

        this.Controls.Add(tableLayout);
        this.AcceptButton = btnCalc;
    }
}