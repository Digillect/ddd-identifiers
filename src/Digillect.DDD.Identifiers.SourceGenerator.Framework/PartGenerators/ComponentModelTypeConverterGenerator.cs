using System.CodeDom.Compiler;

namespace Digillect.DDD.Identifiers.SourceGenerator.PartGenerators;

public abstract class ComponentModelTypeConverterGenerator : IIdentifierPartGenerator
{
	public IEnumerable<string> GenerateAttributes(Identifier identifier)
	{
		yield return $"[global::System.ComponentModel.TypeConverter(typeof({identifier.Name}.ComponentModelTypeConverter))]";
	}

	public IEnumerable<string> GenerateInterfaces(Identifier identifier)
	{
		return Array.Empty<string>();
	}

	public void GenerateMembers(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("class ComponentModelTypeConverter : global::System.ComponentModel.TypeConverter");
		writer.WriteBlock(() => {
			writer.WriteLine("public override bool CanConvertFrom(global::System.ComponentModel.ITypeDescriptorContext? context, global::System.Type sourceType)");
			writer.WriteBlock(() => {
				IEnumerable<string> comparisons = ConversionSourceTypes.Select(type => $"sourceType == typeof({type})");

				writer.WriteLine($"return {string.Join(" || ", comparisons)} || base.CanConvertFrom(context, sourceType);");
			});
			writer.WriteEmptyLine();
			writer.WriteLine(
				"public override object? ConvertFrom(global::System.ComponentModel.ITypeDescriptorContext? context, global::System.Globalization.CultureInfo? culture, object value)");
			writer.WriteBlock(() => EmitConvertFromBody(identifier, writer));
			writer.WriteEmptyLine();
			writer.WriteLine("public override bool CanConvertTo(global::System.ComponentModel.ITypeDescriptorContext? context, global::System.Type? sourceType)");
			writer.WriteBlock(() => {
				IEnumerable<string> comparisons = ConversionSourceTypes.Select(type => $"sourceType == typeof({type})");

				writer.WriteLine($"return {string.Join(" || ", comparisons)} || base.CanConvertTo(context, sourceType);");
			});
			writer.WriteEmptyLine();
			writer.WriteLine(
				"public override object? ConvertTo(global::System.ComponentModel.ITypeDescriptorContext? context, global::System.Globalization.CultureInfo? culture, object? value, global::System.Type destinationType)");
			writer.WriteBlock(() => {
				writer.WriteLine($"if (value is {identifier.Name} idValue)");
				writer.WriteBlock(() => EmitConvertToBody(identifier, writer));
				writer.WriteEmptyLine();
				writer.WriteLine("return base.ConvertTo(context, culture, value, destinationType);");
			});
		});
	}

	/// <summary>
	/// Типы, которые могут быть преобразованы в идентификатор.
	/// </summary>
	protected abstract IEnumerable<string> ConversionSourceTypes { get; }

	/// <summary>
	/// Генерирует код метода <c>ConvertFrom</c>.
	/// </summary>
	protected abstract void EmitConvertFromBody(Identifier identifier, IndentedTextWriter writer);

	/// <summary>
	/// Генерирует код метода <c>ConvertTo</c>.
	/// </summary>
	protected abstract void EmitConvertToBody(Identifier identifier, IndentedTextWriter writer);
}

public sealed class GuidComponentModelTypeConverterGenerator : ComponentModelTypeConverterGenerator
{
	protected override IEnumerable<string> ConversionSourceTypes => ["System.Guid", "string"];

	protected override void EmitConvertFromBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return value switch");
		writer.WriteBlock(() => {
			writer.WriteLine($"global::System.Guid guidValue => new {identifier.Name}(guidValue),");
			writer.WriteLine($"string stringValue when !string.IsNullOrEmpty(stringValue) && global::System.Guid.TryParse(stringValue, out global::System.Guid result) => new {identifier.Name}(result),");
			writer.WriteLine("_ => base.ConvertFrom(context, culture, value)");
		}, withSemicolon: true);
	}

	protected override void EmitConvertToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (destinationType == typeof(global::System.Guid))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value;"));
		writer.WriteEmptyLine();
		writer.WriteLine("if (destinationType == typeof(string))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value.ToString();"));
	}
}

public sealed class IntegerComponentModelTypeConverterGenerator : ComponentModelTypeConverterGenerator
{
	protected override IEnumerable<string> ConversionSourceTypes => ["int", "string"];

	protected override void EmitConvertFromBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return value switch");
		writer.WriteBlock(() => {
			writer.WriteLine($"int intValue => new {identifier.Name}(intValue),");
			writer.WriteLine(
				$"string stringValue when !string.IsNullOrEmpty(stringValue) && int.TryParse(stringValue, out int result) => new {identifier.Name}(result),");
			writer.WriteLine("_ => base.ConvertFrom(context, culture, value)");
		}, withSemicolon: true);
	}

	protected override void EmitConvertToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (destinationType == typeof(int))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value;"));
		writer.WriteEmptyLine();
		writer.WriteLine("if (destinationType == typeof(string))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value.ToString();"));
	}
}

public sealed class StringComponentModelTypeConverterGenerator : ComponentModelTypeConverterGenerator
{
	protected override IEnumerable<string> ConversionSourceTypes => ["string"];

	protected override void EmitConvertFromBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("return value switch");
		writer.WriteBlock(() => {
			writer.WriteLine($"string stringValue when !string.IsNullOrEmpty(stringValue) => new {identifier.Name}(stringValue),");
			writer.WriteLine("_ => base.ConvertFrom(context, culture, value)");
		}, withSemicolon: true);
	}

	protected override void EmitConvertToBody(Identifier identifier, IndentedTextWriter writer)
	{
		writer.WriteLine("if (destinationType == typeof(string))");
		writer.WriteBlock(() => writer.WriteLine("return idValue.Value;"));
	}
}
