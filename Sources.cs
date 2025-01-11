namespace ConsoleMiniGames {

	internal static partial class Sources {
		public static readonly string Digits = "0123456789";

		// Устанавлевает цвет
		public static void SetColor(ConsoleColor back, ConsoleColor fore) {
			Console.BackgroundColor = back;
			Console.ForegroundColor = fore;
		}
		public static void SetColor(ConsoleColor color) {
			Console.BackgroundColor = color;
			Console.ForegroundColor = color;
		}
		// Перетасовывает элемменты в массиве
		public static void ArrayShuffle<T>(T[] array) {
			Random rnd = new();
			for (int i = array.Length - 1; i > 0; i--) {
				int j = rnd.Next(i + 1); // Случайное число от 0 до i включительно
				(array[i], array[j]) = (array[j], array[i]);
			}
		}
		// Функция для перетасовки двумерного массива
		public static void ArrayShuffle<T>(T[,] array) {
			int rows = array.GetLength(0), cols = array.GetLength(1);

			// Преобразуем двумерный массив в одномерный
			T[] flattenedArray = new T[rows * cols];
			for (int i = 0; i < rows; i++)
				for (int j = 0; j < cols; j++)
					flattenedArray[i * cols + j] = array[i, j];

			ArrayShuffle(flattenedArray);

			// Преобразуем одномерный массив обратно в двумерный
			for (int i = 0; i < rows; i++)
				for (int j = 0; j < cols; j++)
					array[i, j] = flattenedArray[i * cols + j];
		}
		// Генерирует рандомное число заданной длины
		public static string GenRandNumber(int length) {
			Random rand = new();
			string number = "";
			do {
				number += rand.Next(0, 10).ToString();
			} while (number.Length != length);
			return number;
		}
		// Проверяет не повторяются ли цифры в числе
		public static bool HasUniqueDigits(string number, int length) {
			if (number.Length == length && number.Length > 0 && number.Length <= 10) {
				string digits = "";
				foreach (char digit in number) {
					if (digits.Contains(digit))
						return false;
					digits += digit;
				}
				return true;
			}
			return false;
		}
		// Превращает KeyChar в число.
		public static int KeyToNumb(ConsoleKeyInfo x) {
			return x.KeyChar switch {
				'0' => 0,
				'1' => 1,
				'2' => 2,
				'3' => 3,
				'4' => 4,
				'5' => 5,
				'6' => 6,
				'7' => 7,
				'8' => 8,
				'9' => 9,
				_ => -1,
			};
		}
		// Превращает Char в число.
		public static int CharToInt(char x) {
			return x switch {
				'0' => 0,
				'1' => 1,
				'2' => 2,
				'3' => 3,
				'4' => 4,
				'5' => 5,
				'6' => 6,
				'7' => 7,
				'8' => 8,
				'9' => 9,
				_ => -1,
			};
		}
		// Превращает string в число.
		public static int StringToInt(string input) {
			if (int.TryParse(input, out int result)) {
				return result; // Успешное преобразование
			}
			else {
				throw new ArgumentException("The input string is not a valid number."); // Если строка не может быть преобразована
			}
		}
		// Проверяет стоит ли элемент1 [x, y] вокруг элемента2 [row, col], включая его самого
		public static bool IsAround(int row, int col, int x, int y) {
			for (int i = -1; i <= 1; i++)
				for (int j = -1; j <= 1; j++)
					if (x == row + i && y == col + j)
						return true;
			return false;
		}
		// Центрирует строку добавляя пробелы с двух сторон
		public static string CentereString(string line, int totalWidth) {
			int spacesToAdd = totalWidth - line.Length;

			if (spacesToAdd > 0) {
				// Делим пробелы на две стороны
				int padRight = spacesToAdd / 2;
				int padLeft = spacesToAdd - padRight;
				// Формируем строку с пробелами по краям
				return new string(' ', padLeft) + line + new string(' ', padRight);
			}
			// Если строка длиннее нужной длины, обрезаем
			else return line[..totalWidth];
		}
		// Конвертирует секунды в mm:ss
		public static string FormatTime(int totalSeconds) {
			int minutes = totalSeconds / 60;
			int seconds = totalSeconds % 60;
			return $"{minutes:D2}:{seconds:D2}";
		}

	}
}