using NUnit.Framework;
using ParallAI.MarkdownV2;

namespace ParallAI.MarkdownV2.Tests;

[TestFixture]
public class MarkdownConverterTests
{
    private MarkdownV2Converter converter;

    [SetUp]
    public void Setup()
    {
        converter = new MarkdownV2Converter();
    }
    
    [TestCase(".", @"\.")] 
    [TestCase("!", @"\!")] 
    [TestCase("-", "•")] 
    [TestCase("+", "•")] 
    [TestCase("=", @"\=")] 
    [TestCase("#", "**")] 
    [TestCase("(", @"\(")] 
    [TestCase(")", @"\)")] 
    [TestCase("[", "")] 
    [TestCase("]", @"\]")] 
    [TestCase("{", @"\{")] 
    [TestCase("}", @"\}")] 
    [TestCase("_", @"\_")]
    [TestCase("*", "•")]
    [TestCase("~", @"\~")]
    [TestCase(">", "")]
    [TestCase("|", "")]
    [TestCase("`", @"\`")]
    [TestCase(@"\", @"\\")]
    [TestCase("2 * 2 = 4", @"2 \* 2 \= 4")]
    [TestCase("Hello. World!", @"Hello\. World\!")]
    [TestCase("C# is cool", @"C\# is cool")]
    [TestCase("email_address", @"email\_address")]
    [TestCase("(bracket content)", @"\(bracket content\)")]
    [TestCase(
        @"_ * [ ] ( ) ~ > # + - = | { } . ! \", 
        @"\_ \* \[ \] \( \) \~ \> \# \+ \- \=  \{ \} \. \! \\"
    )]
    [TestCase("(Parentheses)", @"\(Parentheses\)")]
    [TestCase("[Brackets]", @"\[Brackets\]")]
    [TestCase("{Braces}", @"\{Braces\}")]
    [TestCase("# Hashtag", "*Hashtag*")] 
    [TestCase("> Quote", ">Quote")]
    [TestCase("|Pipe|", "Pipe")]
    [TestCase("Price: $100.00", "Price: $100\\.00")]
    [TestCase("x * y = z", @"x \* y \= z")]
    [TestCase(@"C:\Windows\System32", @"C:\\Windows\\System32")]
    [TestCase("`code with . and -`", "`code with . and -`")]
    [TestCase("`print('Hello')`", "`print('Hello')`")]
    [TestCase("```\nMulti-line\nCode\n```", "```\nMulti-line\nCode\n```")]
    public void Convert_AllSpecialCharacters_AreEscaped(string inputMarkdown, string expectedV2)
    {
        var resultList = converter.Convert(inputMarkdown);
        var actualText = string.Join("", resultList.Select(x => x.Text));
        
        Assert.That(actualText, Is.EqualTo(expectedV2));
    }

    [TestCase("Hello world.", @"Hello world\.")]
    [TestCase("**Bold**", "*Bold*")]
    [TestCase("_Italic_", "_Italic_")]
    [TestCase("~Strike~", "~Strike~")]
    [TestCase("2 + 2 = 4", @"2 \+ 2 \= 4")]
    [TestCase("Item-1", @"Item\-1")]
    [TestCase("", "")]
    [TestCase("   ", "")]
    [TestCase("...", @"\.\.\.")]
    [TestCase("**Bold _Italic_**", "*Bold _Italic_*")]
    public void Convert_BasicFormatting_ReturnsExpected(string inputMarkdown, string expectedV2)
    {
        var resultList = converter.Convert(inputMarkdown);
        var actualText = string.Join("", resultList.Select(x => x.Text));
        
        Assert.That(actualText, Is.EqualTo(expectedV2));
    }
    
    [TestCase(@"$x^2 + y^3$", @"x² \+ y³")]
    [TestCase(@"$x_1 + x_2$", @"x₁ \+ x₂")]

    [TestCase(@"$\frac{1}{2}$", @"½")]
    [TestCase(@"$\frac{3}{4}$", @"¾")]
    [TestCase(@"$\frac{a}{b}$", @"a/b")]
    [TestCase(@"$1\frac{1}{2}$", @"1½")]

    [TestCase(@"$\sqrt{x}$", @"√\(x\)")]
    [TestCase(@"$\sqrt{a+b}$", @"√\(a\+b\)")]
    [TestCase(@"$\sqrt[3]{x}$", @"∛\(x\)")]
    [TestCase(@"$\sqrt[4]{x}$", @"∜\(x\)")]

    [TestCase(@"$\alpha + \beta$", @"α \+ β")]
    [TestCase(@"$\gamma \delta \varepsilon$", @"γ δ ε")]

    [TestCase(@"$\binom{n}{k}$", @"C\(n, k\)")]
    [TestCase(@"$\tbinom{n}{k}$", @"C\(n, k\)")]
    [TestCase(@"$\dbinom{n}{k}$", @"C\(n, k\)")]

    [TestCase(@"$\mathbb{N}$", @"ℕ")]
    [TestCase(@"$\mathbb{Z}$", @"ℤ")]
    [TestCase(@"$\mathbb{R}$", @"ℝ")]

    [TestCase(@"$\leq$", @"≤")]
    [TestCase(@"$\geq$", @"≥")]
    [TestCase(@"$\neq$", @"≠")]
    [TestCase(@"$\in$", @"∈")]
    [TestCase(@"$\notin$", @"∉")]

    [TestCase(@"$\{ab^n \mid n \in \mathbb{N}\}$", @"\{abⁿ ∣ n ∈ ℕ\}")]
    [TestCase(@"$\{aba^n \mid n \in \mathbb{N}\}$", @"\{abaⁿ ∣ n ∈ ℕ\}")]
    [TestCase(@"$\{ab^n\} \cup \{aba^n\}$", @"\{abⁿ\} ∪ \{abaⁿ\}")]

    [TestCase(@"$(Полагаем \mathbb{N} = \{1, 2, \dots\})$", @"\(Полагаем ℕ \= \{1, 2, …\}\)")]

    [TestCase(@"$E = mc^2$", @"E \= mc²")]
    [TestCase(@"$a^{n+1}$", @"aⁿ⁺¹")]
    [TestCase(@"$x_{i+1}$", @"xᵢ₊₁")]

    [TestCase(@"$x_\alpha$", @"x\_\(α\)")]
    [TestCase(@"$x^\beta$", @"xᵝ")]

    [TestCase(@"$\overline{AB}$", @"A̅B̅")]
    [TestCase(@"$\vec{v}$", @"v⃗")]

    [TestCase(@"$Hello {world}$", @"Hello \{world\}")]
    [TestCase(@"${unclosed$", @"\{unclosed")]
    [TestCase(@"$\frac{1}{$", @"1/")]

    [TestCase(@"$$", @"")]
    [TestCase(@"$   $", @"")]

    public void Convert_LatexMath_ReturnsUnicodeEscaped(string inputMarkdown, string expectedV2)
    {
        var resultList = converter.Convert(inputMarkdown);
        var actualText = string.Join("", resultList.Select(x => x.Text));
        
        Assert.That(actualText, Is.EqualTo(expectedV2));
    }

    [Test]
    [TestCase("[Google](https://google.com)", "[Google](https://google.com)")]
    [TestCase(
        "[Wiki](https://ru.wikipedia.org/wiki/Test_Page)", 
        "[Wiki](https://ru.wikipedia.org/wiki/Test_Page)"
    )]
    [TestCase("`var x = 1;`", "`var x = 1;`")]
    [TestCase("https://google.com", "[https://google\\.com](https://google.com)")]
    [TestCase("[Google](https://google.com)", "[Google](https://google.com)")]
    [TestCase("user@example.com", "user@example\\.com")]
    public void Convert_ComplexStructures_ReturnsValidV2(string inputMarkdown, string expectedV2)
    {
        var resultList = converter.Convert(inputMarkdown);
        var actualText = string.Join("", resultList.Select(x => x.Text));
        
        Assert.That(actualText, Is.EqualTo(expectedV2));
    }
}