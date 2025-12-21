using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public static class DefaultPresets
{
    private static readonly ImmutablePreset Default = new(new Guid("00000000-0000-0000-0000-000000000001"),
        "Default", PromptSettings.Default);

    private static readonly ImmutablePreset DefaultCompare = new(new Guid("00000000-0000-0000-0000-000000000002"),
        "Оркестратор", new PromptSettings(DefaultComparePrompt, 0.3m, ThinkingBudget.Medium));

    public static readonly ImmutablePreset[] Presets = [Default, DefaultCompare];

    private const string DefaultComparePrompt = """
                                                <example>

                                                <input>
                                                ```
                                                Выберите истинные утверждения:
                                                1. 2 + 2 = 0;
                                                2. $\sin x$ линеен по $x$;
                                                3. $\Gamma(x)$ сходится равномерно на $[\varepsilon, E]$, $\forall{0 < \varepsilon < E < \infty}$;
                                                4. Любое ограниченное и замкнутое множество является компактным.

                                                ---

                                                <response_1>
                                                Ответы:
                                                1. Да в Z_2 и нет в стандартной арифметике
                                                2. Нет
                                                3. Да
                                                4. Нет, есть контрпример
                                                </response_1>

                                                <response_2>
                                                **Ответ: 3, 4.**

                                                **Пояснение:**
                                                1.  **Ложно.** В стандартной арифметике $2 + 2 = 4$.
                                                2.  **Ложно.** Функция $\sin x$ не является линейной (график — синусоида, а не прямая, и $\sin(x+y) \neq \sin x + \sin y$).
                                                3.  **Истинно.** Интеграл, определяющий Гамма-функцию $\Gamma(x) = \int_0^\infty t^{x-1}e^{-t}dt$, сходится равномерно на любом отрезке $[\varepsilon, E] \subset (0, +\infty)$ по признаку Вейерштрасса.
                                                4.  **Истинно (в контексте классического матанализа).** В конечномерных евклидовых пространствах ($\mathbb{R}^n$) ограниченность и замкнутость множества являются необходимыми и достаточными условиями его компактности (Теорема Гейне-Бореля).
                                                    *(Примечание: В бесконечномерных пространствах утверждение 4 ложно, но в контексте других вопросов данного списка подразумевается классический анализ).*
                                                </response_2>

                                                <response_3>
                                                3, 4
                                                </response_3>
                                                ```
                                                </input>

                                                <output>
                                                Результат анализа ответов:
                                                1. - Да (1 ответ): в Z_2 это так.
                                                   - Нет (3 ответа): в стандартной арифметике это не так.
                                                2. Нет (3 ответа): $\sin(x+y) \neq \sin x + \sin y$.
                                                3. Да (3 ответа): по признаку Вейерштрасса.
                                                4. - Да (2 ответа): В $\mathbb{R}^n$ это так.
                                                   - Нет (2 ответа): В бесконечномерных пространствах это не так.
                                                </output>

                                                </example>

                                                <task>
                                                Based on the examples above, your task is to analyze several responses to the query and combine them into a result. You should rely only on the information provided in the responses. You should respond in the same language as the query text.
                                                </task>

                                                <input_format>
                                                ```
                                                [Text of the original query]

                                                ---

                                                <response_1>
                                                [Response 1]
                                                </response_1>

                                                <response_2>
                                                [Response 2]
                                                </response_2>
                                                ...
                                                ```
                                                </input_format>
                                                """;
}