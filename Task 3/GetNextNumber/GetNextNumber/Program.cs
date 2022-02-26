using System.Net.NetworkInformation;
public class Programm
{
	public static void Main(string[] args)
	{
		ulong number;
		while((number = ulong.Parse(Console.ReadLine()))!= null)
			Console.WriteLine(NumberConverter.GetNextNumber(number));
	}
}
public class NumberConverter
{
	/// <summary>
	/// returns the next largest number with the same digits as the initial number, but the next largest; if initial number have largest value, returns it unchanged
	/// </summary>
	/// <param name="number">initial number</param>
	/// <returns></returns>
	public static ulong GetNextNumber(ulong number)
	{
		if (number < 10)
			return number;

		List<char> digits = number.ToString().ToCharArray().ToList();

		int i = digits.Count() - 2, value = digits[digits.Count() - 1];
		while (i > 0 && value <= digits[i])
		{
			value = digits[i];
			i--;
		}
		if (value > digits[i])
		{
			List<char> result = ChangeDigitOrder(digits.GetRange(i, digits.Count - i));
			digits.RemoveRange(i, digits.Count - i);
			digits.AddRange(result);
		}
		return ulong.Parse(new string(digits.ToArray()));
	}
	/// <summary>
	/// returns an array of digits in the following order: the first is the maximum number, the next are the other numbers in ascending order 
	/// </summary>
	/// <param name="digits">an array of digits whose order we need to change</param>
	/// <returns></returns>
	private static List<char> ChangeDigitOrder(List<char> digits)
	{
		digits = digits.OrderBy(x => x).ToList();
		digits.Insert(0, digits[digits.Count - 1]);
		digits.RemoveAt(digits.Count - 1);
		return digits;
	}
}