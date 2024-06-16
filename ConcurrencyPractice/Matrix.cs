namespace ConcurrencyPractice
{
	public class Matrix
	{
		public Matrix(bool isInvertible = true) { IsInvertible = isInvertible; }

		public void Rotate(float degrees)
		{
			Console.WriteLine($"Roate {degrees}");
		}

		public bool IsInvertible { get; set; }

		public void Invert() { }


	}
}
