namespace FurryPets.Shared.Extensions;

public static class StringExtensions
{
	public static string ToLowerFirstChar(this string input)
	{
		var newString = input;

		if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
		{
			newString = char.ToLower(newString[0]) + newString[1..];
		}

		return newString;
	}
}