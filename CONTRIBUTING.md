# Contributing to AlohaAI

Thank you for your interest in contributing to AlohaAI! 🌺

## How to Contribute

### Adding or Improving Content

The easiest way to contribute is by adding learning content:

1. Fork the repository
2. Content lives in `content/` as JSON metadata + Markdown lessons
3. Follow the existing structure:
   - `content/<path-id>/path.json` — list of module IDs
   - `content/<path-id>/modules/<module-id>/module.json` — module metadata and lesson list
   - `content/<path-id>/modules/<module-id>/lessons/*.md` — lesson content in Markdown
   - `content/<path-id>/modules/<module-id>/quiz.json` — quiz questions
4. Submit a pull request

### Lesson Writing Guidelines

- Keep lessons **bite-sized** (300-500 words)
- Use clear **headers** (`##`, `###`) to break up content
- Include **code examples** where applicable (C# preferred)
- Add a **key takeaway** blockquote (`>`) at the end
- Use **bullet lists** for key points
- Quiz questions should test understanding, not memorization

### Code Contributions

1. Fork and clone the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Make your changes
4. Ensure the project builds: `dotnet build src/AlohaAI/AlohaAI.csproj -f net10.0-android`
5. Submit a pull request

### Code Style

- Follow standard C# conventions
- Use MVVM pattern (no framework — plain `INotifyPropertyChanged`)
- XAML files should use `x:DataType` for compiled bindings
- Keep ViewModels testable (inject services via constructor)

## Reporting Issues

Use GitHub Issues to report bugs or suggest features. Please include:

- Device/OS version
- Steps to reproduce (for bugs)
- Expected vs actual behavior

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
