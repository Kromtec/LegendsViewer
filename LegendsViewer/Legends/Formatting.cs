using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;

namespace LegendsViewer.Legends
{
    public static class Formatting
    {
        public static string InitCaps(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name).Replace(" The", " the").Replace(" Of", " of");
        }

        public static string MakePopulationPlural(string population)
        {
            string ending = "";

            if (population.Contains(" of"))
            {
                ending = population.Substring(population.IndexOf(" of"), population.Length - population.IndexOf(" of"));
                population = population.Substring(0, population.IndexOf(" of"));
            }

            if (population.EndsWith("Men") || population.EndsWith("men") || population == "Humans")
            {
                return population + ending;
            }

            if (population.EndsWith("s") && !population.EndsWith("ss"))
            {
                return population + ending;
            }

            if (population == "Human")
            {
                population = "Humans";
            }
            else if (population.EndsWith("Man"))
            {
                population = population.Replace("Man", "Men");
            }
            else if (population.EndsWith("man") && !population.Contains("Human"))
            {
                population = population.Replace("man", "men");
            }
            else if (population.EndsWith("Woman"))
            {
                population = population.Replace("Woman", "Women");
            }
            else if (population.EndsWith("woman"))
            {
                population = population.Replace("woman", "women");
            }
            else if (population.EndsWith("f"))
            {
                population = population.Substring(0, population.Length - 1) + "ves";
            }
            else if (population.EndsWith("x") || population.EndsWith("ch") || population.EndsWith("sh") || population.EndsWith("s"))
            {
                population += "es";
            }
            else if (population.EndsWith("y") && !population.EndsWith("ay") && !population.EndsWith("ey") && !population.EndsWith("iy") && !population.EndsWith("oy") && !population.EndsWith("uy"))
            {
                population = population.Substring(0, population.Length - 1) + "ies";
            }
            else if (!population.EndsWith("i") && !population.EndsWith("le"))
            {
                population += "s";
            }

            if (ending != "")
            {
                population += ending;
            }

            return population;
        }

        public static string FormatRace(string race)
        {
            if (race.Contains("DEMON"))
            {
                return "Demon";
            }

            if (race.Contains("FORGOTTEN"))
            {
                return "Forgotten Beast";
            }

            if (race.Contains("NIGHT_CREATURE"))
            {
                return "Night Creature";
            }

            if (race.Contains("TITAN"))
            {
                return "Titan";
            }

            return InitCaps(race.Replace('_', ' ').ToLower());

        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9' || str[i] >= 'A' && str[i] <= 'z' || str[i] == '.' || str[i] == '_')
                {
                    sb.Append(str[i]);
                }
            }

