# 🌺 AlohaAI

**Learn Agentic AI, Machine Learning & AI concepts — one bite-sized lesson at a time.**

AlohaAI is an open-source mobile learning app built with .NET MAUI that teaches AI concepts through interactive lessons, quizzes, progress tracking, and daily learning streaks — inspired by [Enki](https://www.enki.com).

## 🎯 Learning Paths

### 🤖 Agentic AI
- Introduction to AI Agents
- Model Context Protocol (MCP)
- Retrieval-Augmented Generation (RAG)
- Skills & Tools
- Agent Orchestration

### 🧠 ML Fundamentals
- What is Machine Learning
- Supervised Learning
- Neural Networks
- LLMs & Foundation Models

### 🚀 AI in Practice
- Cloud AI Services (Azure AI, Cognitive Services)
- Semantic Kernel & AI frameworks
- Copilot Ecosystem & AI-assisted development

## ✨ Features

- 📖 **Bite-sized lessons** — Learn in 5-minute sessions with Markdown-rendered content
- 🧩 **Interactive quizzes** — Test your knowledge with A/B/C/D indicators and styled feedback
- 🔥 **Learning streaks** — Track consecutive learning days with daily goals
- 📊 **Progress tracking** — Per-path progress breakdown on your profile
- ⭐ **XP & levels** — Earn experience points for completing lessons and quizzes
- 🏆 **Achievements** — Unlock milestones as you learn (First Steps, Curious Mind, Week Warrior, etc.)
- 🔍 **Search with filters** — Find lessons with path-based filter chips and browse topics
- 🎬 **Smooth animations** — Page transitions, XP popups, quiz feedback, onboarding carousel
- 🌺 **Multi-step onboarding** — Beautiful 4-screen carousel with page indicators and slide animations
- 🌴 **Tropical sunset theme** — Dark mode with gradient headers and vibrant accent colors
- 📱 **Offline-first** — All content embedded, no internet required
- 📳 **Haptic feedback** — Tactile responses on quiz answers and lesson completion
- 🔓 **100% open source** — MIT licensed, community contributions welcome

## 🛠️ Tech Stack

| Component | Technology |
|---|---|
| Framework | .NET 10 MAUI |
| Language | C# |
| Architecture | MVVM (plain, no framework) |
| Navigation | Shell (TabBar + route-based) |
| Local Storage | SQLite (sqlite-net-pcl) |
| Content Format | JSON + Markdown |
| Platforms | Android 7.0+ / iOS 16+ |

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MAUI workload](https://learn.microsoft.com/dotnet/maui/get-started/installation): `dotnet workload install maui`
- Android SDK or Xcode (for iOS)

### Build & Run

```bash
# Clone the repo
git clone https://github.com/kubaflo/AlohaAI.git
cd AlohaAI

# Restore packages
dotnet restore

# Run on Android
dotnet build src/AlohaAI/AlohaAI.csproj -f net10.0-android -t:Run

# Run on iOS (macOS only)
dotnet build src/AlohaAI/AlohaAI.csproj -f net10.0-ios -t:Run
```

## 📁 Project Structure

```
AlohaAI/
├── src/AlohaAI/          # .NET MAUI app
│   ├── Models/           # Data models (UserProgress, LearningPath, etc.)
│   ├── ViewModels/       # MVVM view models (Home, Paths, Search, etc.)
│   ├── Views/            # XAML pages (8 pages + onboarding)
│   ├── Services/         # ContentService, ProgressService, StreakService
│   ├── Converters/       # Value converters
│   ├── Helpers/          # MarkdownRenderer (tables, code blocks, etc.)
│   └── Resources/        # Fonts, images, styles
├── content/              # Learning content (JSON + Markdown)
│   ├── agentic-ai/       # Agentic AI learning path (5 modules, 18 lessons)
│   ├── ml-fundamentals/  # ML Fundamentals path (4 modules, 12 lessons)
│   └── ai-in-practice/   # AI in Practice path (3 modules, 12 lessons)
└── tests/                # Unit tests (content validation)
```

## 📱 App Overview

| Home | Search | Profile |
|---|---|---|
| Dashboard with streak, XP, daily goals | Real-time search across all lessons | Per-path progress breakdown |

**42 lessons** • **12 quizzes** • **3 learning paths** • **Fully offline**

## 🎨 Design

AlohaAI features a **tropical sunset dark theme** inspired by Hawaii's vibrant landscapes:

- **Gradient headers** — Sky blue → purple → deep navy (`#5B9FE6` → `#7B68AE` → `#2D1B69`)
- **Dark mode default** — Deep purple-tinted surfaces for reduced eye strain
- **Accent colors** — Hibiscus pink (`#E88BBF`), warm gold XP (`#FFD54F`), sunset streak (`#F5A623`)
- **Rounded cards** — Modern card-based layout with subtle elevation

## 🤝 Contributing

Contributions are welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

### Adding Content

Lessons are written in Markdown with JSON metadata. See `content/` for the structure. PRs for new lessons, corrections, or translations are especially welcome.

## 📄 License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.

---

*Built with 🌺 and .NET MAUI*
