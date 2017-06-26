using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Models
{
    public class CalculatorModel
    {
        public string Display { get; set; }
        public bool clearDisplay;
        public string First { get; set; }
        public string Second { get; set; }
        public string Operate { get; set; }
        public string LastOperate { get; set; }
        public string disabled;
        public bool frozen;
        public string result;

        public CalculatorModel()
        {
            Clear();
        }
        
        public void Clear()
        {
            Display = "0";
            clearDisplay = false;
            frozen = false;
            result = string.Empty;
            LastOperate = string.Empty;
            disabled= string.Empty;
            First = string.Empty;
            Second = string.Empty;
            Operate = string.Empty;
        }

        public void Process(string press)
        {
            switch (press)
            {
                case "+/-":
                    if (Display == "0")
                        break;
                    else if (Display.Contains("-"))
                    {
                        Display = Display.Remove(Display.IndexOf("-"), 1);
                    }
                    else
                        Display = "-" + Display;
                    break;

                case ".":
                    var d = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                    if (Display == "0")
                        Display = "0" + d;
                    else
                    {
                        if (!Display.Contains(d))
                            Display = Display + d;
                    }
                    break;

                case "C":
                    Clear();
                    break;

                default:
                    if (Display == "0" || clearDisplay)
                        Display = press;
                    else
                        Display = Display + press;
                    break;
            }
            if (press != ".")
                clearDisplay = false;
        }

        private void DoResult()
        {
            double tempResult;

            switch (Operate)
            {
                case ("+"):
                    tempResult = Convert.ToDouble(First) + Convert.ToDouble(Second);
                    break;

                case ("-"):
                    tempResult = Convert.ToDouble(First) - Convert.ToDouble(Second);
                    break;

                case ("*"):
                    tempResult = Convert.ToDouble(First) * Convert.ToDouble(Second);
                    break;

                case ("/"):
                    tempResult = Convert.ToDouble(First) / Convert.ToDouble(Second);
                    break;

                case ("sqrt"):
                    tempResult = Math.Sqrt(Convert.ToDouble(First));
                    break;

                case ("%"):
                    tempResult = Convert.ToDouble(First) * Convert.ToDouble(Second) / 100;
                    break;

                default:
                    tempResult = Double.NaN;
                    break;
            }
            result = tempResult.ToString();
            Display = result;
            if (Double.IsNaN(tempResult) || Double.IsNegativeInfinity(tempResult) || Double.IsPositiveInfinity(tempResult))
            {
                frozen = true;
                disabled = "disable";
                Display = "ERR";
            }
        }
        public void DoOperate(string operate)
        {
            if (First == string.Empty || (clearDisplay && LastOperate != "="))
            {
                First = Display;
                LastOperate = operate;
                clearDisplay = true;
            }
            else
            {
                Second = Display;
                Operate = LastOperate;
                DoResult();
                LastOperate = operate;
                First = result;
                clearDisplay = true;
            }

            if (operate == "=")
            {
                First = string.Empty;
                Second = string.Empty;
                Operate = string.Empty;
            }
            else if (operate == "sqrt")
            {
                First = Display;
                Operate = operate;
                DoResult();
                LastOperate = operate;
                First = string.Empty;
                clearDisplay = true;
            }
        }
    }
}
