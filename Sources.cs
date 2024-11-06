using System.Runtime.InteropServices;

namespace MiniGames_con {

	internal enum Difficulty {
		Easy,
		Medium,
		Hard
	}
	internal enum MouseButton {
		NoButton = 0x0000,
		LeftButton = 0x0001,
		RightButton = 0x0002,
		MiddleButton = 0x0004,
		FourthButton = 0x0008,
		FivethButton = 0x0010
	}


	//internal interface IRenderable {
	//	//Field GetMyField();
	//	//Field GetEnemyField();
	//}
	//internal interface IGame {
	//	public void StartGame();
	//	public void EndGame();
	//}
	//internal interface IBoard<T> {
	//	public T[,] Grid { get; }
	//	public bool IsValidMove(XY position);
	//}
	//internal interface IRenderer {
	//	void RenderBoard<T>(IBoard<T> board);
	//	void RenderMessage(string message);
	//}

	public class ReturnToMenu : Exception {
	}


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
			else return line.Substring(0, totalWidth);
		}
		// Конвертирует секунды в mm:ss
		public static string FormatTime(int totalSeconds) {
			int minutes = totalSeconds / 60;
			int seconds = totalSeconds % 60;
			return $"{minutes:D2}:{seconds:D2}";
		}

	}



	internal static class MouseClickHandler {
		// Структура для хранения информации о событии ввода в консоли
		[StructLayout(LayoutKind.Sequential)]
		private struct INPUT_RECORD {
			public short EventType;
			public MOUSE_EVENT_RECORD MouseEvent;
		}
		// Структура для хранения информации о событии мыши
		[StructLayout(LayoutKind.Sequential)]
		private struct MOUSE_EVENT_RECORD {
			public COORD dwMousePosition;
			public int dwButtonState;
			public int dwControlKeyState;
			public int dwEventFlags;
		}
		// Структура для хранения 
		[StructLayout(LayoutKind.Sequential)]
		private struct COORD {
			public short X;
			public short Y;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ReadConsoleInput(IntPtr hConsoleInput, out INPUT_RECORD lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

		private const int STD_INPUT_HANDLE = -10;
		private const short MOUSE_EVENT = 0x0002;
		private const int ENABLE_MOUSE_INPUT = 0x0010;
		private const int ENABLE_QUICK_EDIT_MODE = 0x0040;
		private const int ENABLE_EXTENDED_FLAGS = 0x0080;

		// dwEventFlags
		private const int MOUSE_WHEELED = 0x0004; // Указывает, что произошло событие прокрутки колесика
		private const int MOUSE_HWHEELED = 0x0008; // Указывает на горизонтальную прокрутку колесика.

		private static IntPtr consoleHandle;

		static MouseClickHandler() {
			// Получаем дескриптор консоли
			consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
			EnableMouseInput();
		}
		// Метод для включения обработки событий мыши
		private static void EnableMouseInput() {
			// Получаем текущий режим консоли
			GetConsoleMode(consoleHandle, out int consoleMode);
			// Включаем режим обработки мыши
			consoleMode |= ENABLE_MOUSE_INPUT;
			// Отключаем режим быстрого редактирования
			consoleMode &= ~ENABLE_QUICK_EDIT_MODE;
			// Применяем измененный режим консоли
			SetConsoleMode(consoleHandle, consoleMode);
		}
		// Метод для отключения обработки событий мыши
		private static void DisableMouseInput() {
			// Получаем текущий режим консоли
			GetConsoleMode(consoleHandle, out int consoleMode);
			// Отключаем режим обработки мыши
			consoleMode &= ~ENABLE_MOUSE_INPUT;
			// Отключаем режим быстрого редактирования
			consoleMode |= ENABLE_QUICK_EDIT_MODE;
			// Применяем измененный режим консоли
			SetConsoleMode(consoleHandle, consoleMode);
			// Метод для включения обработки событий мыши
		}

		internal static (XY, MouseButton) GetMouseClick() {
			//EnableMouseInput();
			while (true) {
				ReadConsoleInput(consoleHandle, out INPUT_RECORD record, 1, out uint eventsRead);

				if (eventsRead > 0 && record.EventType == MOUSE_EVENT && record.MouseEvent.dwButtonState != 0x0000 && record.MouseEvent.dwEventFlags == 0) {
					XY position = new(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y);
					if (record.MouseEvent.dwButtonState == 0x0001) return (position, MouseButton.LeftButton);
					else if (record.MouseEvent.dwButtonState == 0x0002) return (position, MouseButton.RightButton);
					else if (record.MouseEvent.dwButtonState == 0x0004) return (position, MouseButton.MiddleButton);
				}
			}
		}
		internal static (XY, MouseButton) GetMouse() {
			while (true) {
				ReadConsoleInput(consoleHandle, out INPUT_RECORD record, 1, out uint eventsRead);
				if (eventsRead > 0 && record.EventType == MOUSE_EVENT) {
					XY position = new(record.MouseEvent.dwMousePosition.X, record.MouseEvent.dwMousePosition.Y);
					if (record.MouseEvent.dwEventFlags == 0) {
						if (record.MouseEvent.dwButtonState == 0x0001) return (position, MouseButton.LeftButton);
						else if (record.MouseEvent.dwButtonState == 0x0002) return (position, MouseButton.RightButton);
						else if (record.MouseEvent.dwButtonState == 0x0004) return (position, MouseButton.MiddleButton);
					}
					return (position, MouseButton.NoButton);
				}
			}
		}
	}



	internal struct XY(int x, int y) : IEquatable<XY> {
		internal int X = x;
		internal int Y = y;

		public readonly bool Equals(XY other) {
			return this == other;
		}
		public readonly override bool Equals(object? obj) {
			return obj is XY bulls && Equals(bulls);
		}
		public readonly override int GetHashCode() {
			return HashCode.Combine(X, Y);
		}

		public readonly bool XInBtwn(int a, int b) {
			return X >= a && X <= b;
		}
		public readonly bool YInBtwn(int a, int b) {
			return Y >= a && Y <= b;
		}
		public readonly bool XBtwn(int a, int b) {
			return X > a && X < b;
		}
		public readonly bool YBtwn(int a, int b) {
			return Y > a && Y < b;
		}

		public static bool operator ==(XY left, XY right) {
			return left.X == right.X && left.Y == right.Y;
		}
		public static bool operator ==(XY left, (int x, int y) right) {
			return left.X == right.x && left.Y == right.y;
		}
		public static bool operator !=(XY left, XY right) {
			return left.X != right.X || left.Y != right.Y;
		}
		public static bool operator !=(XY left, (int x, int y) right) {
			return left.X != right.x || left.Y != right.y;
		}

		public static bool operator <(XY left, XY right) {
			return left.X < right.X && left.Y < right.Y;
		}
		public static bool operator <(XY left, (int x, int y) right) {
			return left.X < right.x && left.Y < right.y;
		}
		public static bool operator >(XY left, XY right) {
			return left.X > right.X && left.Y > right.Y;
		}
		public static bool operator >(XY left, (int x, int y) right) {
			return left.X > right.x && left.Y > right.y;
		}

		public static bool operator <=(XY left, XY right) {
			return left.X <= right.X && left.Y <= right.Y;
		}
		public static bool operator <=(XY left, (int x, int y) right) {
			return left.X <= right.x && left.Y <= right.y;
		}
		public static bool operator >=(XY left, XY right) {
			return left.X >= right.X && left.Y >= right.Y;
		}
		public static bool operator >=(XY left, (int x, int y) right) {
			return left.X >= right.x && left.Y >= right.y;
		}
	}
}