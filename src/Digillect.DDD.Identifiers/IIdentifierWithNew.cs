namespace Digillect.DDD.Identifiers;

/// <summary>
/// Интерфейс, сообщающий, что тип идентификатора может генерировать уникальные значения.
/// </summary>
/// <typeparam name="TId">Тип идентификатора.</typeparam>
public interface IIdentifierWithNew<out TId>
	where TId : IIdentifierWithNew<TId>
{
	/// <summary>
	/// Генерирует новое уникальное значение идентификатора.
	/// </summary>
	/// <returns>Идентификатор с уникальным значением.</returns>
	static abstract TId New();
}
