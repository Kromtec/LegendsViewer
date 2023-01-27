using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace LegendsViewer.Legends
{
    public static class Formatting
    {
        public static string InitCaps(string text)
        {
            char[] newText = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0 || newText[i - 1] == ' ' &&
                    !(text[i] == 't' && i + 2 < text.Length && (text[i + 1] == 'h' || text[i + 1] == 'H') && (text[i + 2] == 'e' || text[i + 2] == 'E')) &&
                    !(text[i] == 'o' && i + 1 < text.Length && (text[i + 1] == 'f' || text[i + 1] == 'f')))
                {
                    newText[i] = char.ToUpper(text[i]);
                }
                else
                {
                    newText[i] = text[i] == '_' ? ' ' : char.ToLower(text[i]);
                }
            }
            return string.Intern(new string(newText));
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
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
            return race.Contains("FORGOTTEN") ? "Forgotten Beast" : InitCaps(race);
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
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }
            return text[0] == 'a' || text[0] == 'A' ||
                text[0] == 'e' || text[0] == 'E' ||
                text[0] == 'i' || text[0] == 'I' ||
                text[0] == 'o' || text[0] == 'O' ||
                text[0] == 'u' || text[0] == 'U'
                ? $"an {text}"
                : $"a {text}";
        }

        public static string ReplaceNonAscii(string name)
        {
            name = name.Replace("\u017D", "a")
                .Replace("\u017E", "a")
                .Replace("\u201E", "a")
                .Replace("\u0192", "a")
                .Replace("\u008F", "a")
                .Replace("\u2020", "a")
                .Replace("\u00A0", "a")
                .Replace("\u2026", "a")
                .Replace("\u02C6", "e")
                .Replace("\u2030", "e")
                .Replace("\u201A", "e")
                .Replace("\u0220", "e")
                .Replace("\u0160", "e")
                .Replace("\u0090", "e")
                .Replace("\u2039", "i")
                .Replace("\u00A1", "i")
                .Replace("\u008D", "i")
                .Replace("\u0152", "i")
                .Replace("\u00A4", "n")
                .Replace("\u00A5", "n")
                .Replace("\u201D", "o")
                .Replace("\u00A2", "o")
                .Replace("\u2022", "o")
                .Replace("\u201C", "o")
                .Replace("\u2122", "o")
                .Replace("\u2014", "u")
                .Replace("\u2013", "u")
                .Replace("\u00A3", "u")
                .Replace("\u02DC", "y");
            return name;
        }

        public static Location ConvertToLocation(string coordinates)
        {
            var indexOfComma = coordinates.IndexOf(',');
            int x = int.Parse(coordinates.Substring(0, indexOfComma));
            int y = int.Parse(coordinates.Substring(indexOfComma + 1, coordinates.Length - indexOfComma - 1));
            return new Location(x, y);
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

        public static Color HsvToColor(double h, double s, double v)
        {
            int red, green, blue;
            HsvToRgb(h, s, v, out red, out green, out blue);
            return Color.FromArgb(red, green, blue);
        }

        public static void HsvToRgb(double hue, double saturation, double value, out int red, out int green, out int blue)
        {
            if (saturation == 0)
            {
                red = (int)(value * 255);
                green = (int)(value * 255);
                blue = (int)(value * 255);
                return;
            }

            hue /= 60;
            int i = (int)Math.Floor(hue);
            double f = hue - i;
            double p = value * (1 - saturation);
            double q = value * (1 - saturation * f);
            double t = value * (1 - saturation * (1 - f));

            switch (i)
            {
                case 0:
                    red = (int)(value * 255);
                    green = (int)(t * 255);
                    blue = (int)(p * 255);
                    break;
                case 1:
                    red = (int)(q * 255);
                    green = (int)(value * 255);
                    blue = (int)(p * 255);
                    break;
                case 2:
                    red = (int)(p * 255);
                    green = (int)(value * 255);
                    blue = (int)(t * 255);
                    break;
                case 3:
                    red = (int)(p * 255);
                    green = (int)(q * 255);
                    blue = (int)(value * 255);
                    break;
                case 4:
                    red = (int)(t * 255);
                    green = (int)(p * 255);
                    blue = (int)(value * 255);
                    break;
                default:
                    red = (int)(value * 255);
                    green = (int)(p * 255);
                    blue = (int)(q * 255);
                    break;
            }
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
            var numString = num.ToString();
            if (num <= 0)
            {
                return numString;
            }

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return $"{numString}th";
            }

            switch (num % 10)
            {
                case 1:
                    return $"{numString}st";
                case 2:
                    return $"{numString}nd";
                case 3:
                    return $"{numString}rd";
                default:
                    return $"{numString}th";
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
            {
                return "zero";
            }

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
                    retval = $"{thou[level]} {retval}";
                    retval = retval.Trim();
                }

                // check that the last two digits is not a zero
                if (lasttwo > 0)
                {
                    retval = lasttwo < 20 ? $"{ones[lasttwo]} {retval}" : $"{tens[dig2]} {ones[dig3]} {retval}";
                }

                // if a hundreds part is there, translate it
                if (dig1 > 0)
                {
                    retval = ones[dig1] + " hundred " + retval;
                }

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