            return sb.ToString();
        }

        public static string AddArticle(string text)
        {
            if (text.ToLower().StartsWith("a") || 
                text.ToLower().StartsWith("e") || 
                text.ToLower().StartsWith("i") || 
                text.ToLower().StartsWith("o") || 
                text.ToLower().StartsWith("u"))
            {
                text = "an " + text;
            }
            else
            {
                text = "a " + text;
            }

            return text;
        }

        public static string ReplaceNonAscii(string name)
        {
            name = name.Replace("\u017D", "a");
            name = name.Replace("\u017E", "a");
            name = name.Replace("\u201E", "a");
            name = name.Replace("\u0192", "a");
            name = name.Replace("\u008F", "a");
            name = name.Replace("\u2020", "a");
            name = name.Replace("\u00A0", "a");
            name = name.Replace("\u2026", "a");
            name = name.Replace("\u02C6", "e");
            name = name.Replace("\u2030", "e");
            name = name.Replace("\u201A", "e");
            name = name.Replace("\u0220", "e");
            name = name.Replace("\u0160", "e");
            name = name.Replace("\u0090", "e");
            name = name.Replace("\u2039", "i");
            name = name.Replace("\u00A1", "i");
            name = name.Replace("\u008D", "i");
            name = name.Replace("\u0152", "i");
            name = name.Replace("\u00A4", "n");
            name = name.Replace("\u00A5", "n");
            name = name.Replace("\u201D", "o");
            name = name.Replace("\u00A2", "o");
            name = name.Replace("\u2022", "o");
            name = name.Replace("\u201C", "o");
            name = name.Replace("\u2122", "o");
            name = name.Replace("\u2014", "u");
            name = name.Replace("\u2013", "u");
            name = name.Replace("\u00A3", "u");
            name = name.Replace("\u02DC", "y");
            return name;
        }

        public static Location ConvertToLocation(string coordinates)
        {
            int[] coords = Array.ConvertAll(coordinates.Split(','), int.Parse); // 0.520
            return new Location(coords[0], coords[1]); 
        }

        public static void ResizeImage(Bitmap source, ref Bitmap dest, int height, int width, bool keepRatio, bool smooth = true)
        {
            Size imageSize = new Size();
            if (keepRatio)
            {
                var resizePercent = source.Width > source.Height
                    ? height / Convert.ToDouble(source.Width)
                    : height / Convert.ToDouble(source.Height);
                imageSize.Width = Convert.ToInt32(source.Width * resizePercent);
                imageSize.Height = Convert.ToInt32(source.Height * resizePercent);
            }
            else
            {
                imageSize.Width = width;
                imageSize.Height = height;
            }

            dest = new Bitmap(imageSize.Width, imageSize.Height);
            using (Graphics g = Graphics.FromImage(dest))
            {

                if (smooth)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.None;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(0, 0, 50)))
                {
                    g.FillRectangle(fillBrush, 0, 0, height, height);
                }

                g.DrawImage(source, new Rectangle(0, 0, imageSize.Width, imageSize.Height), new Rectangle(0, 0, source.Width, source.Height), GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        public static Color HsvToRgb(double h, double s, double v)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            while (h < 0) { h += 360; };
            while (h >= 360) { h -= 360; };
            double r, g, b;
            if (v <= 0)
            { r = g = b = 0; }
            else if (s <= 0)
            {
                r = g = b = v;
            }
            else
            {
                double hf = h / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = v * (1 - s);
                double qv = v * (1 - s * f);
                double tv = v * (1 - s * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        r = v;
                        g = tv;
                        b = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        r = qv;
                        g = v;
                        b = pv;
                        break;
                    case 2:
                        r = pv;
                        g = v;
                        b = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        r = pv;
                        g = qv;
                        b = v;
                        break;
                    case 4:
                        r = tv;
                        g = pv;
                        b = v;
                        break;

                    // Red is the dominant color

                    case 5:
                        r = v;
                        g = pv;
                        b = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        r = v;
                        g = tv;
                        b = pv;
                        break;
                    case -1:
                        r = v;
                        g = pv;
                        b = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        r = g = b = v; // Just pretend its black/white
                        break;
                }
            }
            int red = Clamp((int)(r * 255.0));
            int green = Clamp((int)(g * 255.0));
            int blue = Clamp((int)(b * 255.0));
            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private static int Clamp(int i)
        {
            if (i < 0)
            {
                return 0;
            }

            if (i > 255)
            {
                return 255;
            }

            return i;
        }

        public static string TimeCountToSeason(int count)
        {
            string seasonString = string.Empty;
            int month = count % 100800;
            if (month <= 33600)
            {
                seasonString += "early ";
            }
            else if (month <= 67200)
            {
                seasonString += "mid";
            }
            else if (month <= 100800)
            {
                seasonString += "late ";
            }

            int season = count % 403200;
            if (season < 100800)
            {
                seasonString += "spring";
            }
            else if (season < 201600)
            {
                seasonString += "summer";
            }
            else if (season < 302400)
            {
                seasonString += "autumn";
            }
            else if (season < 403200)
            {
                seasonString += "winter";
            }

            return seasonString;
        }

        public static string AddOrdinal(int num)
        {
            if (num <= 0)
            {
                return num.ToString();
            }

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }

        public static string IntegerToWords(long inputNum)
        {
            int level = 0;

            string retval = "";
            string[] ones ={
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
              };
            string[] tens ={
                "zero",
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
              };
            string[] thou ={
                "",
                "thousand",
                "million",
                "billion",
                "trillion",
                "quadrillion",
                "quintillion"
              };

            bool isNegative = false;
            if (inputNum < 0)
            {
                isNegative = true;
                inputNum *= -1;
            }

            if (inputNum == 0)
                return "zero";

            string s = inputNum.ToString();

            while (s.Length > 0)
            {
                // Get the three rightmost characters
                var x = s.Length < 3 ? s : s.Substring(s.Length - 3, 3);

                // Separate the three digits
                var threeDigits = int.Parse(x);
                var lasttwo = threeDigits % 100;
                var dig1 = threeDigits / 100;
                var dig2 = lasttwo / 10;
                var dig3 = threeDigits % 10;

                // append a "thousand" where appropriate
                if (level > 0 && dig1 + dig2 + dig3 > 0)
                {
                    retval = thou[level] + " " + retval;
                    retval = retval.Trim();
                }

                // check that the last two digits is not a zero
                if (lasttwo > 0)
                {
                    if (lasttwo < 20) // if less than 20, use "ones" only
                        retval = ones[lasttwo] + " " + retval;
                    else // otherwise, use both "tens" and "ones" array
                        retval = tens[dig2] + " " + ones[dig3] + " " + retval;
                }

                // if a hundreds part is there, translate it
                if (dig1 > 0)
                    retval = ones[dig1] + " hundred " + retval;

                s = s.Length - 3 > 0 ? s.Substring(0, s.Length - 3) : "";
                level++;
            }

            while (retval.IndexOf("  ", StringComparison.Ordinal) > 0)
            {
                retval = retval.Replace("  ", " ");
            }

            retval = retval.Trim();

            if (isNegative)
            {
                retval = "negative " + retval;
            }

            return retval;
        }
    }
}

