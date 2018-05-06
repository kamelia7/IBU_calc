using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBUcalc
{
    class Drink
    {
        private double wortSugarPercent;
        private double wortBoilingTime;
        private double hopAlphaAcidsPercent;
        private double hopWeight;
        private double drinkVolume;
        private double IBU;

        public Drink(double wsp, double wbt, double haap, double hw, double dv)
        {
            this.wortSugarPercent = wsp;
            this.wortBoilingTime = wbt;
            this.hopAlphaAcidsPercent = haap;
            this.hopWeight = hw;
            this.drinkVolume = dv;
            this.IBU = 0;
        }

        public static double Bx(double S)
        {
            return 182.4601 * Math.Pow(S, 3) - 775.6921 * Math.Pow(S, 2) + 1262.7794 * Math.Pow(S, 1) - 669.5622;
        }

        public double calculateIBU()
        {
            const double MIN_S = 1;
            const double MAX_S = 1.1790;
            const double EPS = 0.0001;
            const double MAX_UTILIZATION = 4.15;

            MathCalculations calc = new MathCalculations();
            double S = calc.HalfDivisionMethod(MIN_S, MAX_S, EPS, wortSugarPercent);

            double p1 = (1.65 * Math.Pow(0.000125, S - 1));
            double p2 = ((1 - Math.Exp(-0.04 * wortBoilingTime)) / MAX_UTILIZATION);
            double p3 = ((hopAlphaAcidsPercent / 100 * hopWeight * 1000) / drinkVolume);
            IBU = p1 * p2 * p3;
            return IBU;
        }
    }

    class MathCalculations
    {
        public double HalfDivisionMethod(double left_bound, double right_bound, double accuracy, double equals)
        {
            double new_left_bound = left_bound;
            double new_right_bound = right_bound;
            double result = new_left_bound;

            while (new_right_bound - new_left_bound > accuracy)
            {
                if ((Drink.Bx(result) - equals) * (Drink.Bx(new_right_bound) - equals) < 0)
                    new_left_bound = result;
                else
                    new_right_bound = result;
                result = (new_left_bound + new_right_bound) / 2;
            }
            return result;
        }
    }
}
