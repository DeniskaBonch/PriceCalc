using System;
using System.Globalization;

namespace PriceCalc;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Расчёт цены с НДС и без НДС");
        Console.WriteLine("Обе цены будут иметь не более 2 знаков после запятой.");
        Console.WriteLine();

        while (true)
        {
            decimal inputPrice = ReadDecimal("Введите рекомендованную цену с НДС: ");
            int procNds = ReadVatPercent();

            PriceCalculator.CalcPrices(
                inputPrice,
                procNds,
                out decimal correctedWithNds,
                out decimal correctedWithoutNds);

            decimal diff = Math.Abs(correctedWithNds - inputPrice);
            decimal check = correctedWithoutNds * (1 + procNds / 100m);

            Console.WriteLine();
            Console.WriteLine($"Входная цена с НДС:        {inputPrice}");
            Console.WriteLine($"Процент НДС:               {procNds}%");
            Console.WriteLine($"Цена с НДС:                {correctedWithNds:F2}");
            Console.WriteLine($"Цена без НДС:              {correctedWithoutNds:F2}");
            Console.WriteLine($"Отклонение от входной:     {diff:F20}");
            Console.WriteLine($"Проверка:                  {correctedWithoutNds:F2} * (1 + {procNds}/100) = {check:F2}");
            Console.WriteLine();

            Console.Write("Продолжить? (y/n): ");
            string? answer = Console.ReadLine();

            if (!string.Equals(answer?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
                break;
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Пустой ввод. Попробуйте снова.");
                continue;
            }

            text = text.Trim().Replace(',', '.');

            if (decimal.TryParse(
                    text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal value))
            {
                return value;
            }

            Console.WriteLine("Некорректное число. Пример: 123.45 или 123,45");
        }
    }

    private static int ReadVatPercent()
    {
        while (true)
        {
            Console.Write("Введите процент НДС (0..99): ");
            string? text = Console.ReadLine();

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                && value >= 0
                && value <= 99)
            {
                return value;
            }

            Console.WriteLine("Процент НДС должен быть целым числом от 0 до 99.");
        }
    }
}