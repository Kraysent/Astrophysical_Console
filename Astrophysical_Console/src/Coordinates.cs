using System;
using System.Linq;

namespace Astrophysical_Console
{
    public class Coordinates
    {
        private int raH;
        private int raM;
        private int raS;
        private int decD;
        private int decM;
        private int decS;

        public Coordinates(int raH, int raM, int raS, int decD, int decM, int decS)
        {
            this.raH = raH;
            this.raM = raM;
            this.raS = raS;
            this.decD = decD;
            this.decM = decM;
            this.decS = decS;
        }

        public Coordinates(int raSeconds, int decSeconds)
        {
            raH = raSeconds / 3600;
            raM = (raSeconds % 3600) / 60;
            raS = raSeconds - 3600 * raH - 60 * raM;

            decD = decSeconds / 3600;
            decM = (decSeconds % 3600) / 60;
            decS = decSeconds - decD * 3600 - decM * 60;
        }

        public Coordinates(string ra, string dec, char splitChar)
        {
            int[] raArr = ra.Split(splitChar).Select(x => int.Parse(x)).ToArray();
            int[] decArr = ra.Split(splitChar).Select(x => int.Parse(x)).ToArray();

            raH = raArr[0];
            raM = raArr[1];
            raS = raArr[2];
            decD = decArr[0];
            decM = decArr[1];
            decS = decArr[2];
        }

        /// <summary>
        /// Coordinate's right ascension hours value
        /// </summary>
        public int RAH
        {
            get => raH;
            set
            {
                if (value >= 0 && value < 24)
                    raH = value;
                else throw new InvalidOperationException("Right ascension hours value can not be more than 24 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's right ascension minutes value
        /// </summary>
        public int RAM
        {
            get => raM;
            set
            {
                if (value >= 0 && value < 60)
                    raM = value;
                else throw new InvalidOperationException("Right ascension minutes value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's right ascension seconds value
        /// </summary>
        public int RAS
        {
            get => raS;
            set
            {
                if (value >= 0 && value < 60)
                    raS = value;
                else throw new InvalidOperationException("Right ascension seconds value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline degrees value
        /// </summary>
        public int DecD
        {
            get => decD;
            set
            {
                if (value >= 0 && value < 360)
                    decD = value;
                else throw new InvalidOperationException("Decline degrees value can not be more than 360 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline minutes value
        /// </summary>
        public int DecM
        {
            get => decM;
            set
            {
                if (value >= 0 && value < 60)
                    decM = value;
                else throw new InvalidOperationException("Decline minutes value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline seconds value
        /// </summary>
        public int DecS
        {
            get => decS;
            set
            {
                if (value >= 0 && value < 60)
                    decS = value;
                else throw new InvalidOperationException("Decline seconds value can not be more than 60 and less than 0.");
            }
        }

        /// <summary>
        /// Converts RA coordinate to RA seconds
        /// </summary>
        public int RASeconds => RAH * 3600 + RAM * 60 + RAS;
        /// <summary>
        /// Converts decline coordinate to decline seconds
        /// </summary>
        public int DecSeconds => DecD * 3600 + DecM * 60 + DecS;
        
        public override bool Equals(object obj1)
        {
            Coordinates obj = (Coordinates)obj1;

            if (RAH == obj.RAH &&
                RAM == obj.RAM &&
                RAS == obj.RAS &&
                DecD == obj.DecD &&
                DecM == obj.DecM &&
                DecS == obj.DecS)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return RAH.GetHashCode() ^ RAM.GetHashCode() ^ RAS.GetHashCode() ^ DecD.GetHashCode() ^ DecM.GetHashCode() ^ DecS.GetHashCode();
        }

        public override string ToString()
        {
            return RAH + "+" + RAM + "+" + RAS + "+" + DecD + "+" + DecM + "+" + DecS;
        }

        public string RAToString()
        {
            return RAH + "+" + RAM + "+" + RAS;
        }

        public string DecToString()
        {
            return DecD + "+" + DecM + "+" + DecS;
        }

        public static Coordinates operator +(Coordinates c1, Coordinates c2)
        {
            int raSecs = (c1.RAS + c2.RAS) % 60;
            int raMins = ((c1.RAS + c2.RAS) / 60 + c1.RAM + c2.RAM) % 60;
            int raHours = (((c1.RAS + c2.RAS) / 60 + c1.RAM + c2.RAM) / 60 + c1.RAH + c2.RAH) % 24;
            int decSecs = (c1.DecS + c2.DecS) % 60;
            int decMins = ((c1.DecS + c2.DecS) / 60 + c1.DecM + c2.DecM) % 60;
            int decDegrees = (((c1.DecS + c2.DecS) / 60 + c1.DecM + c2.DecM) / 60 + c1.DecD + c2.DecD) % 180;

            return new Coordinates(raHours, raMins, raSecs, decDegrees, decMins, decSecs);
        }
        public static Coordinates operator -(Coordinates c1, Coordinates c2)
        {
            int buffer = c1.RAS - c2.RAS;
            int raSecs = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.RAM - c2.RAM - 1 : c1.RAM - c2.RAM;
            int raMins = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.RAH - c2.RAH - 1 : c1.RAH - c2.RAH;
            int raHours = Residual(buffer, 24);

            buffer = c1.DecS - c2.DecS;
            int decSecs = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.DecM - c2.DecM - 1 : c1.DecM - c2.DecM;
            int decMins = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.DecD - c2.DecD - 1 : c1.DecD - c2.DecD;
            int decDegrees = Residual(buffer, 180);

            return new Coordinates(raHours, raMins, raSecs, decDegrees, decMins, decSecs);
        }

        public static double Distance(Coordinates c1, Coordinates c2) => Math.Sqrt(Math.Pow((c2.RASeconds - c1.RASeconds) / 15, 2) + Math.Pow((c2.DecSeconds - c1.DecSeconds), 2));

        private static int Residual(int n, int d)
        {
            int result = n % d;
            if (Math.Sign(result) * Math.Sign(d) < 0)
                result += d;
            return result;
        }
    }
}
