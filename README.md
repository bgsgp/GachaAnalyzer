# 蔚蓝档案 · 抽卡运气分析器 (GachaAnalyzer)

[![.NET](https://img.shields.io/badge/.NET-10-blue)](https://dotnet.microsoft.com/) [![C#](https://img.shields.io/badge/C%23-14-purple)](https://docs.microsoft.com/en-us/dotnet/csharp/) [![WinForms](https://img.shields.io/badge/WinForms-.NET%2010-blueviolet)](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

基于 **标准正态 Z-score** 的蔚蓝档案抽卡运气分析工具，用统计学告诉你：自己到底是“欧皇”还是“非酋”。

---

## ✨ 功能简介

输入你在《蔚蓝档案》中的实际抽卡记录，工具会计算：

- 🍀 **运气 Z 值**（正值偏欧，负值偏非）
- 📊 **欧非等级**（❌ 非酋 → 🌟 极欧，共五档）
- 📈 **95% 置信区间**（你的出彩数是否落在理论期望范围内）
- 🎯 **显著性检验**（是否显著偏欧 / 偏非）
- 🎓 **百分位排名**（超越了多少比例的老师）

---

## 🎰 BA 抽卡规则速览

| 卡池类型   | 3星总概率 | 当期UP概率 | 备注                   |
| ------ | ----- | ------ | -------------------- |
| 常规池    | 3%    | 0.7%   | 无额外保底，200抽可天井兑换      |
| FES庆典池 | 6%    | 0.7%   | 3星率翻倍，但UP率不变         |
| Pick Up池    | --   | 0.7%   | 支持任意概率（如 0.7%、1%、2%） |

- **无软保底**（抽数不会提高概率），唯一的硬保底是 **200抽天井兑换** 当期UP。  
- 如果使用了天井兑换，工具会自动扣除一个保底 3星，计算“纯运气”部分。

---

## 🖥️ 技术栈

- **.NET 10** (net10.0-windows)
- **C# 14**
- **Windows Forms** (WinForms)
- **高 DPI 支持**：已显式禁用 DPI 缩放，避免界面模糊（可在 `Program.cs` 中调整）

---

## 🚀 快速开始

### 环境要求

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2022 (17.4+) 或 VS Code / Rider

### 运行步骤

```bash
git clone https://github.com/你的用户名/GachaAnalyzer.git
cd GachaAnalyzer
dotnet run
```

或直接在 Visual Studio 中打开 `GachaAnalyzer.csproj`，按 F5 运行。

### 输入说明

| 字段          | 说明                           |
| ----------- | ---------------------------- |
| 总抽数         | 你投入的招募次数（整数）                 |
| 出彩数(3星)     | 实际获得的3星角色总数（含兑换的）            |
| 卡池          | 常规池 (3%) / FES庆典池 (6%) / 自定义 |
| 使用了200抽天井兑换 | 若勾选，会从总出彩数中扣除1个3星再分析         |

---

## 📊 统计原理

- 假设抽卡服从 **二项分布 B(n, p)**，用正态近似计算 **Z-score**。
- Z = (实际有效出彩数 - 期望出彩数) / 标准差
- 等级划分基于 Z 值区间：
  - Z < -1.5 → ❌ 非酋
  - -1.5 ≤ Z < -0.5 → ⚠️ 偏非
  - -0.5 ≤ Z < 0.5 → 🎯 正常
  - 0.5 ≤ Z < 1.5 → 🍀 欧皇
  - Z ≥ 1.5 → 🌟 极欧
- 百分位通过标准正态累积分布函数（CDF）近似计算。

---

## 📂 项目结构

```
GachaAnalyzer/
├── Program.cs                 # 入口（禁用高DPI）
├── MainForm.cs                # 主界面逻辑与输入校验
├── MainForm.Designer.cs       # 界面布局代码
├── Core/
│   └── LuckCalculator.cs      # 核心算法与结果记录
└── GachaAnalyzer.csproj       # 项目文件 (.NET 10)
```

---

## 🛠️ 自定义与扩展

- **调整 DPI 设置**：修改 `Program.cs` 中的 `Application.SetHighDpiMode(HighDpiMode.DpiUnaware)` 可恢复高 DPI 感知。
- **添加更多卡池**：在 `MainForm.Designer.cs` 的 `cmbPool.Items` 中增加概率选项。
- **历史记录**：后续可扩展 SQLite 存储抽卡记录，绘制欧非曲线。

---

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！  
如果你有好的想法（如可视化曲线、多游戏支持），可以直接 fork 或联系作者。

---

## 📄 许可证

本项目采用 [MIT License](LICENSE)。

---

> 🎓 统计学告诉你：即使是 6% 的 FES 池，非洲老师依然需要准备 200 抽天井。祝各位老师一发入魂！  
> *“概率不会背叛你，它只负责执行。”* 😼
