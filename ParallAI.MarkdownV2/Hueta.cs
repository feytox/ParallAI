using NUnit.Framework;

namespace ParallAI.MarkdownV2;

[TestFixture]
public class VisualConverterTests
{
    private MarkdownV2Converter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new MarkdownV2Converter();
    }

    /// <summary>
    /// Вспомогательный метод для красивого вывода в консоль теста
    /// </summary>
    private void RunVisualTest(string testName, string inputMarkdown)
    {
        Console.WriteLine($"════════════════════════════════════════════════════");
        Console.WriteLine($"🧪 TEST: {testName}");
        Console.WriteLine($"════════════════════════════════════════════════════");
        Console.WriteLine("📥 [INPUT MARKDOWN]:");
        Console.WriteLine(inputMarkdown);
        Console.WriteLine("────────────────────────────────────────────────────");

        // Запускаем конвертацию
        var sentElements = _converter.Convert(inputMarkdown);

        Console.WriteLine($"📤 [OUTPUT TELEGRAM MARKDOWN V2] ({sentElements.Count} messages):");
        for (int i = 0; i < sentElements.Count; i++)
        {
            if (i > 0) Console.WriteLine("\n--- Split ---");
            Console.WriteLine(sentElements[i].Text);
        }

        Console.WriteLine($"════════════════════════════════════════════════════\n\n");
    }

    [Test]
    public void Test_Latex_Equations()
    {
        var input = @"

";
        RunVisualTest("LaTeX & Math", input);
    }
}