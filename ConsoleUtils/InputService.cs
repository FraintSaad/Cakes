using System.Data;

namespace ConsoleUtils
{
    public class InputService
    {
        public static double? GetDouble()
        {
            // Создается пустая переменная для результата
            double result;
            
            // Берем ввод пользователя и сохраняем как строку в input
            string? input = Console.ReadLine();

            bool parsingResult = double.TryParse(input, out result);
            if (!parsingResult)
            {
                Console.WriteLine("Неверный ввод");
                return null;
            }

            return result;
        }
        public static DateTime? GetDate()
        {
            DateTime result;
            string? input = Console.ReadLine();
            bool parsingResult = DateTime.TryParse(input, out result);
            if (!parsingResult)
            {
                Console.WriteLine("Неверный ввод");
                return null;
            }
            
            return result;
        }

        public static string? GetString()
        {
            return Console.ReadLine();
            
        }

        public static int? GetInt()
        {
            int result;
            string? input= Console.ReadLine();
            bool parsingResult= int.TryParse(input, out result);
            if(!parsingResult )
            {
                Console.WriteLine("Неверный ввод");
                return null;
            }
            return result;
        }
        public static char GetOption(string message, Dictionary<char, string> options)
        {
            Console.WriteLine("Выберите режим: ");
            
            foreach (var option in options)
            {
                Console.WriteLine($"{option.Key}. {option.Value}");
            }

            try
            {
                // Пытаемся получить числовое значение
                ConsoleKeyInfo input = Console.ReadKey(true);
                Console.Clear();
                char mode = input.KeyChar;

                if (!options.ContainsKey(mode) && mode != '\r')
                {
                    // Если ошибка, запускаем текущий метод, заново
                    return GetOption(message, options);
                }

                // Если код дошел сюда, возвращаем значение выбранного режима
                return mode;
                
            }
            catch (Exception)
            {
                // Если ошибка, запускаем текущий метод, заново
                return GetOption(message, options);
            }
        }
        
    }
}
