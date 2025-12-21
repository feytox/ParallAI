using NUnit.Framework;

namespace ParallAI.MarkdownV2.Tests;

[TestFixture]
public class MarkdownConverterTests
{
    private MarkdownV2Converter converter = null!;

    [SetUp]
    public void Setup() => converter = new MarkdownV2Converter();

    [TestCase(".", @"\.")]
    [TestCase("!", @"\!")]
    [TestCase("=", @"\=")]
    [TestCase("(", @"\(")]
    [TestCase(")", @"\)")]
    [TestCase("]", @"\]")]
    [TestCase("{", @"\{")]
    [TestCase("}", @"\}")]
    [TestCase("_", @"\_")]
    [TestCase("~", @"\~")]
    [TestCase("`", @"\`")]
    [TestCase(@"\", @"\\")]
    [TestCase("...", @"\.\.\.")]
    [TestCase("Hello. World!", @"Hello\. World\!")]
    [TestCase("C# is cool", @"C\# is cool")]
    [TestCase("email_address", @"email\_address")]
    [TestCase("(bracket content)", @"\(bracket content\)")]
    [TestCase(@"(Parentheses)", @"\(Parentheses\)")]
    [TestCase(@"[Brackets]", @"\[Brackets\]")]
    [TestCase(@"{Braces}", @"\{Braces\}")]
    [TestCase(@"C:\Windows\System32", @"C:\\Windows\\System32")]
    [TestCase("Price: $100.00", "Price: $100\\.00")]
    public void Convert_SpecialCharacters_ShouldBeEscaped(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"_ * [ ] ( ) ~ > # + - = | { } . ! \", @"\_ \* \[ \] \( \) \~ \> \# \+ \- \=  \{ \} \. \! \\")]
    public void Convert_MixedSpecialCharacters_ShouldEscapeAll(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("-", "•")]
    [TestCase("+", "•")]
    [TestCase("*", "•")]
    [TestCase("Item-1", @"Item\-1")]
    public void Convert_ListMarkers_ShouldBecomeBullets(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("#", "**")]
    [TestCase("# Hashtag", "*Hashtag*")]
    public void Convert_Headers_ShouldConvertToBoldOrItalic(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(">", "")]
    [TestCase("> Quote", ">Quote")]
    [TestCase("|", "")]
    [TestCase("|Pipe|", "Pipe")]
    [TestCase("[", "")]
    public void Convert_UnsupportedOrSpecificTags_ShouldBeStrippedOrAdapted(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("**Bold**", "*Bold*")]
    [TestCase("_Italic_", "_Italic_")]
    [TestCase("~Strike~", "~Strike~")]
    [TestCase("**Bold _Italic_**", "*Bold _Italic_*")]
    [TestCase("", "")]
    [TestCase("   ", "")]
    public void Convert_MarkdownStyles_ShouldMapToV2Styles(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("`code with . and -`", "`code with . and -`")]
    [TestCase("`print('Hello')`", "`print('Hello')`")]
    [TestCase("`var x = 1;`", "`var x = 1;`")]
    [TestCase("```\nMulti-line\nCode\n```", "```\nMulti-line\nCode\n```")]
    public void Convert_CodeBlocks_ShouldPreserveContent(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("[Google](https://google.com)", "[Google](https://google.com)")]
    [TestCase("[Wiki](https://ru.wikipedia.org/wiki/Test_Page)", "[Wiki](https://ru.wikipedia.org/wiki/Test_Page)")]
    [TestCase("https://google.com", "[https://google\\.com](https://google.com)")]
    [TestCase("user@example.com", "user@example\\.com")]
    public void Convert_LinksAndEmails_ShouldFormatCorrectly(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("$x^2 + y^3$", @"x² \+ y³")]
    [TestCase("$x_1 + x_2$", @"x₁ \+ x₂")]
    [TestCase(@"\[x^2 + y^3\]", @"x² \+ y³")]
    [TestCase(@"\[x_1 + x_2\]", @"x₁ \+ x₂")]
    [TestCase("$a^{n+1}$", @"aⁿ⁺¹")]
    [TestCase("$x_{i+1}$", @"xᵢ₊₁")]
    [TestCase(@"$x_\alpha$", @"x\_\(α\)")]
    [TestCase(@"$x^\beta$", @"xᵝ")]
    [TestCase("2 * 2 = 4", @"2 \* 2 \= 4")]
    [TestCase("2 + 2 = 4", @"2 \+ 2 \= 4")]
    [TestCase("x * y = z", @"x \* y \= z")]
    [TestCase(@"$E = mc^2$", @"E \= mc²")]
    public void Convert_MathOperations_ShouldConvertToUnicode(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\frac{1}{2}$", @"½")]
    [TestCase(@"$\frac{3}{4}$", @"¾")]
    [TestCase(@"$\frac{a}{b}$", "a/b")]
    [TestCase(@"$1\frac{1}{2}$", @"1½")]
    [TestCase(@"$\binom{n}{k}$", @"C\(n, k\)")]
    [TestCase(@"$\tbinom{n}{k}$", @"C\(n, k\)")]
    [TestCase(@"$\dbinom{n}{k}$", @"C\(n, k\)")]
    public void Convert_MathFractionsAndCombinatorics_ShouldConvertToUnicode(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\sqrt{x}$", @"√\(x\)")]
    [TestCase(@"$\sqrt{a+b}$", @"√\(a\+b\)")]
    [TestCase(@"$\sqrt[3]{x}$", @"∛\(x\)")]
    [TestCase(@"$\sqrt[4]{x}$", @"∜\(x\)")]
    public void Convert_MathRoots_ShouldConvertToUnicode(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\leq$", @"≤")]
    [TestCase(@"$\geq$", @"≥")]
    [TestCase(@"$\neq$", @"≠")]
    [TestCase(@"$\in$", @"∈")]
    [TestCase(@"$\notin$", @"∉")]
    [TestCase(@"$\alpha + \beta$", @"α \+ β")]
    [TestCase(@"$\gamma \delta \varepsilon$", @"γ δ ε")]
    public void Convert_MathSetsAndSymbols_ShouldConvertToUnicode(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathbb{N}$", "ℕ")]
    [TestCase(@"$\textbb{Z}$", "ℤ")]
    [TestCase(@"$\mathbb{QR}$", "ℚℝ")]
    public void Convert_LatexBlackboardBold_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathbf{ABC}$", "𝐀𝐁𝐂")]
    [TestCase(@"$\textbf{xyz}$", "𝐱𝐲𝐳")]
    public void Convert_LatexBold_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathcal{ABC}$", "𝓐𝓑𝓒")]
    [TestCase(@"$\textcal{L}$", "𝓛")]
    public void Convert_LatexCalligraphic_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathfrak{ABC}$", "𝔄𝔅ℭ")]
    [TestCase(@"$\textfrak{g}$", "𝔤")]
    public void Convert_LatexFraktur_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathit{abc}$", "𝑎𝑏𝑐")]
    [TestCase(@"$\textit{XYZ}$", "𝑋𝑌𝑍")]
    [TestCase(@"$\mathit{\alpha}$", "𝛼")]
    public void Convert_LatexItalic_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathtt{abc}$", "𝚊𝚋𝚌")]
    [TestCase(@"$\texttt{123}$", "𝟷𝟸𝟹")]
    public void Convert_LatexTypewriter_ShouldApplyStyle(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\mathbf{A} \cap \mathbb{B}$", "𝐀 ∩ 𝔹")]
    public void Convert_LatexMixedStyles_ShouldApplyMultipleStyles(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$\overline{AB}$", @"A̅B̅")]
    [TestCase(@"$\vec{v}$", @"v⃗")]
    [TestCase(@"$\{ab^n \mid n \in \mathbb{N}\}$", @"\{abⁿ ∣ n ∈ ℕ\}")]
    [TestCase(@"$\{aba^n \mid n \in \mathbb{N}\}$", @"\{abaⁿ ∣ n ∈ ℕ\}")]
    [TestCase(@"$\{ab^n\} \cup \{aba^n\}$", @"\{abⁿ\} ∪ \{abaⁿ\}")]
    [TestCase(@"$(Полагаем \mathbb{N} = \{1, 2, \dots\})$", @"\(Полагаем ℕ \= \{1, 2, …\}\)")]
    public void Convert_MathComplexExpressions_ShouldConvertToUnicode(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase(@"$Hello {world}$", @"Hello \{world\}")]
    [TestCase(@"${unclosed$", @"\{unclosed")]
    [TestCase(@"$\frac{1}{$", @"1/")]
    [TestCase(@"$$", @"")]
    [TestCase(@"$   $", @"")]
    public void Convert_MalformedOrEdgeCaseMath_ShouldHandleGracefully(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("| Header 1 | Header 2 |\n|---|---|\n| Cell 1 | Cell 2 |",
        "```\nHeader 1 | Header 2\nCell 1 | Cell 2\n```")]
    [TestCase("| Col 1 | Col 2 |\n| :--- | ---: |\n| Left | Right |", "```\nCol 1 | Col 2\nLeft | Right\n```")]
    public void Convert_Tables_ShouldRenderReadableFormat(string input, string expected)
        => AssertConversion(input, expected);

    [TestCase("- [ ] To do item", @"⬜ To do item")]
    [TestCase("- [x] Done item", @"✅ Done item")]
    [TestCase("- [ ] Item with **bold**", @"⬜ Item with *bold*")]
    public void Convert_TaskLists_ShouldUseDefaultIcons(string input, string expected)
        => AssertConversion(input, expected);

    [Test]
    public void Convert_WithCustomOptions_ShouldApplyCustomSettings()
    {
        var customOptions = new MarkdownOptions
        {
            HeadLevel1 = "👑",
            HeadLevel2 = "➡️",
            HeadLevel3 = "🥀",
            HeadLevel4 = "📝️",
            HeadLevel5 = "🥶",
            HeadLevel6 = "😈️",
            TaskCompleted = "[DONE]",
            TaskUncompleted = "[TODO]",
        };

        var customConverter = new MarkdownV2Converter(customOptions);

        var h1Result = customConverter.Convert("# King");
        Assert.That(h1Result, Is.EqualTo("👑 *King*"));

        var h2Result = customConverter.Convert("## Next");
        Assert.That(h2Result, Is.EqualTo("➡️ *Next*"));

        var h3Result = customConverter.Convert("### Next");
        Assert.That(h3Result, Is.EqualTo("🥀 *Next*"));

        var h4Result = customConverter.Convert("#### Next");
        Assert.That(h4Result, Is.EqualTo("📝️ *Next*"));

        var h5Result = customConverter.Convert("##### Next");
        Assert.That(h5Result, Is.EqualTo("🥶 *Next*"));

        var h6Result = customConverter.Convert("###### Next");
        Assert.That(h6Result, Is.EqualTo("😈️ *Next*"));

        var tasksResult = customConverter.Convert("- [ ] Work\n- [x] Sleep");
        Assert.That(tasksResult, Does.Contain("[TODO] Work"));
        Assert.That(tasksResult, Does.Contain("[DONE] Sleep"));
    }

    private void AssertConversion(string input, string expected)
    {
        var actual = converter.Convert(input);
        Assert.That(actual, Is.EqualTo(expected), $"Failed to convert input: '{input}'");
    }
}