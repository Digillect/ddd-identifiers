using Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

namespace Digillect.DDD.Identifiers.SourceGenerator;

public static class IdentifierSourceGeneratorBuilderExtensions
{
	public static IdentifierType HandleStructsDecoratedWithDigillectIdentifierAttribute<T>(this IdentifierSourceGeneratorBuilder builder) =>
		builder.HandleStructsDecoratedWithDigillectIdentifierAttribute($"global::{typeof(T).FullName!}");

	public static IdentifierType HandleStructsDecoratedWithDigillectIdentifierAttribute(
		this IdentifierSourceGeneratorBuilder builder,
		string valueTypeName)
	{
		builder.AddSelector<AttributeBasedIdentifierSelector>();

		return builder
			.AddType(valueTypeName)
			.AddDetector(new DigillectIdentifierAttributeBasedIdentifierDetector(valueTypeName));
	}

	public static IdentifierSourceGeneratorBuilder AddIntegerType(this IdentifierSourceGeneratorBuilder builder, Action<IdentifierType>? configure = null)
	{
		var type = builder.HandleStructsDecoratedWithDigillectIdentifierAttribute<int>()
			.AddGenerator<DefaultConstructorGenerator>()
			.AddGenerator<IntegerIdentifierWithEmptyGenerator>()
			.AddGenerator<IntegerParsableInterfaceGenerator>()
			.AddGenerator<DefaultEquatableInterfaceGenerator>()
			.AddGenerator<DefaultComparableInterfaceGenerator>()
			.AddGenerator<DefaultSystemObjectOverridesGenerator>()
			.AddGenerator<IntegerComponentModelTypeConverterGenerator>()
			.AddGenerator<IntegerSystemTextJsonConverterGenerator>();

		configure?.Invoke(type);

		return builder;
	}

	public static IdentifierSourceGeneratorBuilder AddStringType(this IdentifierSourceGeneratorBuilder builder, Action<IdentifierType>? configure = null)
	{
		var type = builder.HandleStructsDecoratedWithDigillectIdentifierAttribute<string>()
			.AddGenerator<StringConstructorGenerator>()
			.AddGenerator<StringIdentifierWithEmptyGenerator>()
			.AddGenerator<StringEquatableInterfaceGenerator>()
			.AddGenerator<StringComparableInterfaceGenerator>()
			.AddGenerator<StringSystemObjectOverridesGenerator>()
			.AddGenerator<StringComponentModelTypeConverterGenerator>()
			.AddGenerator<StringSystemTextJsonConverterGenerator>();

		configure?.Invoke(type);

		return builder;
	}

	public static IdentifierSourceGeneratorBuilder AddGuidType(this IdentifierSourceGeneratorBuilder builder, Action<IdentifierType>? configure = null)
	{
		var type = builder.HandleStructsDecoratedWithDigillectIdentifierAttribute<Guid>()
			.AddGenerator<DefaultConstructorGenerator>()
			.AddGenerator<GuidIdentifierWithNewGenerator>()
			.AddGenerator<GuidIdentifierWithEmptyGenerator>()
			.AddGenerator<GuidParsableInterfaceGenerator>()
			.AddGenerator<DefaultEquatableInterfaceGenerator>()
			.AddGenerator<DefaultComparableInterfaceGenerator>()
			.AddGenerator<DefaultSystemObjectOverridesGenerator>()
			.AddGenerator<GuidComponentModelTypeConverterGenerator>()
			.AddGenerator<GuidSystemTextJsonConverterGenerator>();

		configure?.Invoke(type);

		return builder;
	}

	public static IdentifierSourceGeneratorBuilder AddGuidV7Type(this IdentifierSourceGeneratorBuilder builder, Action<IdentifierType>? configure = null)
	{
		var type = builder.HandleStructsDecoratedWithDigillectIdentifierAttribute<Guid>()
			.AddGenerator<DefaultConstructorGenerator>()
			.AddGenerator<GuidV7IdentifierWithNewGenerator>()
			.AddGenerator<GuidIdentifierWithEmptyGenerator>()
			.AddGenerator<GuidParsableInterfaceGenerator>()
			.AddGenerator<DefaultEquatableInterfaceGenerator>()
			.AddGenerator<DefaultComparableInterfaceGenerator>()
			.AddGenerator<DefaultSystemObjectOverridesGenerator>()
			.AddGenerator<GuidComponentModelTypeConverterGenerator>()
			.AddGenerator<GuidSystemTextJsonConverterGenerator>();

		configure?.Invoke(type);

		return builder;
	}
}
