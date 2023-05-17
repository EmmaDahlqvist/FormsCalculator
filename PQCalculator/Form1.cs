using System.Data;
using System;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;
using System.Reflection;

namespace PQCalculator
{
    public partial class Form1 : Form
    {
        string numbers;

        public Form1( )
        {
            InitializeComponent();
        }


        private void AddText(string textToAdd)
        {
            numbers += textToAdd;
            outputText.Text = numbers;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddText("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddText("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddText("9");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddText("6");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddText("5");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddText("4");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddText("3");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddText("2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddText("1");
        }

        private void button0_Click(object sender, EventArgs e)
        {
            AddText("0");
        }

        private void buttonAC_Click(object sender, EventArgs e)
        {
            //rensa allt
            outputText.Text = "";
            numbers = "";
            errorMsg.Text = "";
            x1output.Text = "";
            x2output.Text = "";
        }

        private void plus_Click(object sender, EventArgs e)
        {
            AddText(" + ");
        }

        private void minus_Click(object sender, EventArgs e)
        {
            AddText(" - ");
        }

        private void multiply_Click(object sender, EventArgs e)
        {
            AddText(" * ");

        }

        private void buttonDivide_Click(object sender, EventArgs e)
        {
            AddText(" / ");
        }


        private void buttonEquals_Click(object sender, EventArgs e)
        {
            int answer = 0;

            //beräkna kvadrat uttryck
            numbers = SquareCheck();

            if (numbers == null)
            {
                IncorrectInput();
            }
            else
            {
                try
                {
                    //beräkna
                    string value = new DataTable().Compute(numbers, null).ToString();
                    string correctValue = value.Replace(",", "."); //korrekt decimal format

                    outputText.Text = correctValue;

                    numbers = correctValue;
                } catch
                {
                    IncorrectInput();
                }
            }
        }

        private string SquareCheck()
        {
            string newNumbers = "";
            if (numbers.Contains("^"))
            {
                string[] separatedNumbers = numbers.Split(" ");
                for(int i = 0; i < separatedNumbers.Length; i++)
                {
                    
                    if (separatedNumbers[i].Contains("^"))
                    {


                        string[] separatedNumbersSplit = separatedNumbers[i].Split('^');
                        if(separatedNumbersSplit.Length == 2)
                        {

                            try
                            {
                                double newVal = Math.Pow(double.Parse(separatedNumbersSplit[0]), double.Parse(separatedNumbersSplit[1]));
                                newNumbers = numbers.Replace(separatedNumbers[i], newVal.ToString());

                            }
                            catch (Exception ex)
                            {
                                //något är ogiltligt
                                newNumbers = null;
                            }
                        } else
                        {
                            //det är ogiltligt
                            newNumbers = null;
                        }
                    }

                }

            }
            else
            {
                newNumbers = numbers;
            }

            return newNumbers;
        }

        private void buttonDot_Click(object sender, EventArgs e)
        {
            AddText(".");
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            AddText("X");
        }

        private void buttonXSquare_Click(object sender, EventArgs e)
        {
            AddText("X\u00b2");
        }

        private void buttonSquare_Click(object sender, EventArgs e)
        {
            AddText("^");
        }

        private void IncorrectInput()
        {
            outputText.Text = "Incorrect input";
            numbers = "";
        }

        private double GetAmountX(string xString, string removeThis)
        {
            try {
                //ta bort stringen framför koefficient
                string newString = xString.Replace(removeThis, "");
                if(newString == "")
                {
                    return 1; //ingen koefficient framför
                }
                //returnera koefficienten framför X
                return double.Parse(newString);
            } catch(Exception ex)
            {
                return -1;
            }
        }

        private int GetOperator(string numberString, int numIndex)
        {
            string[] numberParts = numberString.Split(" ");

            //finns inget framför => plus
            if (numIndex == 0)
            {
                return 1;
            }

            if (numberParts[numIndex - 1] == "+")
            {
                return 1;
            }
            else if (numberParts[numIndex - 1] == "-")
            {
                return -1;
            } else if (numberParts[numIndex - 1] == "*")
            {
                IncorrectInput();
                return 0;
            } else if (numberParts[numIndex - 1] == "/")
            {
                IncorrectInput();
                return 0;
            }
            return 1;
        }

        private void SolvePQ()
        {
            //de kan finnas på två sidor om =, 2D array
            double[] xsquare = new double[] { 0, 0 };
            double[] x = new double[] { 0, 0 };
            double[] a = new double[] { 0, 0 };

            string[] equalsignSplit = numbers.Split('=');
            if (equalsignSplit.Length == 2)
            {
                for (int i = 0; i < equalsignSplit.Length; i++)
                {
                    string[] termSplit = equalsignSplit[i].Split(' ');
                    for (int s = 0; s < termSplit.Length; s++)
                    {
                        //X kvadrat
                        if (termSplit[s].Contains("X\u00b2"))
                        {
                            double number = GetAmountX(termSplit[s], "X\u00b2");
                            if (number != -1)
                            {

                                xsquare[i] += GetOperator(equalsignSplit[i], s) * number;
                                Console.WriteLine("xsq: " + xsquare[i]);
                            }
                        }

                        //X
                        else if (termSplit[s].Contains("X"))
                        {
                            double number = GetAmountX(termSplit[s], "X");
                            if (number != -1)
                            {
                                x[i] += GetOperator(equalsignSplit[i], s) * number;
                                Console.WriteLine("x: " + x[i]);
                            }
                        }

                        //vanligt nummer
                        else if (IsNumber(termSplit[s]))
                        {
                            a[i] += GetOperator(equalsignSplit[i], s) * double.Parse(termSplit[s]);
                            Console.WriteLine(i + "num: " + a[i]);
                        }
                    }

                }
            }
            else
            {
                IncorrectInput();
            }

            string[] answers = CalculateX(x, xsquare, a);
            Console.WriteLine(answers[0] + " " + answers[1]);
            x1output.Text = answers[0];
            x2output.Text = answers[1];
        }

        private void buttonPQ_Click(object sender, EventArgs e)
        {
            if (numbers != null && numbers.Contains("="))
            {
                SolvePQ();
            } else
            {
                IncorrectInput();
            }

        }

        private bool IsNumber(string numberString)
        {
            bool isNumeric = int.TryParse(numberString, out _);
            return isNumeric;
        }

        private string[] CalculateX(double[] x, double[] xsquare, double[] numbers)
        {
            string[] answer = new string[2]; // kommer finnas två svar på X

            //Lägg ihop båda sidor av = och dividera på X kvadrat koefficient
            double xsquareSum = xsquare[0] - xsquare[1];
            if(xsquareSum != 0)
            {
                double xSum = (x[0] - x[1]) / xsquareSum;
                double numSum = (numbers[0] - numbers[1]) / xsquareSum;

                double p = xSum * -0.5;
                double numToRoot = Math.Pow(p, 2) - numSum;


                if (numToRoot < 0) //kommer bli ett komplext tal
                {
                    answer[0] = Math.Round(p, 3) + " + \u221A" + Math.Round(numToRoot, 3);
                    answer[1] = Math.Round(p, 3) + " - \u221A" + Math.Round(numToRoot, 3);
                }
                else //kommer bli reellt tal
                {
                    double root = Math.Sqrt(Math.Pow(p, 2) - numSum);
                    answer[0] = Math.Round((p + root), 3).ToString();
                    answer[1] = Math.Round((p - root), 3).ToString();
                }
            } else
            {
                double xSum = x[0] - x[1];
                double numSum = (numbers[1] - numbers[0]) / xSum;
                answer[0] = Math.Round(numSum, 3).ToString();
                answer[1] = "0";
            }
            return answer;
        }

        private bool ContainsXSquared(string input)
        {

            if (input.Contains("X\u00b2") && input.Contains("="))
            {
                return true;
            }

            return false;
        }

        private bool ContainsXOnly(string input)
        {
            if(input.Contains("X") && input.Contains("=") && !input.Contains("\"X\\u00b2\"")){
                return true;
            }
            return false;
        }

        private void buttonEqualSign_Click(object sender, EventArgs e)
        {
            AddText(" = ");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    button1_Click(sender, e);
                    break;
                case Keys.D2:
                    button2_Click(sender, e);
                    break;
                case Keys.D3:
                    button3_Click(sender, e);
                    break;
                case Keys.D4:
                    button4_Click(sender, e);
                    break;
                case Keys.D5:
                    button5_Click(sender, e);
                    break;
                case Keys.D6:
                    button6_Click(sender, e);
                    break;
                case Keys.D8:
                    button8_Click(sender, e);
                    break;
                case Keys.D9:
                    button9_Click(sender, e);
                    break;
                case Keys.Escape:
                    buttonAC_Click(sender, e);
                    break;
                case Keys.Oemplus:
                    plus_Click(sender, e);
                    break;
                case Keys.OemMinus:
                    minus_Click(sender, e);
                    break;
                case Keys.Enter:
                    buttonEquals_Click(sender, e);
                    break;
            }

            if(e.KeyCode == Keys.D0 && e.Shift)
            {
                buttonEqualSign_Click(sender, e);
            } else if(e.KeyCode == Keys.D0)
            {
                button0_Click(sender, e);
            }
            if(e.KeyCode == Keys.OemQuestion && e.Shift)
            {
                multiply_Click(sender, e);
            }
            if(e.KeyCode == Keys.D7 && e.Shift)
            {
                buttonDivide_Click(sender, e);
            } else if(e.KeyCode == Keys.D7)
            {
                button7_Click(sender, e);
            }

        }

    }
}