# Digillect DDD Identifiers

Extensible framework to build source generators that create value-object-based identifiers, accompanied by the
pre-made all-purpose source generator.

## Why?

**Value-object identifiers** prioritize clear, stable representations of entities, enabling developers to focus 
on the unique identity of objects rather than potentially mutable or overloaded values. This approach mitigates
risks of misrepresentation, improves maintainability, and enhances the semantic precision of the codebase.

#### **Why Not Value Obsession?**
1. **Ambiguity**: Over-reliance on raw values can lead to ambiguity. For example, using `12345` as both an account number and a transaction ID can cause confusion.
2. **Mutability**: If values change (e.g., due to normalization or formatting), code dependent on these raw values may break.
3. **Lack of Context**: Values alone lack the context needed to distinguish their role, which can result in bugs or misinterpretation.

#### **Why Value-Object Identifiers?**
1. **Strong Typing**: Encapsulating identifiers in value objects enforces a strict type system. For example, `AccountNumber` and `TransactionId` are distinct types, reducing the likelihood of accidental misuse.
2. **Immutability**: Value-object identifiers are typically immutable, ensuring their stability and reliability as references.
3. **Enhanced Readability**: Using descriptive value objects makes the code self-documenting. Instead of `string` or `int`, a developer sees `CustomerId` or `ProductSku`, clarifying intent.
4. **Domain Modeling**: Value objects align with Domain-Driven Design principles by representing concepts in the domain explicitly and unambiguously.

By transitioning to value-object identifiers, developers create a codebase that is more **resilient to change**, 
easier to understand, and less prone to errors, especially in complex systems with interrelated entities.


## Usage

### As a ready-to-use source generator

If the value types of your identifiers are limited to `int`, `string` or `Guid`, you can use the 
`Digillect.DDD.Identifiers.SourceGenerator` by referencing that package (and `Digillect.DDD.Identifiers` as well) from
your project. Then declare public readonly partial structs and decorate them with the
`Digillect.DDD.Identifiers.IdentifierAttribute<T>`, where `T` is one of `int`, `string` or `Guid`.

```csharp
using Digillect.DDD.Identifiers;

[Identifier<int>]
public readonly partial struct IntegerBasedIdentifier;

[Identifier<string>]
public readonly partial struct StringBasedIdentifier;

[Identifier<Guid>]
public readonly partial struct GuidBasedIdentifier;
```

### As a slightly tuned custom generator

Use [Example generator](https://github.com/Digillect/ddd-identifiers/blob/main/example/ExampleGenerator/ExampleGenerator.csproj)
as a reference to learn how to modify source generator configuration.

Please note that you have to modify the project file of your source generator's project to
simplify its consumption.

### As a custom source generator with your own logic

[TBD]
