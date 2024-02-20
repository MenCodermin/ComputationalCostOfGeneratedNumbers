using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using MathNet.Numerics.Distributions;
using System.Numerics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestingGeneratedNumbersProject
{
    public partial class NumTest : Form
    {
        private BigInteger generatedNum;
        private RandomNumberGenerator rng;
        private long rangeNumMin = 0;
        private long rangeNumMax = 0;
        private int byteN = 0;
        private int previousbyteN = 0;
        private bool noRangeLimitation = false;
        public NumTest()
        {
            InitializeComponent();
            rng = RandomNumberGenerator.Create();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            byteN = 0;
        }

        private void uniDistButton_Click(object sender, EventArgs e)
        {
            GenerateRandomNumber();
        }

        private void trianDistButton_Click(object sender, EventArgs e)
        {
            GenerateTrianDistRandomNumber();
        }

        private void NumTest_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                byteN = 16;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                byteN = 8;
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                byteN = 4;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }

        private void GenerateRandomNumber()
        {
            if (!noRangeLimitation)
            {
                if (string.IsNullOrEmpty(minValTextBox.Text) && string.IsNullOrEmpty(maxValTextBox.Text))
                {
                    MessageBox.Show("Please enter a values");
                    return;
                }
                else
                {
                    rangeNumMin = long.Parse(minValTextBox.Text);
                    rangeNumMax = long.Parse(maxValTextBox.Text);
                }              
            }

            if (rangeNumMax < rangeNumMin)
            {
                MessageBox.Show("Max value must be greater than Min value.");
                return;
            }
            byte[] bytes;
            switch (byteN)
            {
                case 4:
                    {
                        bytes = new byte[byteN];
                        break;
                    }
                case 8:
                    {
                        bytes = new byte[byteN];
                        break;
                    }
                case 16:
                    {
                        bytes = new byte[byteN];
                        break;
                    }
                default:
                    {
                        bytes = new byte[0];
                        break;
                    }
            }

            Stopwatch sw = Stopwatch.StartNew(); // stopwatch 

            rng.GetBytes(bytes);

            if (!string.IsNullOrEmpty(genNumTextBox.Text) || !string.IsNullOrEmpty(compCostTextBox.Text))
            {
                genNumTextBox.Text = "";
                compCostTextBox.Text = "";
            }

            generatedNum = new BigInteger(bytes);

            BigInteger mappedNumber = generatedNum;

            if (!noRangeLimitation)
            {
                mappedNumber = MapToRange(generatedNum, rangeNumMin, rangeNumMax);
            }

            sw.Stop();// stopwatch ending point
            genNumTextBox.Text += $"{mappedNumber}";
            compCostTextBox.Text += $"Consumed Time = {sw.Elapsed}";
        }

        private BigInteger MapToRange(BigInteger number, long min, long max)
        {
            // Calculate the range of possible BigInteger values
            BigInteger range = max - min + 1; // Add 1 to include both min and max values

            // Ensure the range is positive
            if (range <= 0)
            {
                throw new ArgumentException("Invalid range: max must be greater than min.");
            }

            // Calculate the modulus of the number with the range
            BigInteger modulus = number % range;

            // Ensure the modulus is positive
            if (modulus < 0)
            {
                // Add the range to ensure the result is non-negative
                modulus += range;
            }

            // Map the modulus to the specified range
            return min + modulus;
        }

        private void noRangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            noRangeLimitation = false;
            if (noRangeCheckBox.Checked)
            {
                noRangeLimitation = true;
            }
        }

        private void GenerateTrianDistRandomNumber()
        {
            if (string.IsNullOrEmpty(minValTextBox.Text) || string.IsNullOrEmpty(maxValTextBox.Text))
            {
                MessageBox.Show("Please enter both minimum and maximum values.");
                return;
            }
            else
            {
                rangeNumMin = long.Parse(minValTextBox.Text);
                rangeNumMax = long.Parse(maxValTextBox.Text);
            }

            if (rangeNumMax < rangeNumMin)
            {
                MessageBox.Show("Max value must be greater than Min value.");
                return;
            }

            if (!string.IsNullOrEmpty(genNumTextBox.Text) || !string.IsNullOrEmpty(compCostTextBox.Text))
            {
                genNumTextBox.Text = "";
                compCostTextBox.Text = "";
            }

            Stopwatch sw = Stopwatch.StartNew(); // stopwatch 

            double generatedNumb = GenerateTriangularRandomValue(rangeNumMin, rangeNumMax);

            BigInteger mappedNumber = BigInteger.Zero;

            mappedNumber = MapToRangeTrianDist(generatedNumb, rangeNumMin, rangeNumMax);

            sw.Stop();// stopwatch ending point
            genNumTextBox.Text += $"{mappedNumber}";
            compCostTextBox.Text += $"Consumed Time = {sw.Elapsed}";
        }
        private double GenerateTriangularRandomValue(long min, long max)
        {
            // Create a triangular distribution with the specified min, max, and mode values
            Triangular triangularDistribution = new Triangular(min, max, (min + max) / 2.0);

            // Generate a random number from the triangular distribution
            return triangularDistribution.Sample();
        }

        private BigInteger MapToRangeTrianDist(double number, long min, long max)
        {
            double normalizedNumber = (number - min) / (max - min);

            long range = max - min;
            return min + (BigInteger)(normalizedNumber * range); ;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            GenerateRandomNumber();
        }
    }
}
