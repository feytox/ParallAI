using System.Linq.Expressions;
using System.Reflection;
using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;

namespace ParallAI.TeleBot.Core.Settings;

public class SettingsPartsBuilder<TState> where TState : SettingsState
{
    private readonly List<SettingsPart<TState>> parts = [];

    public SettingsPartsBuilder<TState> AddSimple<TValue>(string name, string inputMessage, string failMessage,
        Func<string, TValue?> parser, Expression<Func<TState, TValue?>> propertySelector)
    {
        var getter = propertySelector.Compile();
        var setter = CreateSetter(propertySelector);
        
        var part = new SimpleSettingsHandlerPart<TValue, TState>(
            name, inputMessage, failMessage, parser, getter, setter);
        parts.Add(part);
        return this;
    }
    
    public SettingsPartsBuilder<TState> AddEnum<TEnum>(string tag, string name, string inputMessage,
        Func<TEnum, bool> selector, Expression<Func<TState, TEnum?>> propertySelector) where TEnum : struct, Enum
    {
        var getter = propertySelector.Compile();
        var setter = CreateSetter(propertySelector);
        
        var part = new EnumSettingsPart<TEnum, TState>(tag, name, inputMessage, selector, getter, setter);
        parts.Add(part);
        return this;
    }

    public SettingsPartsBuilder<TState> AddSelect<TValue>(string tag, string name, string inputMessage,
        Func<User, IReadOnlyList<TValue>> elementsProvider, Func<TValue, string> nameSelector,
        Expression<Func<TState, TValue?>> propertySelector)
    {
        var getter = propertySelector.Compile();
        var setter = CreateSetter(propertySelector);
        
        var part = new SelectSettingsPart<TState, TValue>(tag, name, inputMessage, 
            elementsProvider, nameSelector, getter, setter);
        parts.Add(part);
        return this;
    }

    public SettingsPartsBuilder<TState> Add(SettingsPart<TState> part)
    {
        parts.Add(part);
        return this;
    }

    public SettingsPart<TState>[] Build() => parts.ToArray();
    
    private static Action<TState, TProperty> CreateSetter<TProperty>(
        Expression<Func<TState, TProperty?>> propertySelector)
    {
        var propertyInfo = GetPropertyInfo(propertySelector);
        
        return (state, value) => propertyInfo.SetValue(state, value);
    }
    
    private static PropertyInfo GetPropertyInfo<T>(Expression<Func<TState, T>> propertySelector)
    {
        var body = propertySelector.Body;
        
        if (body is UnaryExpression unary)
            body = unary.Operand;

        if (body is MemberExpression { Member: PropertyInfo propertyInfo })
            return propertyInfo;

        throw new ArgumentException($"Expression '{propertySelector}' must be a property access (e.g. x => x.Prop)");
    }
}