using System.ComponentModel.DataAnnotations;

namespace Desafio3.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class DecimalScaleAttribute(int maximumScale) : ValidationAttribute
{
    public int MaximumScale { get; } = maximumScale;

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        return value is decimal number
            && decimal.Round(number, MaximumScale, MidpointRounding.ToEven) == number;
    }

    public override string FormatErrorMessage(string name) =>
        $"{name} no puede tener más de {MaximumScale} decimales.";
}