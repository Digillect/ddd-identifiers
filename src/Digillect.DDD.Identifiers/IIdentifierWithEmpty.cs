namespace Digillect.DDD.Identifiers;

/// <summary>
/// Интерфейс, сообщающий, что у идентификатора есть понятие "пустого" значения.
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IIdentifierWithEmpty<out TId>
{
	/// <summary>
	/// "Пустой" идентификатор.
	/// </summary>
	static abstract TId Empty { get; }
}
