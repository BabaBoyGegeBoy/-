# Comparison Video Player / 对比视频播放器

A WPF desktop application for side-by-side video comparison with synchronized and independent playback controls.

一款用于视频并排对比播放的 WPF 桌面应用，支持同步和独立播放控制。

---

## Features / 功能

### Side-by-Side Playback / 并排播放
- Two video players placed side-by-side in a single window, separated by a divider.
- 两个视频播放器并排显示在同一窗口中，中间由分隔线隔开。

### Synchronized Controls / 同步控制
- **Play / Pause / Stop / Reset** — Control both videos simultaneously.
- **Sync Progress** — Align both videos to the average playback position.
- **▶ 播放 / ⏸ 暂停 / ⏹ 停止 / ↺ 重置** — 同时控制两个视频。
- **⇄ 同步进度** — 将两个视频对齐到平均播放位置。

### Independent Controls / 独立控制
Each side has its own set of controls (tagged **Left** / **Right**):
- Play / Pause toggle
- Seek backward 5s / 30s
- Seek forward 5s / 30s
- Individual speed control (0.5x, 1.0x, 1.25x, 1.5x, 2.0x)

每一侧都有独立的控制按钮（标记为 **左** / **右**）：
- 播放/暂停切换
- 后退 5 秒 / 30 秒
- 前进 5 秒 / 30 秒
- 独立速度控制

### Playback Speed / 播放速度
- Global speed control affecting both players: 0.5x, 1.0x, 1.25x, 1.5x, 2.0x
- 全局速度控制，同时影响两个播放器

### Mouse Gestures / 鼠标手势
- **Brightness** — Scroll on the left half of a player to adjust brightness.
- **Volume** — Scroll on the right half of a player to adjust volume.
- **Progress bar** — Auto-hides; appears when hovering near the bottom.
- **亮度调节** — 在播放器左半部分滚动滚轮。
- **音量调节** — 在播放器右半部分滚动滚轮。
- **进度条** — 自动隐藏，鼠标移至底部时显示。

### Window / 窗口
- Custom borderless window with title bar
- Minimize / Maximize / Always-on-Top / Close buttons
- 自定义无边框窗口，支持最小化 / 最大化 / 置顶 / 关闭

### Supported Formats / 支持格式
MP4, AVI, MKV, MOV, WMV, FLV

---

## Tech Stack / 技术栈

- **WPF** (Windows Presentation Foundation)
- **.NET 8.0**
- Zero external dependencies / 零外部依赖

---

## Build & Run / 构建与运行

### Prerequisites / 前置要求
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Build / 构建
```bash
dotnet build -c Release
```

### Publish (Single File) / 发布（单文件）
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### Run / 运行
```bash
dotnet run
```

---

## Usage / 使用方式

1. Launch the application.
2. Click on the left player area to select the first video.
3. Click on the right player area to select the second video.
4. Use the bottom control bar for synchronized or independent playback control.
5. Use mouse wheel on each player to adjust brightness (left half) and volume (right half).

1. 启动应用。
2. 点击左侧播放区域选择第一个视频。
3. 点击右侧播放区域选择第二个视频。
4. 使用底部控制栏进行同步或独立播放控制。
5. 在播放器上使用鼠标滚轮调节亮度（左半部分）和音量（右半部分）。

---

## License / 许可证

MIT
