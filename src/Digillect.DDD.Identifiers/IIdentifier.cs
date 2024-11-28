namespace Digillect.DDD.Identifiers;

/// <summary>
/// Интерфейс, сообщающий, что тип является идентификатором.
/// </summary>
/// <typeparam name="TId">Тип идентификатора.</typeparam>
/// <typeparam name="TValue">Тип значения.</typeparam>
public interface IIdentifier<out TId, TValue> : IIdentifier
	where TId : IIdentifier<TId, TValue>
{
	/// <summary>
	/// Значение идентификатора.
	/// </summary>
	TValue Value { get; }

	/// <summary>
	/// Создаёт новый экземпляр идентификатора с указанным значением.
	/// </summary>
	/// <param name="value">Значение.</param>
	/// <returns>Идентификатор.</returns>
	static abstract TId Create(TValue value);
}

public interface IIdentifier;
