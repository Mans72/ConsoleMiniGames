using ConsoleMiniGames.Types;
using System.Runtime.InteropServices;

namespace ConsoleMiniGames {

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
}