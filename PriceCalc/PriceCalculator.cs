using System;

namespace PriceCalc;

public static class PriceCalculator
{
    public static void CalcPrices(
        decimal inputPriceWithNds,
        int procNds,
        out decimal correctedPriceWithNds,
        out decimal correctedPriceWithoutNds)
    {
        if (procNds < 0 || procNds > 99)
            throw new ArgumentOutOfRangeException(nameof(procNds), "Процент НДС должен быть в диапазоне 0..99.");

        int q = 100 + procNds;

        int g = Gcd(q, 100);
        int stepCents = q / g;

        decimal targetCents = inputPriceWithNds * 100m;

        decimal k = Math.Round(targetCents / stepCents, 0, MidpointRounding.AwayFromZero);
        decimal withNdsCents = k * stepCents;

        correctedPriceWithNds = withNdsCents / 100m;

        decimal withoutNdsCents = withNdsCents * 100m / q;
        correctedPriceWithoutNds = withoutNdsCents / 100m;
    }

    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);

        while (b != 0)
        {
            int t = a % b;
            a = b;
            b = t;
        }

        return a;
    }
}