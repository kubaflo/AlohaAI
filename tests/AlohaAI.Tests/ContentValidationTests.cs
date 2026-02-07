using System.Text.Json;

namespace AlohaAI.Tests;

/// <summary>
/// Tests that all content JSON files are valid and follow the expected schema.
/// These tests validate the content directory without needing a MAUI reference.
/// </summary>
public class ContentValidationTests
{
    private static readonly string ContentRoot = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "content");

    [Fact]
    public void PathsJson_IsValid()
    {
        var path = Path.Combine(ContentRoot, "paths.json");
        Assert.True(File.Exists(path), $"paths.json not found at {path}");

        var json = File.ReadAllText(path);
        var doc = JsonDocument.Parse(json);
        var paths = doc.RootElement.GetProperty("paths");
        Assert.True(paths.GetArrayLength() > 0, "paths.json should contain at least one path");

        foreach (var p in paths.EnumerateArray())
        {
            Assert.False(string.IsNullOrEmpty(p.GetProperty("id").GetString()));
            Assert.False(string.IsNullOrEmpty(p.GetProperty("title").GetString()));
            Assert.False(string.IsNullOrEmpty(p.GetProperty("description").GetString()));
            Assert.True(p.GetProperty("moduleCount").GetInt32() > 0);
        }
    }

    [Theory]
    [InlineData("agentic-ai")]
    [InlineData("ml-fundamentals")]
    [InlineData("ai-in-practice")]
    public void PathJson_HasModules(string pathId)
    {
        var pathFile = Path.Combine(ContentRoot, pathId, "path.json");
        Assert.True(File.Exists(pathFile), $"path.json not found for {pathId}");

        var json = File.ReadAllText(pathFile);
        var doc = JsonDocument.Parse(json);
        var modules = doc.RootElement.GetProperty("modules");
        Assert.True(modules.GetArrayLength() > 0, $"Path {pathId} should have at least one module");
    }

    [Theory]
    [InlineData("agentic-ai")]
    [InlineData("ml-fundamentals")]
    [InlineData("ai-in-practice")]
    public void AllModules_HaveValidStructure(string pathId)
    {
        var pathFile = Path.Combine(ContentRoot, pathId, "path.json");
        var json = File.ReadAllText(pathFile);
        var doc = JsonDocument.Parse(json);
        var modules = doc.RootElement.GetProperty("modules");

        foreach (var moduleId in modules.EnumerateArray())
        {
            var moduleDir = Path.Combine(ContentRoot, pathId, "modules", moduleId.GetString()!);
            var moduleFile = Path.Combine(moduleDir, "module.json");

            Assert.True(File.Exists(moduleFile), $"module.json missing for {pathId}/{moduleId}");

            var moduleJson = File.ReadAllText(moduleFile);
            var moduleDoc = JsonDocument.Parse(moduleJson);

            // Verify module has required fields
            Assert.False(string.IsNullOrEmpty(moduleDoc.RootElement.GetProperty("id").GetString()));
            Assert.False(string.IsNullOrEmpty(moduleDoc.RootElement.GetProperty("title").GetString()));

            // Verify lessons exist
            var lessons = moduleDoc.RootElement.GetProperty("lessons");
            Assert.True(lessons.GetArrayLength() > 0, $"Module {moduleId} should have lessons");

            foreach (var lesson in lessons.EnumerateArray())
            {
                var lessonFile = lesson.GetProperty("file").GetString()!;
                var lessonPath = Path.Combine(moduleDir, "lessons", lessonFile);
                Assert.True(File.Exists(lessonPath), $"Lesson file missing: {lessonPath}");

                var content = File.ReadAllText(lessonPath);
                Assert.True(content.Length > 100, $"Lesson {lessonFile} seems too short ({content.Length} chars)");
            }
        }
    }

    [Theory]
    [InlineData("agentic-ai")]
    [InlineData("ml-fundamentals")]
    [InlineData("ai-in-practice")]
    public void AllQuizzes_HaveValidStructure(string pathId)
    {
        var pathFile = Path.Combine(ContentRoot, pathId, "path.json");
        var json = File.ReadAllText(pathFile);
        var doc = JsonDocument.Parse(json);
        var modules = doc.RootElement.GetProperty("modules");

        foreach (var moduleId in modules.EnumerateArray())
        {
            var quizFile = Path.Combine(ContentRoot, pathId, "modules", moduleId.GetString()!, "quiz.json");

            if (!File.Exists(quizFile)) continue;

            var quizJson = File.ReadAllText(quizFile);
            var quizDoc = JsonDocument.Parse(quizJson);

            Assert.True(quizDoc.RootElement.GetProperty("passingScore").GetInt32() > 0);

            var questions = quizDoc.RootElement.GetProperty("questions");
            Assert.True(questions.GetArrayLength() > 0, $"Quiz for {moduleId} should have questions");

            foreach (var q in questions.EnumerateArray())
            {
                Assert.False(string.IsNullOrEmpty(q.GetProperty("question").GetString()));
                var options = q.GetProperty("options");
                Assert.True(options.GetArrayLength() >= 2, "Questions should have at least 2 options");
                var correctIndex = q.GetProperty("correctIndex").GetInt32();
                Assert.True(correctIndex >= 0 && correctIndex < options.GetArrayLength(),
                    $"correctIndex {correctIndex} out of range for question {q.GetProperty("id")}");
            }
        }
    }

    [Fact]
    public void TotalContentCount_IsReasonable()
    {
        var lessonFiles = Directory.GetFiles(ContentRoot, "*.md", SearchOption.AllDirectories);
        Assert.True(lessonFiles.Length >= 30, $"Expected at least 30 lessons, found {lessonFiles.Length}");

        var quizFiles = Directory.GetFiles(ContentRoot, "quiz.json", SearchOption.AllDirectories);
        Assert.True(quizFiles.Length >= 10, $"Expected at least 10 quizzes, found {quizFiles.Length}");
    }
}
