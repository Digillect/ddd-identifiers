namespace Digillect.DDD.Identifiers;

/// <summary>
/// Атрибут, предоставляющий пример значения идентификатора.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class IdentifierExampleAttribute : Attribute
{
    /// <summary>
    /// Инициализирует экземпляр класса.
    /// </summary>
    /// <param name="value">Пример значение идентификатора.</param>
    public IdentifierExampleAttribute(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Пример значения идентификатора.
    /// </summary>
    public string Value { get; }
}
